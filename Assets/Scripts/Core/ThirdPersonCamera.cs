using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("�۾����H�ؼ�")]
    [SerializeField] Transform target;

    [Header("�����b�F�ӫ�")]
    [SerializeField] float sensitivity_X = 2;
    [Header("�����b�F�ӫ�")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("�ƹ��F�ӫ�")]
    [SerializeField] float sensitivity_Z = 5;
    [Header("�̤p��������")]
    [SerializeField] float miniVerticalAngle = -10;
    [Header("�̤j��������")]
    [SerializeField] float maxVerticalAngle = 85;
    [Header("�۾��P�ؼЪ��Z��")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("�̤p�۾��P�ؼЪ��Z��")]
    [SerializeField] float miniDistance = 2;
    [Header("�̤j�۾��P�ؼЪ��Z��")]
    [SerializeField] float maxDistance = 25;
    [Header("����")]
    [SerializeField] Vector3 offset;
    [Header("���a�ؼ�")]
    [SerializeField] GameObject player;
    [Header("���˯S��")]
    [SerializeField] ParticleSystem beHitParticle;
    [Header("�[�t�S��")]
    [SerializeField] ParticleSystem spriteParticle;
    [Header("Pause UI")]
    [SerializeField] GameObject pauseUI;

    [Header("�Ȱ�����")]
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
                mouse_Y = Mathf.Clamp(mouse_Y, miniVerticalAngle, maxVerticalAngle);//����Y�b�̤j�̤p�ȥ� Mathf.Clamp
                transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);//��۾�������
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
                mouse_Y = Mathf.Clamp(mouse_Y, miniVerticalAngle, maxVerticalAngle);//����Y�b�̤j�̤p�ȥ� Mathf.Clamp
                transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);//��۾�������
                transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(1.5f, 0, -1.5f) + target.position + Vector3.up * offset.y;

            }
        }
        else
        {

            pauseUI.SetActive(true);
            Time.timeScale = 0;//�Ȱ�
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            audioSource.PlayOneShot(pauseSFX);
        }
    }
}
