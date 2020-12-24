////////////////////////////////////////////////////////////
// File: TutorialFinishedTrigger.cs
// Author: Charles Carter
// Brief: A script that tells the singleplayer manager when the player has finished the tutorial
//////////////////////////////////////////////////////////// 

using UnityEngine;

//A really small script that could be intergrated into a different one
public class TutorialFinishedTrigger : MonoBehaviour
{
    [SerializeField]
    private SingleplayerManager singleplayerManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The player has entered the collider
        if (collision.tag.Equals("Player"))
        {
            if (singleplayerManager)
            {
                singleplayerManager.TutorialFinished();
            }
            else
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Set the singleplayer manager in the inspector", this);
                }
            }

            gameObject.SetActive(false);
        }
    }
}
