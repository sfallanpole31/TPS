using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("起始武器")]
    [SerializeField] List<WeaponController> startingWeapon = new List<WeaponController>();

    [Header("儲存武器位置")]
    [SerializeField] Transform eqipWeaponParent;

    [Header("瞄準時間")]
    [SerializeField] float aimTime = 2f;



    public event Action<WeaponController, int> onAddWeapon;

    /// <summary>
    /// 目前裝備武器的index
    /// </summary>
    int activeWeaponIndex;

    bool isAim;

    /// <summary>
    /// 武器最多三個
    /// </summary>
    WeaponController[] weapons = new WeaponController[3];
    PlayerController player;
    InputController input;

    private void Start()
    {
        activeWeaponIndex = -1;
        player = GetComponent<PlayerController>();
        input = GameMangerSingleton.Instance.InputController;
        player.OnAim += OnAim;

        foreach (WeaponController weapon in startingWeapon)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(1);
    }

    private void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        if (activeWeapon && isAim)
        {
            activeWeapon.HandlShootInput(input.GetFireInputDown(), input.GetFireInputHold(), input.GetFireInputUp());
        }

        int switchWeaponInput = input.GetSwitchWeaponInput();
        if (switchWeaponInput != 0)
        {
            SwitchWeapon(switchWeaponInput);
        }
    }

    public void SwitchWeapon(int addIndex)
    {
        int newWeaponIndex = -1;
        if (activeWeaponIndex + addIndex > weapons.Length - 1)
        {
            newWeaponIndex = 0;
        }
        else if (activeWeaponIndex + addIndex < 0)
        {
            newWeaponIndex = weapons.Length - 1;
        }
        else
        {
            newWeaponIndex = activeWeaponIndex + addIndex;
        }

        SwitchToWeaponIndex(newWeaponIndex);
    }

    private void SwitchToWeaponIndex(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            if (GetWeaponAtSlotIndex(index) != null)
            {
                if (GetActiveWeapon() != null)
                {
                    GetActiveWeapon().ShowWeapon(false);
                }

                //顯示武器
                activeWeaponIndex = index;
                GetActiveWeapon().ShowWeapon(true);
            }
        }
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        if (index >= 0 && index < weapons.Length - 1 && weapons[index] != null)
        {
            return weapons[index];
        }
        return null;
    }
    public bool AddWeapon(WeaponController weaponPrefab)
    {
        //檢查SLOT內有沒有該武器
        if (HasWeapon(weaponPrefab))
        {
            return false;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                WeaponController weaponInstance = Instantiate(weaponPrefab, eqipWeaponParent); //產生武器至設定位置
                weaponInstance.sourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowWeapon(false);
                weapons[i] = weaponInstance;

                onAddWeapon?.Invoke(weaponInstance, i);
                return true;
            }
        }

        return false;
    }

    private bool HasWeapon(WeaponController weaponPrefab)
    {
        foreach (WeaponController weapon in weapons)
        {
            if (weapon != null && weapon.sourcePrefab == weaponPrefab.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void OnAim(bool value)
    {
        if (value)
        {
            StopAllCoroutines();
            StartCoroutine(DelayAim());
        }
        else
        {
            StopAllCoroutines();
            isAim = value;
        }
    }

    IEnumerator DelayAim()
    {
        yield return new WaitForSecondsRealtime(aimTime);
        isAim = true;
    }
}
