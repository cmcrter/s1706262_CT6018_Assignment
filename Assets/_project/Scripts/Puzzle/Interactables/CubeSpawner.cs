////////////////////////////////////////////////////////////
// File: CubeSpawner.cs
// Author: Charles Carter
// Brief: A factory pattern to spawn rigidbody cubes
////////////////////////////////////////////////////////////

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

    #region Class Variables

    [Header("Variables Needed")]
    [SerializeField]
    private GameObject CubePrefab;
    [SerializeField]
    private CState state;
    //The spawner could have a cube already spawned in
    [SerializeField]
    private GameObject currentCube;
    [SerializeField]
    private Transform cubeParent;
    private bool bSpawnerLocked;
    //I want the spawner renderer itself to look different than it's walls etc
    [SerializeField]
    private Material[] stateMaterials = new Material[4];
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private int iMaxCubes;
    private int iCurrentCubes;

    #endregion

    private void Awake()
    {
        state = state ?? GetComponent<CState>();
        _renderer = _renderer ?? GetComponent<Renderer>();
    }

    private void Start()
    {
        //Making sure it's the correct material for the spawner
        _renderer.material = stateMaterials[state.returnID()];

        //Making sure there's a max cube count
        if (iMaxCubes == 0)
        {
            iMaxCubes = 1;
        }

        //Cube spawners can only start with 1 current cube within the world
        if (currentCube)
        {
            iCurrentCubes = 1;
        }
    }

    //Interface Functions
    private void SpawnerTriggered()
    {
        //Limiting it to one cube at a time
        if (currentCube && iCurrentCubes == iMaxCubes)
        {
            currentCube.transform.position = transform.position;
        }
        else if (iCurrentCubes < iMaxCubes)
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

            iCurrentCubes++;
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
