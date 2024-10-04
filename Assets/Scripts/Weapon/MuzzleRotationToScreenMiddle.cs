using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleRotationToScreenMiddle : MonoBehaviour
{
    [Header("�g�u�̤j�Z��")]
    public float maxDistance;

    public Ray ray;
    RaycastHit hit;

    [Header("�Ǥ��B������X")]
    public float offset_x = 0f;
    [Header("�Ǥ��B������Y")]
    public float offset_y = 0f;


    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2+ offset_x, Screen.height / 2 + offset_y, 0));
        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.blue);
    }
}
