using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class Character : ScriptableObject
{
    public PlayerClass Class;
    public float baseDamage;
    public float movementSpeed;
    public float healthPoint;
    public float magicPoint;    
    public Weapon RightHandWeapon;
    public Weapon LeftHandWeapon;
}

