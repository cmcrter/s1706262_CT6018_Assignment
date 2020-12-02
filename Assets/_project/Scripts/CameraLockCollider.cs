using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CameraLockCollider : MonoBehaviour
{
    [SerializeField]
    Camera cameraUsed;
    CameraMovement _camMovement;

    [SerializeField]
    GameObject player;

    [SerializeField]
    float newCamSize;

    private void Awake()
    {
        cameraUsed = cameraUsed ?? Camera.main;
        _camMovement = cameraUsed.GetComponent<CameraMovement>();
        player = player ?? GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player))
        {
            if (_camMovement != null)
            {
                _camMovement.OverrideCameraPos(transform.position, newCamSize);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player))
        {
            if (_camMovement != null)
            {
                _camMovement.FreeCameraMovement();
            }
        }
    }
}
