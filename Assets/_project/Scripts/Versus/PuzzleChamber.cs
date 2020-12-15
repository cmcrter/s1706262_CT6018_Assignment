using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleChamber : MonoBehaviour
{
    [SerializeField]
    LocalVersusManager versusManager;

    //The id for the chamber itself (and therefore the objects within it)
    public int chamberID;

    //All of the colour based objects within the chamber
    [SerializeField]
    List<CState> cStateObjects = new List<CState>();

    bool bPlayerExited;
    [SerializeField]
    PlayerHealth playerHealthInChamber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ContextMenu("Get Chamber Objects")]
    private void GetAllCStateObjects()
    {
        cStateObjects = GetComponentsInChildren<CState>(true).ToList();
    }

    [ContextMenu("Set Chamber Objects")]
    private void ChangeAllObjectsToCorrectState()
    {
        foreach (CState state in cStateObjects)
        {
            state.SetState(chamberID, true);
        }
    }

    //Starting the chamber timer
    public void StartChamber(float fTimer)
    {
        StartCoroutine(eChamberTimer(fTimer));
    }

    private IEnumerator eChamberTimer(float fTimer)
    {
        //Wait for the chamber timer
        yield return new WaitForSeconds(fTimer);

        if (!bPlayerExited && playerHealthInChamber && playerHealthInChamber.enabled)
        {
            //Kill player in chamber
            playerHealthInChamber.InstantKillPlayer();
        }
    }

    //Player has left the chamber (figured out the puzzle to leave)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(playerHealthInChamber.gameObject))
        {
            bPlayerExited = true;
        }
    }
}
