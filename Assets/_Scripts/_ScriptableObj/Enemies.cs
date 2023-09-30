using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy Data")]
public class Enemies : ScriptableObject
{
    public EnemyType type;
    public float maxHealthPoint;
    public float damage;
    public float movementSpeed;
    public float senseRange;
    public float atkRange;
    public float cooldownAtk;
}
