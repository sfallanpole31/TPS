using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    [Header("相機跟隨目標")]
    [SerializeField] Transform player;

    [Header("相機與跟隨目標距離")]
    [SerializeField] float distanceToTarget;

    [Header("相機高度")]
    [SerializeField] float startHeight;

    [Header("平滑移動時間")]
    [SerializeField] float smoothTime;//平滑移動時間

    [Header("靈敏度Z")]
    [SerializeField] float sensitivityOffset_z;

    [Header("最小垂直Y 偏移量")]
    [SerializeField] float miniOffset_y;

    [Header("最大垂直Y 偏移量")]
    [SerializeField] float maxOffset_y;

    [Header("目前Y高度")]
    [SerializeField] float offset_y ;


    Vector3 smoothPosition = Vector3.zero;

    /// <summary>
    /// 當前速率
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
            transform.position = Vector3.Lerp(transform.position,offsetTarget,smoothTime);//線性移動
        }
        if (CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up * offset_y, ref currentVelocity, smoothTime);
            transform.position = smoothPosition;
        }
    }
    /// <summary>
    /// 檢查與目標的距離
    /// </summary>
    /// <returns></returns>
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }
}
