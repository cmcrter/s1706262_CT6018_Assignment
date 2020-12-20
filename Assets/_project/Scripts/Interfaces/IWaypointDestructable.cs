////////////////////////////////////////////////////////////
// File: IWaypointDestructable.cs
// Author: Charles Carter
// Brief: Anything that needs to have a behaviour when it crosses a waypoint (mainly things accessed by the player)
////////////////////////////////////////////////////////////

public interface IWaypointDestructable
{
    void Destruct();
}
