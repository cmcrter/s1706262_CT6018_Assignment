public interface ITriggerable
{
    void Triggered();
    void UnTriggered();

    void Locked();
    void Unlocked();

    bool GetLockState();
}