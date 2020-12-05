using System.Collections;
using System.Collections.Generic;
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
}

//This dictates the game loop of the local versus gamemode, this will probably be a monolithic script
public class LocalVersusManager : MonoBehaviour
{
    //Different states require different bools to maintain
    private bool bStartPressed = false;
    private bool bMapLoaded = false;
    private bool bPlayerWon = false;

    [Header("Player data")]

    [SerializeField]
    private PlayerData[] players = new PlayerData[4];

    [SerializeField]
    private List<int> activePlayers = new List<int>();

    [Header("Map Data")]

    [SerializeField]
    private List<GameObject> mapContainers = new List<GameObject>();

    [SerializeField]
    private GameObject[] mapPrefabArr;

    [SerializeField]
    private GameObject lobbyContainer;

    [SerializeField]
    private GameObject lastMapUsed;
    [SerializeField]
    private GameObject currentMap;

    [SerializeField]
    private Vector3[] playerStartPos = new Vector3[4];

    [Header("Game States Data")]

    [SerializeField]
    private IEnumerator[] eVersusStates = new IEnumerator[3];

    private void Awake()
    {
        //These maps are at the default state
        mapPrefabArr = mapContainers.ToArray();
        CheckingPlayerDataVariables();
    }

    // Start is called before the first frame update
    void Start()
    {
        eVersusStates[0] = WaitingInLobby();
        eVersusStates[1] = WaitingForArena();
        eVersusStates[2] = WaitingForWinCondition();

        activePlayers.Add(0);
        StartCoroutine(eVersusStates[0]);
    }

    // Update is called once per frame
    void Update()
    {

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
    private IEnumerator WaitingInLobby()
    {
        while (!bStartPressed)
        {
            for (int i = 1; i < players.Length; ++i)
            {
                if (players[i].handlerUsed.isBeingUsed() && !activePlayers.Contains(i))
                {
                    playerActivated(i);
                }
            }
            yield return null;
        }
    }

    //A player was activated
    private void playerActivated(int playerIndex)
    {
        //Adding the index to the list
        activePlayers.Add(playerIndex);

        //Turning the gameobject on
        players[playerIndex].playerObject.SetActive(true);
    }

    //Player pulled the start lever
    public void StartLeverPulled()
    {
        if (activePlayers.Count > 1)
        {
            bStartPressed = true;
        }
    }

    //Setting up the next combat
    private IEnumerator WaitingForArena()
    {
        //Is not the first map of the session
        if (lastMapUsed)
        {
            ResetMap(lastMapUsed);
            lastMapUsed.SetActive(false);

            //Removing the last map from the selection to avoid repetitions
            mapContainers.Remove(lastMapUsed);
        }
        //This means it's the first map of the session
        else
        {
            //Turn the lobby off
            if (lobbyContainer.activeSelf)
            {
                lobbyContainer.SetActive(false);
            }
        }

        //Next selected map
        int iRandMap = Random.Range(0, mapContainers.Count);

        //This isnt as fancy as it could be
        while (!bMapLoaded)
        {
            ResetPlayers();
            mapContainers[iRandMap].SetActive(true);

            bMapLoaded = true;
            yield return null;
        }

        if (lastMapUsed)
        {
            //Adding the last map back into circulation at the end value
            mapContainers.Add(lastMapUsed);
        }

        //Cant be the same because it's added onto the end
        lastMapUsed = mapContainers[iRandMap];

        //For the next time loading a map
        bMapLoaded = false;
    }

    private void ResetMap(GameObject lastMap)
    {
        //Going through the prefabs
        for (int i = 0; i < mapPrefabArr.Length; ++i)
        {
            //Seeing if it's the last map used
            if (mapPrefabArr[i].Equals(lastMap))
            {
                //Setting it back to how it was at the start
                lastMap = mapPrefabArr[i];
            }
        }
    }

    //Making sure the players are back to how they were before each map
    private void ResetPlayers()
    {
        //Moving all the players to the correct spots and resetting them
        foreach (int index in activePlayers)
        {
            MovePlayersToSpots(index);
            players[index].playerHealth.ResetHealth();
        }
    }

    //This will be changed to a lerp soon
    private void MovePlayersToSpots(int index)
    {
        players[index].playerObject.transform.position = playerStartPos[players[index].playerState.returnID()];
    }

    //I dont really have a use for this yet...
    IEnumerator WaitingForWinCondition()
    {
        while (!bPlayerWon)
        {
            yield return null;
        }
    }


    //There's 1 player left alive
    public void WinConditionMet()
    {
        bPlayerWon = true;
    }

    //A player decides to quit
    public void PlayerQuit(int index)
    {
        //Too many people disconnected
        if (activePlayers.Count < 2)
        {
            ReturnToLobby();
        }
        else
        {
            activePlayers.RemoveAt(index);
        }
    }

    //The group decides to return to the lobby or too many people quit
    private void ReturnToLobby()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
