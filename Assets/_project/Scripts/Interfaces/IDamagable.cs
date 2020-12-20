////////////////////////////////////////////////////////////
// File: IDamagable.cs
// Author: Charles Carter
// Brief: Interface for anything that can be damaged
////////////////////////////////////////////////////////////

public interface IDamagable
{
    void Damage(float amount);
    void Heal(float amount);
}
