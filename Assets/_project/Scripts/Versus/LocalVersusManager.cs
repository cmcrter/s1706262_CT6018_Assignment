using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An easy way to store the player's data needed
[System.Serializable]
struct playerData
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
    private bool bStartPressed = false;
    private bool bMapLoaded = false;
    private bool bPlayerWon = false;

    [SerializeField]
    playerData[] players = new playerData[4];

    [SerializeField]
    List<int> activePlayers = new List<int>();

    [SerializeField]
    List<GameObject> mapContainers = new List<GameObject>();

    [SerializeField]
    GameObject lastMapUsed;

    [SerializeField]
    Vector3[] playerStartPos = new Vector3[4];

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        activePlayers.Add(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Adding players when 
    private IEnumerator WaitingInLobby()
    {
        int knownPlayers = 1;

        while (!bStartPressed)
        {
            if (knownPlayers < players.Length)
            {
                for (int i = knownPlayers; i < players.Length; ++i)
                {
                    if (players[i].handlerUsed.isBeingUsed())
                    {
                        playerActivated(i);
                        knownPlayers++;
                    }
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

    }

    //Setting up the next combat
    private IEnumerator WaitingForArena()
    {
        //Might be going from the lobby
        if (lastMapUsed)
        {
            //Removing the last map
            lastMapUsed.SetActive(false);

            //Removing the last map from the selection to avoid repetitions
            mapContainers.Remove(lastMapUsed);
        }
        else
        {
            //This means it's the first map of the session
        }

        //Next selected map
        int iRandMap = Random.Range(0, mapContainers.Count);

        while (!bMapLoaded)
        {
            //Moving all the players to the correct spots and resetting them


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

    private void ResetPlayers()
    {

    }

    private void MovePlayersToSpots()
    {

    }


    //There's 1 player left alive
    public void WinConditionMet()
    {

    }

    //A player decides to quit
    public void PlayerQuit()
    {

    }
}
