using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _WeaponSystem : MonoBehaviour
{
    CharacterData pData;
    Weapon rHandWeapon;
    Weapon lHandWeapon;

    public List<GameObject> swordModels;
    public List<GameObject> axeModels;
    public List<GameObject> knifeModels;
    public List<GameObject> staffModels;
    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterData>();
        rHandWeapon = pData.RightHandEquippedWeapon;
        lHandWeapon = pData.LeftHandEquippedWeapon;
        LoadModelWeapon();
    }

    void LoadModelWeapon()
    {
        if(pData.RightHandEquippedWeapon != null)
        {
            switch (pData.RightHandEquippedWeapon.weapon)
            {
                case WeaponType.Claymores:
                case WeaponType.Swords:
                    for(int i=0; i<swordModels.Count; i++)
                    {
                        if (swordModels[i].name == pData.RightHandEquippedWeapon.Name) swordModels[i].gameObject.SetActive(true);
                        else swordModels[i].gameObject.SetActive(false);
                    }
                    break;

            }
        }
    }
}
