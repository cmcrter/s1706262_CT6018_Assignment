using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The camera's movement during singleplayer
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform ObjectToFollow;

    public bool isLocked;

    private void Start()
    {
        StartCoroutine(Co_timeBeforeCameraFollow());
    }

    private void Update()
    {
        if (!isLocked)
        {
            transform.position = new Vector3(ObjectToFollow.position.x, ObjectToFollow.position.y + 8.7f, transform.position.z);
        }
    }

    private IEnumerator Co_timeBeforeCameraFollow()
    {
        isLocked = true;

        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            yield return null;
        }

        isLocked = false;
    }

    public void OverrideCameraPos(Vector3 newPos)
    {
        isLocked = true;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    public void FreeCameraMovement()
    {
        isLocked = false;
    }
}
