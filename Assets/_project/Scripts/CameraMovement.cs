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

    Vector3 offset;

    private void Start()
    {
        offset = transform.position;
        StartCoroutine(Co_timeBeforeCameraFollow(0.02f));
    }

    private void Update()
    {
        if (!isLocked)
        {
            transform.position = new Vector3(ObjectToFollow.position.x + offset.x, ObjectToFollow.position.y + offset.y, -20);
        }
    }

    private IEnumerator Co_timeBeforeCameraFollow(float timer)
    {
        isLocked = true;

        for (float t = 0; t < timer; t += Time.deltaTime)
        {
            yield return null;
        }

        isLocked = false;
    }

    public void OverrideCameraPos(Vector3 newPos)
    {
        isLocked = true;
        newPos = new Vector3(newPos.x, newPos.y, transform.position.z);
        StartCoroutine(Co_MoveCamerToPoint(newPos, 1.0f));
    }

    public void FreeCameraMovement()
    {
        isLocked = false;
    }

    private IEnumerator Co_MoveCamerToPoint(Vector3 newPos, float timer)
    {
        for (float t = 0; t < timer; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, t);
            yield return null;
        }
    }
}
