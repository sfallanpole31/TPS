using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleRotationToScreenMiddle : MonoBehaviour
{
    [Header("射線最大距離")]
    public float maxDistance;

    public Ray ray;
    RaycastHit hit;

    [Header("準心矯正偏移X")]
    public float offset_x = 0f;
    [Header("準心矯正偏移Y")]
    public float offset_y = 0f;


    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2+ offset_x, Screen.height / 2 + offset_y, 0));
        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.blue);
    }
}
