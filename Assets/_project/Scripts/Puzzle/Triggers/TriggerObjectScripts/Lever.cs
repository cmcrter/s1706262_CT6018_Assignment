using UnityEngine;

public class Lever : InputTrigger
{
    [SerializeField]
    GameObject handle;

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
                handle.transform.Rotate(new Vector3(0, 0, 90), Space.World);
            }
            //It was just unactivated
            else
            {
                handle.transform.Rotate(new Vector3(0, 0, -90), Space.World);
            }
        }
    }
}
