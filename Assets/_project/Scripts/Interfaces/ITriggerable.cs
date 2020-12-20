////////////////////////////////////////////////////////////
// File: ITriggerable.cs
// Author: Charles Carter
// Brief: This is anything that can be triggered like doors etc
////////////////////////////////////////////////////////////

public interface ITriggerable
{
    void Triggered();
    void UnTriggered();

    void Locked();
    void Unlocked();

    bool GetLockState();
}