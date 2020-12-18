using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour, ITriggerable
{
    #region Interface Contracts

    void ITriggerable.Triggered() => SpawnerTriggered();
    void ITriggerable.UnTriggered() => SpawnerUnTriggered();
    void ITriggerable.Locked() => SpawnerLocked();
    void ITriggerable.Unlocked() => SpawnerUnLocked();
    bool ITriggerable.GetLockState() => SpawnerLockState();

    #endregion

    [Header("Variables Needed")]
    [SerializeField]
    GameObject CubePrefab;
    [SerializeField]
    CState state;
    //The spawner could have a cube already spawned in
    [SerializeField]
    GameObject currentCube;
    [SerializeField]
    Transform cubeParent;

    private bool bSpawnerLocked;

    //I want the spawner renderer itself to look different than it's walls etc
    [SerializeField]
    Material[] stateMaterials = new Material[4];
    [SerializeField]
    Renderer _renderer;

    private void Awake()
    {
        state = state ?? GetComponent<CState>();
        _renderer = _renderer ?? GetComponent<Renderer>();
    }

    private void Start()
    {
        //Making sure it's the correct material for the spawner
        _renderer.material = stateMaterials[state.returnID()];
    }

    //Interface Functions
    private void SpawnerTriggered()
    {
        //Limiting it to one cube at a time
        if (currentCube)
        {
            currentCube.transform.position = transform.position;
        }
        else
        {
            //Spawning the cube
            if (cubeParent)
            {
                currentCube = Instantiate(CubePrefab, transform.position, Quaternion.identity, cubeParent);
            }
            else
            {              
                currentCube = Instantiate(CubePrefab, transform.position, Quaternion.identity);
            }
        }

        //Making sure it has the same state as the spawner
        CState cubeState = currentCube.GetComponent<CState>();
        cubeState.SetManager(state.GetManager());
        cubeState.SetState(state.returnID(), true);
    }

    private void SpawnerUnTriggered()
    {
        //It cant be "UnTriggered" really
    }

    private void SpawnerLocked()
    {
        bSpawnerLocked = true;
    }

    private void SpawnerUnLocked()
    {
        bSpawnerLocked = false;
    }

    private bool SpawnerLockState()
    {
        return bSpawnerLocked;
    }
}
