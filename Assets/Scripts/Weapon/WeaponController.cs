using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponShootType
{
    Single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{
    [Header("武器圖片")]
    public Sprite weaponIcon;

    [Header("武器GameObject")]
    [SerializeField] GameObject weaponRoot;

    [Header("槍口位置")]
    [SerializeField] Transform weaponMuzzle;

    [Space(5)]
    [Header("射擊形式")]
    [SerializeField] WeaponShootType shootType;

    [Header("子彈")]
    [SerializeField] Projectile projectilePrefab;

    [Header("兩次射擊間隔")]
    [SerializeField] float delayBetweenShoot = 0.5f;

    [Header("射一發子彈所需數量")]
    [SerializeField] int bulletPerShoot;

    [Space(5)]
    [Header("每秒Reload彈藥數量")]
    [SerializeField] float ammoReloadRate = 1f;

    [Header("射擊完畢到可以reload的時間")]
    [SerializeField] float ammoReloadDelay = 2f;

    [Header("最大子彈數量")]
    [SerializeField] float maxAmmo = 30f;

    [Space(5)]
    [Header("槍口發射特效")]
    [SerializeField] GameObject muzzleFlashPrefab;
    [Header("發射音效")]
    [SerializeField] AudioClip shootSFX;
    [Header("切換到這個武器的音效")]
    [SerializeField] AudioClip changeWeaponSFX;

    float currentAmmo;
    float timeSinceLastShoot;
    bool isAim;
    public GameObject sourcePrefab { get; set; }
    public float currentAmmoRatio { get; private set; }
    public bool isCooling { get; private set; }

    AudioSource audioSource;

    private void Awake()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {
        if (timeSinceLastShoot + ammoReloadDelay < Time.time && currentAmmo < maxAmmo)
        {
            //當前子彈開始Reload
            currentAmmo += ammoReloadRate * Time.deltaTime;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
            isCooling = true;
        }
        else
        {
            isCooling = false;
        }

        if(maxAmmo == Mathf.Infinity)
        {
            currentAmmoRatio = 1f;
        }
        else
        {
            currentAmmoRatio = currentAmmo / maxAmmo;
        }
    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);
        if (value && changeWeaponSFX != null)
        {
            audioSource.PlayOneShot(changeWeaponSFX);
        }
    }
    public void HandlShootInput(bool inputDown, bool inputHold, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Single:
                if (inputDown)
                {
                    TryShoot();

                }
                return;
            case WeaponShootType.Automatic:
                if (inputHold)
                {
                    TryShoot();

                }
                return;
            default:
                return;
        }
    }

    private void TryShoot()
    {
        if (currentAmmo >= 1f && timeSinceLastShoot + delayBetweenShoot < Time.time)
        {
            HandleShoot();
            currentAmmo -= 1;
        }
    }

    private void HandleShoot()
    {
        for (int i = 0; i < bulletPerShoot; i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            newProjectile.Shoot(GameObject.FindGameObjectWithTag("Player"));
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject newMuzzlePrefab = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
            Destroy(newMuzzlePrefab, 2f);
        }
        if(shootSFX!=null)
        {
            audioSource.PlayOneShot(shootSFX);
        }

        timeSinceLastShoot = Time.time;

    }
}
