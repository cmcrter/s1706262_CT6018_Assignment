////////////////////////////////////////////////////////////
// File: Waypoint.cs
// Author: Charles Carter
// Brief: A waypoint in the singleplayer
////////////////////////////////////////////////////////////

using UnityEngine;

public class Waypoint : MonoBehaviour
{
    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private Collider2D[] playerColliders = new Collider2D[2];
    [SerializeField]
    private WaypointManager waypointManager;
    [SerializeField]
    private ParticleSystem VFX;
    [SerializeField]
    private Collider2D _collider;
    private bool beenPassed;

    //This is the index of the background that the waypoint is on
    [SerializeField]
    private int iBackgroundAssociated;

    #endregion

    public int GetBackgroundIndex()
    {
        return iBackgroundAssociated;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //The player is passing this waypoint for the first time
        if (collision.gameObject.tag.Equals("Player") && !beenPassed)
        {
            //Technically works but there could be a computationally better way to do this
            Physics2D.IgnoreCollision(playerColliders[0], _collider);
            Physics2D.IgnoreCollision(playerColliders[1], _collider);

            beenPassed = true;
            waypointManager.LogWaypoint(this);
            VFX.Play();
        }

        //Things that need objects to destruct when going past the waypoint (things without collision on etc)
        if (collision.gameObject.TryGetComponent<IWaypointDestructable>(out var destructable))
        {
            destructable.Destruct();
        }
    }
}
