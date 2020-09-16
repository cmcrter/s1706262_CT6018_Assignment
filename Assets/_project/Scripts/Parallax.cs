using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startpos;
    public float parallaxEff;
    [SerializeField]
    private GameObject pCamera;
    [SerializeField]
    SpriteRenderer sRenderer;

    private void Awake()
    {
        pCamera = pCamera ?? Camera.main.gameObject;
        sRenderer = sRenderer ?? GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        startpos = transform.position.x;
        length = sRenderer.bounds.size.x;
    }

    // Update is called once per frame
    private void Update()
    {
        float temp = (pCamera.transform.position.x * (1 - parallaxEff));
        float dist = (pCamera.transform.position.x * parallaxEff);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
