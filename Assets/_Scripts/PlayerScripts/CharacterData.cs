using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public Character data;
    [SerializeField] internal PlayerClass Class;
    [SerializeField] internal float healthPoint;
    [SerializeField] internal float magicPoint;
    [SerializeField] internal float baseDamage;
    [SerializeField] internal float movementSpeed;
    [SerializeField] internal Weapon RightHandEquippedWeapon;
    [SerializeField] internal Weapon LeftHandEquippedWeapon;

    [Header("INGAME OBJ")]
    [SerializeField] private GameObject atkRange;
    private void Awake()
    {
        float range = RightHandEquippedWeapon.AttackRange;
        atkRange.transform.localScale = new Vector3(range, range, range);
    }
    private void Start()
    {
        
    }

    void SetData()
    {
        baseDamage = data.baseDamage;
        movementSpeed = data.movementSpeed;
        healthPoint = data.healthPoint;
        magicPoint = data.magicPoint;
        Class = data.Class;
        RightHandEquippedWeapon = data.RightHandWeapon;
        LeftHandEquippedWeapon = data.LeftHandWeapon;
    }
}
