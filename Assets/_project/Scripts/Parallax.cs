////////////////////////////////////////////////////////////
// File: Parallax.cs
// Author: Dani (Edited by Charles Carter)
// Brief: To move an object based on another movements object
//////////////////////////////////////////////////////////// 
using UnityEngine;

//Script base found https://www.youtube.com/watch?v=zit45k6CUMk
public class Parallax : MonoBehaviour
{
    #region Variables Needed
    private float length;
    private float startpos;
    public float parallaxEff;
    [SerializeField]
    private GameObject goTrackingObject;
    [SerializeField]
    private SpriteRenderer sRenderer;
    #endregion

    private void Awake()
    {
        //if there isnt one set, use the main camera
        goTrackingObject = goTrackingObject ?? Camera.main.gameObject;
        sRenderer = sRenderer ?? GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Setting the initial pos and length based on the renderer
        startpos = transform.position.x;
        length = sRenderer.bounds.size.x;
    }

    // Update is called once per frame
    private void Update()
    {
        //Seeing how far to move based on the effect float
        float temp = (goTrackingObject.transform.position.x * (1 - parallaxEff));
        float dist = (goTrackingObject.transform.position.x * parallaxEff);

        //Moving the object
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        //Updating the variables
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
