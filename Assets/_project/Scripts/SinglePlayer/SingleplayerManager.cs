////////////////////////////////////////////////////////////
// File: SingleplayerManager.cs
// Author: Charles Carter
// Brief: A class to handle some of the singleplayer game loop (tutorial skipping etc)
////////////////////////////////////////////////////////////

using UnityEngine;

public class SingleplayerManager : MonoBehaviour
{
    #region Class Variables

    private bool bTutorialDoneOrSkipped;

    [Header("Variables Needed to set up tutorial skipping")]
    [SerializeField]
    private UIPanel TutorialSkipPanel;
    [SerializeField]
    private SingeplayerPauseMenu singeplayerPause;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject TutorialSectionContainer;
    [SerializeField]
    private WaypointManager waypointManager;
    [SerializeField]
    private TutorialFinishedTrigger FinishedTrigger;

    #endregion

    private void Awake()
    {
        LoadSelection();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!bTutorialDoneOrSkipped)
        {
            OpenTutorialSkipPanel();
        }
        else
        {
            SkipTutorial();
        }
    }

    void OpenTutorialSkipPanel()
    {
        //Player will always be yellow at this point
        singeplayerPause.OpenMenu(0, TutorialSkipPanel);
    }

    //Not part of the savable interface because it should be triggering once in the playthrough
    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("TutorialOver", 0);
        FinishedTrigger.gameObject.SetActive(false);

        //Large null check
        if (player)
        {
            //If there's a waypoint manager
            if (waypointManager)
            {
                //move the player to the first waypoint's position (it will be the first one after the tutorial)
                player.transform.position = waypointManager.WaypointPos(0);
                waypointManager.StartBackground();
            }
            else
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Set waypoint manager in inspector", this);
                }
            }

            if (TutorialSectionContainer)
            {
                //Setting the tutorial section objects to false (apart from the pieces seen at the door)
                TutorialSectionContainer.SetActive(false);
            }

            PlayTutorial();
        }
        else
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Cant skip tutorial section, make sure all variables are set in the inspector", this);
            }
        }
    }

    //Slightly unnecessary but it's a prototype
    public void PlayTutorial()
    {
        singeplayerPause.CloseMenu();
    }

    //The player went through the tutorial
    public void TutorialFinished()
    {
        PlayerPrefs.SetInt("TutorialOver", 0);
    }

    private void LoadSelection()
    {
        if (PlayerPrefs.HasKey("TutorialOver"))
        {
            bTutorialDoneOrSkipped = true;
        }
    }
}
