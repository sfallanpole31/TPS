using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    [Header("�۾����H�ؼ�")]
    [SerializeField] Transform player;

    [Header("�۾��P���H�ؼжZ��")]
    [SerializeField] float distanceToTarget;

    [Header("�۾�����")]
    [SerializeField] float startHeight;

    [Header("���Ʋ��ʮɶ�")]
    [SerializeField] float smoothTime;//���Ʋ��ʮɶ�

    [Header("�F�ӫ�Z")]
    [SerializeField] float sensitivityOffset_z;

    [Header("�̤p����Y �����q")]
    [SerializeField] float miniOffset_y;

    [Header("�̤j����Y �����q")]
    [SerializeField] float maxOffset_y;

    [Header("�ثeY����")]
    [SerializeField] float offset_y ;


    Vector3 smoothPosition = Vector3.zero;

    /// <summary>
    /// ��e�t�v
    /// </summary>
    Vector3 currentVelocity;

    InputController input;

    private void Awake()
    {
        input = GameMangerSingleton.Instance.InputController;
        transform.position = player.position + Vector3.up * startHeight;

    }

    private void LateUpdate()
    {
        if (input.GetMouseScrollWheelAxis() != 0)
        {
            offset_y += input.GetMouseScrollWheelAxis() * sensitivityOffset_z;
            offset_y = Mathf.Clamp(offset_y, miniOffset_y, maxOffset_y);
            Vector3 offsetTarget = player.position + player.up * offset_y;
            transform.position = Vector3.Lerp(transform.position,offsetTarget,smoothTime);//�u�ʲ���
        }
        if (CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up * offset_y, ref currentVelocity, smoothTime);
            transform.position = smoothPosition;
        }
    }
    /// <summary>
    /// �ˬd�P�ؼЪ��Z��
    /// </summary>
    /// <returns></returns>
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }
}
