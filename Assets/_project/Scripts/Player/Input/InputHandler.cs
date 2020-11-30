using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    public virtual bool MoveLeft() { return false; }
    public virtual bool MoveRight() { return false; }
    public virtual bool Jump() { return false; }
    public virtual bool Crouch() { return false; }
    public virtual bool FireWeapon() { return false; }
    public virtual bool ThrowWeapon() { return false; }
    public virtual bool Interact() { return false; }
    public virtual bool Block() { return false; }

    //For controllers specifically
    public virtual bool AimLeft() { return false; }
    public virtual bool AimRight() { return false; }

    public virtual bool AimUp() { return false; }
    public virtual bool AimDown() { return false; }
}
