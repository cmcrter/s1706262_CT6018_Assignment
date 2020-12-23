////////////////////////////////////////////////////////////
// File: BackgroundManager.cs
// Author: Charles Carter
// Brief: A class to handle the switching and management of backgrounds in the game
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Letting the manager know to switch the active backgrounds
    public void SwitchActiveBackground(bool forward)
    {
        if (forward)
        {
            Parallax[] parallaxes;
            if (backgroundParallaxes.TryGetValue(iCurrentBackgroundActive, out parallaxes))
            {
                SetComponentArrayActive(parallaxes, false);
            }

            if (iCurrentBackgroundActive != backgrounds.Count - 1)
            {
                iCurrentBackgroundActive++;

                if (backgroundParallaxes.TryGetValue(iCurrentBackgroundActive, out parallaxes))
                {
                    SetComponentArrayActive(parallaxes, true);
                    SetParallaxOffset(parallaxes);
                }
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
            Parallax[] parallaxes;
            if (backgroundParallaxes.TryGetValue(iCurrentBackgroundActive, out parallaxes))
            {
                SetComponentArrayActive(parallaxes, false);
            }

            if (iCurrentBackgroundActive != 0)
            {
                iCurrentBackgroundActive--;

                if (backgroundParallaxes.TryGetValue(iCurrentBackgroundActive, out parallaxes))
                {
                    SetComponentArrayActive(parallaxes, true);
                }
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

    private void SetParallaxOffset(Parallax[] parallaxesToChange)
    {
        for (int i = 0; i < parallaxesToChange.Length; ++i)
        {
            parallaxesToChange[i].SetOffset();
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
