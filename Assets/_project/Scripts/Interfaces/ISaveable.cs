////////////////////////////////////////////////////////////
// File: IMirrorable.cs
// Author: Charles Carter
// Brief: An interface for everything that wants to save or load something from playerprefs
////////////////////////////////////////////////////////////

public interface ISaveable
{
    void Save();
    void Load();
}
