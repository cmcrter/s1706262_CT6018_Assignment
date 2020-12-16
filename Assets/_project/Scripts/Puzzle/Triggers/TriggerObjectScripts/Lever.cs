using UnityEngine;

public class Lever : InputTrigger
{
    [SerializeField]
    GameObject handle;

    [SerializeField]
    Transform pivot;

    private void Awake()
    {
        handle = handle ?? transform.GetChild(0).gameObject;
    }

    public override void InputTriggered()
    {
        base.InputTriggered();

        if (handle)
        {
            //It was just activated
            if (isActivated)
            {
                pivot.transform.RotateAround(transform.position, Vector3.forward, -10);
            }
            //It was just unactivated
            else
            {
                pivot.transform.RotateAround(transform.position, Vector3.forward, 10);
            }
        }
    }
}
