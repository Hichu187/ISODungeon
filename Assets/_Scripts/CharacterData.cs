using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public Character data;

    [SerializeField] internal int baseDamage;
    [SerializeField] internal int movementSpeed;
    [SerializeField] internal int HP;
    [SerializeField] internal int MP;
    [SerializeField] internal PlayerClass Class;
    [SerializeField] internal Weapon EquippedWeapon;
    private void Awake()
    {
        SetData();
    }
    private void Start()
    {
        
    }

    void SetData()
    {
        baseDamage = data.baseDamage;
        movementSpeed = data.movementSpeed;
        HP = data.HP;
        MP = data.MP;
        Class = data.Class;
        EquippedWeapon = data.EquippedWeapon;
    }
}
