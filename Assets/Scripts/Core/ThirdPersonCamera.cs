using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("相機跟隨目標")]
    [SerializeField] Transform target;

    [Header("水平軸靈敏度")]
    [SerializeField] float sensitivity_X = 2;
    [Header("垂直軸靈敏度")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("滑鼠靈敏度")]
    [SerializeField] float sensitivity_Z = 5;
    [Header("最小垂直角度")]
    [SerializeField] float miniVerticalAngle = -10;
    [Header("最大垂直角度")]
    [SerializeField] float maxVerticalAngle = 85;
    [Header("相機與目標的距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("最小相機與目標的距離")]
    [SerializeField] float miniDistance = 2;
    [Header("最大相機與目標的距離")]
    [SerializeField] float maxDistance = 25;
    [Header("偏移")]
    [SerializeField] Vector3 offset;
    [Header("玩家目標")]
    [SerializeField] GameObject player;
    [Header("受傷特效")]
    [SerializeField] ParticleSystem beHitParticle;
    [Header("加速特效")]
    [SerializeField] ParticleSystem spriteParticle;
    [Header("Pause UI")]
    [SerializeField] GameObject pauseUI;

    [Header("暫停音效")]
    [SerializeField] AudioClip pauseSFX;
    AudioSource audioSource;

    InputController inputController;


    float mouse_X = 0;
    float mouse_Y = 30;
    bool IsAim;

    private void Awake()
    {
        inputController = GameMangerSingleton.Instance.InputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onSprint += OnSprint;
        player.GetComponent<PlayerController>().OnAim += OnAim;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnAim(bool value)
    {
        if (value)
        {
            IsAim = true;
        }
        else
        {
            IsAim = false;
        }
    }

    private void OnSprint()
    {
        if (spriteParticle == null)
            return;
        spriteParticle.Play();
    }

    private void OnDamage()
    {
        if (beHitParticle == null)
            return;

        beHitParticle.Play();
    }

    private void LateUpdate()
    {

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (!IsAim)
            {
                pauseUI.SetActive(false);
                Time.timeScale = 1;
                mouse_X += inputController.GetMouseXAxis() * sensitivity_X;
                mouse_Y += inputController.GetMouseYAxis() * sensitivity_Y;
                mouse_Y = Mathf.Clamp(mouse_Y, miniVerticalAngle, maxVerticalAngle);//限制Y軸最大最小值用 Mathf.Clamp
                transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);//對相機做旋轉
                transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(0, 0, -cameraToTargetDistance) + target.position + Vector3.up * offset.y;
                cameraToTargetDistance += inputController.GetMouseScrollWheelAxis() * sensitivity_Z;
                cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, miniDistance, maxDistance);
            }
            else
            {
                pauseUI.SetActive(false);
                Time.timeScale = 1;
                mouse_X += inputController.GetMouseXAxis() * sensitivity_X;
                mouse_Y += inputController.GetMouseYAxis() * sensitivity_Y;
                mouse_Y = Mathf.Clamp(mouse_Y, miniVerticalAngle, maxVerticalAngle);//限制Y軸最大最小值用 Mathf.Clamp
                transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);//對相機做旋轉
                transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(1.5f, 0, -1.5f) + target.position + Vector3.up * offset.y;

            }
        }
        else
        {

            pauseUI.SetActive(true);
            Time.timeScale = 0;//暫停
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            audioSource.PlayOneShot(pauseSFX);
        }
    }
}
