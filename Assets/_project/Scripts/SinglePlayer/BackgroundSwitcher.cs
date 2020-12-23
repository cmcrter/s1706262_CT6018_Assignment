////////////////////////////////////////////////////////////
// File: BackgroundSwitcher.cs
// Author: Charles Carter
// Brief: A trigger to let the background manager change active backgrounds
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needs an edge collider 2D
[RequireComponent(typeof(EdgeCollider2D))]
public class BackgroundSwitcher : MonoBehaviour
{
    [SerializeField]
    BackgroundManager backgroundManager;
    [SerializeField]
    Rigidbody2D playerRB;

    private void Awake()
    {
        backgroundManager = backgroundManager ?? GameObject.FindGameObjectWithTag("BackgroundManager").GetComponent<BackgroundManager>();
        playerRB = playerRB ?? GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            //The player is going right
            if (playerRB.velocity.x > 0)
            {
                //turn on the next background
                backgroundManager.SwitchActiveBackground(true);
            }
            //The player is going left
            else if (playerRB.velocity.x < 0)
            {
                //turn on the previous background
                backgroundManager.SwitchActiveBackground(false);
            }
        }
    }
}
