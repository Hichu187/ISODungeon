using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon Info")]
public class Weapon : ScriptableObject
{
    [SerializeField] private GameObject model;
    public string Name;
    public int Damage;
    public int AttackRange;
    public bool IsOneHanded;
    public string NormalAttackName;
    public string Skill1Name;
    public string Skill2Name;
    public string Skill3Name;

    private void OnValidate()
    {
        if(model!= null)
            Name = model.name;
    }
}
