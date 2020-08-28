using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is on the player to keep track of the input triggers in range and to activate the correct one
public class TriggerTracker : MonoBehaviour
{
    [SerializeField]
    List<InputTrigger> triggers = new List<InputTrigger>();

    //[SerializeField]
    //CharacterManager _manager;

    private void Awake()
    {
        //_manager = _manager ?? GetComponent<CharacterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggers.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            ClosestTriggerCheck().InputTriggered();            
        }
    }

    //Getting the closest input trigger
    private InputTrigger ClosestTriggerCheck()
    {
        InputTrigger returnTrigger;

        if (triggers.Count == 1)
        {
            returnTrigger = triggers[0];
        }
        else
        {
            returnTrigger = triggers[0];
            float currentDist = Vector2.Distance(gameObject.transform.position, triggers[0].transform.position);

            for (int i = 1; i < triggers.Count; ++i)
            {
                float distToCheck = Vector2.Distance(gameObject.transform.position, triggers[i].transform.position);

                if (distToCheck < currentDist)
                {
                    currentDist = distToCheck;
                    returnTrigger = triggers[i];
                }
            }
        }

        return returnTrigger;
    }

    public void Add(InputTrigger inputTrigger)
    {
        //Need to check the colour of the input trigger vs the current colour of the player
        if (1 == 1)
        {
            triggers.Add(inputTrigger);
        }
    }

    public void Remove(InputTrigger inputTrigger)
    {
        if (1 == 1)
        {
            triggers.Remove(inputTrigger);
        }
    }
}
