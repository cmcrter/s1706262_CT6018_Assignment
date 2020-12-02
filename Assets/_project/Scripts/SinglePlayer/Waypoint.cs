using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    WaypointManager waypointManager;

    [SerializeField]
    ParticleSystem VFX;

    [SerializeField]
    Collider2D _collider;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            _collider.enabled = false;
            waypointManager.LogWaypoint(this);
            VFX.Play();
        }
    }
}
