using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Image[] pockets;
    [SerializeField] Image[] weaponIcon;
    [SerializeField] Image[] energy;

    WeaponManager weaponManager;
    // Start is called before the first frame update
    void Awake()
    {
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        weaponManager.onAddWeapon += OnAddWeapon;
    }


    private void OnAddWeapon(WeaponController weapon, int index)
    {
        weaponIcon[index].enabled = true;
        weaponIcon[index].sprite = weapon.weaponIcon;
    }

    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (weaponManager.GetWeaponAtSlotIndex(i) == null)
            {
                pockets[i].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                pockets[i].color = Color.gray;
                weaponIcon[i].color = Color.gray;
                energy[i].color = Color.gray;
                continue;
            }
         
            float value = weaponManager.GetWeaponAtSlotIndex(i).currentAmmoRatio;
            energy[i].fillAmount = Mathf.Lerp(energy[i].fillAmount, value, 0.05f);
            if (weaponManager.GetWeaponAtSlotIndex(i) == weaponManager.GetActiveWeapon())
            {
                pockets[i].transform.localScale = new Vector3(1f, 1f, 1f);
                pockets[i].color = Color.white;
                weaponIcon[i].color = Color.white;
                energy[i].color = Color.white;
            }
            else
            {
                pockets[i].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                pockets[i].color = Color.gray;
                weaponIcon[i].color = Color.gray;
                energy[i].color = Color.gray;
            }
        }
    }
}
