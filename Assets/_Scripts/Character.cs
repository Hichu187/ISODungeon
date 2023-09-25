using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class Character : ScriptableObject
{
    public PlayerClass Class;
    public int baseDamage;
    public int movementSpeed;
    public int healthPoint;
    public int magicPoint;    
    public Weapon RightHandWeapon;
    public Weapon LeftHandWeapon;
}

