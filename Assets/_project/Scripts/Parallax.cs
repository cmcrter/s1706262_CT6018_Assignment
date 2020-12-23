////////////////////////////////////////////////////////////
// File: Parallax.cs
// Author: Dani (Edited by Charles Carter)
// Brief: To move an object based on another movements object
//////////////////////////////////////////////////////////// 

using UnityEngine;

//Script in initial state found https://www.youtube.com/watch?v=zit45k6CUMk
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
    //The first sprite of the background
    [SerializeField]
    private GameObject goStartSprite;
    private float ParentOffset;

    //The offset from the beginning parallax
    private float initialOffset = 0;

    #endregion

    private void Awake()
    {
        //if there isnt one set, use the main camera
        goTrackingObject = goTrackingObject ?? Camera.main.gameObject;
        sRenderer = sRenderer ?? GetComponentInChildren<SpriteRenderer>();
        goStartSprite = goStartSprite ?? transform.GetChild(0).gameObject;
        ParentOffset = transform.parent.transform.position.x;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Setting the initial pos and length based on the renderer
        startpos = transform.position.x + goTrackingObject.transform.position.x + ParentOffset;
        length = sRenderer.bounds.size.x;
    }

    public void SetOffset()
    {
        //If it has a parallax eff currently and there's no intial offset already set
        if (parallaxEff > 0 && initialOffset == 0)
        {
            //the minimum x half + a magic number
            initialOffset = (sRenderer.bounds.min.x * 0.5f) + 6f;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Seeing how far to move based on the effect float
        float temp = (goTrackingObject.transform.position.x * (1 - parallaxEff));
        float dist = (goTrackingObject.transform.position.x * parallaxEff);

        //Moving the object
        transform.position = new Vector3((dist + ParentOffset) - initialOffset, transform.position.y, transform.position.z);

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

    //The manager might need the offset or to set it
    public float GetOffset()
    {
        return startpos;
    }

    public void SetOffset(float NewOffset)
    {
        startpos = NewOffset;
    }
}
