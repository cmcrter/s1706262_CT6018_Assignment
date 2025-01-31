﻿////////////////////////////////////////////////////////////
// File: LocalVersusManager.cs
// Author: Charles Carter
// Brief: The manager class that controls the local versus gamemode
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//A container class to store the player's data needed
[System.Serializable]
internal class PlayerData
{
    //All of the aspects of the player needed
    public GameObject playerObject;
    public InputHandler handlerUsed;
    public CState playerState;
    public Movement2D playerMovement;
    public Rigidbody2D playerRigidbody;
    public PlayerHealth playerHealth;
    public WeaponManager playerWeaponManager;
}

//This dictates the game loop of the local versus gamemode, this will probably be a monolithic script
public class LocalVersusManager : MonoBehaviour
{
    #region Class Variables

    //Different states require different bools to maintain
    private bool bStartPressed = false;
    private bool bMapLoaded = false;
    private bool bPlayerWon = false;

    [Header("Player data")]
    //All the player data needed
    [SerializeField]
    private PlayerData[] players = new PlayerData[4];
    //The ones currently active in game
    [SerializeField]
    private List<int> activePlayers = new List<int>();

    [Header("Map Data")]
    //The current maps to choose from
    [SerializeField]
    private List<GameObject> mapContainers = new List<GameObject>();
    //The map prefabs
    [SerializeField]
    private GameObject[] mapPrefabArr;

    [Header("Lobby Objects")]
    //The lobby gameobject and UI
    [SerializeField]
    private GameObject lobbyContainer;
    [SerializeField]
    private GameObject lobbyUI;
    [SerializeField]
    private Lever startLever;
    [SerializeField]
    private Camera cameraBeingUsed;
    [SerializeField]
    private int lobbyCameraSize;
    [SerializeField]
    private int puzzleCameraSize;
    [SerializeField]
    private int battleCameraSize;

    [Header("Map randomization variables")]
    //Keeping track of previous and current map
    [SerializeField]
    private int currentMapIndex = 0;
    [SerializeField]
    private GameObject currentMap;
    [SerializeField]
    private Transform currentMapParent;
    [SerializeField]
    private List<PuzzleChamber> puzzleChambers = new List<PuzzleChamber>();
    //And default player positions
    [SerializeField]
    private Vector3[] playerStartPos = new Vector3[4];

    [Header("Game States Data")]
    [SerializeField]
    private float fPuzzlesTimer = 15f;
    [SerializeField]
    private int iPlayersAlive = 4;
    [SerializeField]
    private LocalVersusPauseMenu pauseMenu;

    #endregion

    private void Awake()
    {
        //Making sure all the player variables are correct and set (costly)
        CheckingPlayerDataVariables();
        startLever = startLever ?? GameObject.FindObjectOfType<Lever>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMapIndex = -1;
        activePlayers.Add(0);
        mapContainers = mapPrefabArr.ToList();
        cameraBeingUsed.orthographicSize = lobbyCameraSize;
        StartCoroutine(Co_WaitingForLobby());
    }

    private void CheckingPlayerDataVariables()
    {
        //Going through each player
        foreach (PlayerData player in players)
        {
            //Making sure they have an object
            if (player.playerObject)
            {
                //Going through and making sure they have each component
                player.playerMovement = player.playerMovement ?? player.playerObject.GetComponent<Movement2D>();
                player.playerHealth = player.playerHealth ?? player.playerObject.GetComponent<PlayerHealth>();
                player.playerRigidbody = player.playerRigidbody ?? player.playerObject.GetComponent<Rigidbody2D>();
                player.handlerUsed = player.handlerUsed ?? player.playerMovement.GetInputHandler();
                player.playerState = player.playerState ?? player.playerObject.GetComponent<CState>();
                player.playerWeaponManager = player.playerWeaponManager ?? player.playerObject.GetComponent<WeaponManager>();
            }
            else
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Player Objects not set");
                }

                //Shouldn't run the script
                enabled = false;
            }
        }
    }

    //Adding players when 
    private IEnumerator Co_WaitingForLobby()
    {
        while (!bStartPressed)
        {
            for (int i = 1; i < players.Length; ++i)
            {
                if (players[i].handlerUsed.isBeingUsed() && !activePlayers.Contains(i))
                {
                    playerActivated(i);
                    if (startLever.isLocked)
                    {
                        startLever.isLocked = false;
                    }
                }
            }
            yield return null;
        }

        StartCoroutine(Co_WaitingForMapLoad());
    }

    //A player was activated
    private void playerActivated(int playerIndex)
    {
        //Adding the index to the list
        activePlayers.Add(playerIndex);
        SetUpPauseMenu(playerIndex);

        //Turning the gameobject on
        players[playerIndex].playerObject.SetActive(true);
    }

    //Player pulled the start lever and the countdown ended
    public void StartLeverPulled()
    {
        if (activePlayers.Count > 1)
        {
            cameraBeingUsed.orthographicSize = battleCameraSize;

            bStartPressed = true;
        }
    }

    private void SetUpPauseMenu(int iAddedPlayer)
    {
        pauseMenu.AddHandler(players[iAddedPlayer].handlerUsed);
    }

    //Setting up the next combat
    private IEnumerator Co_WaitingForMapLoad()
    {
        //Is not the first map of the session
        if (currentMapIndex != -1)
        {
            //Removing the last map
            Destroy(currentMap);
        }
        //This means it's the first map of the session
        else
        {
            //Turn the lobby off
            if (lobbyContainer.activeSelf)
            {
                lobbyContainer.SetActive(false);
                lobbyUI.SetActive(false);
            }
        }

        //Next selected map (Range's second number is excluded)
        int iRandMap = Random.Range(0, mapContainers.Count);

        //Then put the players down so they dont interact with the new map
        ResetPlayers();

        //While the map isnt loaded
        while (!bMapLoaded)
        {
            //Load the map
            if (currentMapParent)
            {
                currentMap = Instantiate(mapContainers[iRandMap], currentMapParent);
                currentMap.SetActive(true);
            }
            else
            {
                currentMap = Instantiate(mapContainers[iRandMap]);
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Set Map Parent in inspector");
                }
            }

            bMapLoaded = true;
            yield return null;
        }

        //This is the current map index
        currentMapIndex = iRandMap;

        //For the next time loading a map
        bMapLoaded = false;

        //Removing the current map from the potential maps next time
        mapContainers.Clear();
        for (int i = 0; i < mapPrefabArr.Length; ++i)
        {
            //Using the tag to identify the current object
            if (mapPrefabArr[i].tag == currentMap.tag)
            {
                continue;
            }

            mapContainers.Add(mapPrefabArr[i]);
        }

        //Going to the next game state
        StartCoroutine(Co_PuzzleTimer());
    }

    //Making sure the players are back to how they were before each map
    private void ResetPlayers()
    {
        //Moving all the players to the correct spots and resetting them
        foreach (int index in activePlayers)
        {
            MovePlayersToSpots(index);
            players[index].playerWeaponManager.ResetWeapon();
            players[index].playerHealth.ResetHealth();
        }
    }

    //This will be changed to a lerp soon
    private void MovePlayersToSpots(int index)
    {
        players[index].playerObject.transform.position = playerStartPos[players[index].playerState.returnID()];
    }

    //The puzzle phase of the game
    IEnumerator Co_PuzzleTimer()
    {
        cameraBeingUsed.orthographicSize = puzzleCameraSize;
        puzzleChambers = currentMap.GetComponent<VersusMap>().GetChambers();

        if (puzzleChambers != null)
        {
            //Going through that active players' chambers
            for (int i = 0; i < activePlayers.Count; ++i)
            {
                if (puzzleChambers[i])
                {
                    //And starting them
                    puzzleChambers[i].StartChamber(fPuzzlesTimer);
                }
            }
        }

        yield return new WaitForSeconds(fPuzzlesTimer + 1f);

        StartCoroutine(Co_ArenaCooldown());
    }

    //I dont really have a use for this yet...
    IEnumerator Co_ArenaCooldown()
    {
        cameraBeingUsed.orthographicSize = battleCameraSize;
        iPlayersAlive = activePlayers.Count;

        while (!bPlayerWon)
        {
            yield return null;
        }

        bPlayerWon = false;

        //A little gloat time
        yield return new WaitForSeconds(2.0f);

        //Restarting the cycle
        StartCoroutine(Co_WaitingForMapLoad());
    }

    //One of the players died
    public void PlayerDied(int index)
    {
        iPlayersAlive--;

        if (iPlayersAlive == 1)
        {
            WinConditionMet();
        }
    }

    //There's 1 player left alive
    private void WinConditionMet()
    {
        bPlayerWon = true;
    }

    //A player decides to quit (from a specific panel)
    public void PlayerQuit(UIPanel panel)
    {
        //Making sure there is a panel
        if (!panel)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Assign panel in the inspector", this);
            }
            return;
        }

        //They arent in the lobby anymore
        if (bStartPressed)
        {
            //Too many people disconnected
            if (activePlayers.Count < 2)
            {
                ReturnToLobby();
            }
            else
            {
                //Removing them from the game using the panel's ID
                players[panel.GetPanelID()].playerHealth.InstantKillPlayer();
                activePlayers.RemoveAt(panel.GetPanelID());
            }
        }
        else
        {
            //Removing them from the game using the panel's ID
            players[panel.GetPanelID()].playerHealth.InstantKillPlayer();
            activePlayers.RemoveAt(panel.GetPanelID());

            if (activePlayers.Count < 2)
            {
                startLever.isLocked = true;
            }
        }
    }

    //The group decides to return to the lobby or too many people quit
    public void ReturnToLobby()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetActivePlayerCount()
    {
        return activePlayers.Count;
    }

    public GameObject GetPlayerObject(int index)
    {
        return players[index].playerObject;
    }
}