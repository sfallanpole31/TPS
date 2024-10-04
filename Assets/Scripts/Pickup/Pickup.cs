using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("上下移動頻率")]
    [SerializeField] float verticalBobFrequency = 1f;

    [Header("物件上下移動的距離")]
    [SerializeField] float bobingAmount = 1f;

    [Header("每秒旋轉的角度")]
    [SerializeField] float rotatingSpeed = 360f;

    public event Action<GameObject> onPick;

    Rigidbody rigidbody;
    Collider collider;
    Vector3 startPostion;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        rigidbody.isKinematic = true;
        collider.isTrigger = true;

        startPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float bobingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobingAmount;
        transform.position = startPostion + Vector3.up * bobingAnimationPhase;
        transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            onPick?.Invoke(other.gameObject);
        }
    }
}
