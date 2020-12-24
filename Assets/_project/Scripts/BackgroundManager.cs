////////////////////////////////////////////////////////////
// File: BackgroundManager.cs
// Author: Charles Carter
// Brief: A class to handle the switching and management of backgrounds in the game
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

//Can save which background got reached
public class BackgroundManager : MonoBehaviour
{
    //Backgrounds in order of left to right
    [SerializeField]
    private List<GameObject> backgrounds = new List<GameObject>();
    private int iCurrentBackgroundActive = 0;

    //Linking an int to an array of parallax effects (possibly could be static?)
    Dictionary<int, Parallax[]> backgroundParallaxes = new Dictionary<int, Parallax[]>();

    private void Awake()
    {
        //Going through the backgrounds
        for (int i = 0; i < backgrounds.Count; ++i)
        {
            //Adding the parallax effects on that background
            backgroundParallaxes.Add(i, parallaxEffects(backgrounds[i]));
        }
    }

    public void TurnAllBackgroundsOff()
    {
        //Going through the backgrounds
        for (int i = 0; i < backgrounds.Count; ++i)
        {
            SetBackgroundActive(i, false);
        }
    }

    //Letting the manager know to switch the active backgrounds
    public void SwitchActiveBackground(bool forward)
    {
        if (forward)
        {
            SetBackgroundActive(iCurrentBackgroundActive, false);

            if (iCurrentBackgroundActive != backgrounds.Count - 1)
            {
                iCurrentBackgroundActive++;
                SetBackgroundActive(iCurrentBackgroundActive, true);
            }
            else
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("End of the backgrounds");
                }
            }
        }
        else
        {
            SetBackgroundActive(iCurrentBackgroundActive, false);

            if (iCurrentBackgroundActive != 0)
            {
                iCurrentBackgroundActive--;
                SetBackgroundActive(iCurrentBackgroundActive, true);
            }
            else
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Start of the backgrounds");
                }
            }
        }
    }

    //A function to set a background active or unactive based on an index
    public void SetBackgroundActive(int iBackgroundIndex, bool newActive)
    {
        //Making sure the index is within bounds
        if (iBackgroundIndex < backgrounds.Count && iBackgroundIndex > -1)
        {
            Parallax[] parallaxes;

            if (backgroundParallaxes.TryGetValue(iBackgroundIndex, out parallaxes))
            {
                if (newActive)
                {
                    SetParallaxOffset(parallaxes);
                }

                SetComponentArrayActive(parallaxes, newActive);
            }
        }
    }

    public void SetParallaxOffset(Parallax[] parallaxesToChange)
    {
        for (int i = 0; i < parallaxesToChange.Length; ++i)
        {
            parallaxesToChange[i].SetOffset();
        }
    }

    public void SetParallaxOffset(int parallaxesToChange)
    {
        Parallax[] parallaxes;

        if (backgroundParallaxes.TryGetValue(parallaxesToChange, out parallaxes))
        {
            for (int i = 0; i < parallaxes.Length; ++i)
            {
                parallaxes[i].SetOffset();
            }
        }
    }

    private Parallax[] parallaxEffects(GameObject fromGameobject)
    {
        Parallax[] effects = fromGameobject.GetComponentsInChildren<Parallax>();

        return effects;
    }

    private bool SetComponentArrayActive(MonoBehaviour[] behaviours, bool newActive)
    {
        for (int i = 0; i < behaviours.Length; ++i)
        {
            if (behaviours[i])
            {
                behaviours[i].enabled = newActive;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
