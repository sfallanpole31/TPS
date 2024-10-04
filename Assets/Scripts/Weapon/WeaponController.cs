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
    [Header("�Z���Ϥ�")]
    public Sprite weaponIcon;

    [Header("�Z��GameObject")]
    [SerializeField] GameObject weaponRoot;

    [Header("�j�f��m")]
    [SerializeField] Transform weaponMuzzle;

    [Space(5)]
    [Header("�g���Φ�")]
    [SerializeField] WeaponShootType shootType;

    [Header("�l�u")]
    [SerializeField] Projectile projectilePrefab;

    [Header("�⦸�g�����j")]
    [SerializeField] float delayBetweenShoot = 0.5f;

    [Header("�g�@�o�l�u�һݼƶq")]
    [SerializeField] int bulletPerShoot;

    [Space(5)]
    [Header("�C��Reload�u�ļƶq")]
    [SerializeField] float ammoReloadRate = 1f;

    [Header("�g��������i�Hreload���ɶ�")]
    [SerializeField] float ammoReloadDelay = 2f;

    [Header("�̤j�l�u�ƶq")]
    [SerializeField] float maxAmmo = 30f;

    [Space(5)]
    [Header("�j�f�o�g�S��")]
    [SerializeField] GameObject muzzleFlashPrefab;
    [Header("�o�g����")]
    [SerializeField] AudioClip shootSFX;
    [Header("������o�ӪZ��������")]
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
            //��e�l�u�}�lReload
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
