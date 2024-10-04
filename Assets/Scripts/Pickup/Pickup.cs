using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("�W�U�����W�v")]
    [SerializeField] float verticalBobFrequency = 1f;

    [Header("����W�U���ʪ��Z��")]
    [SerializeField] float bobingAmount = 1f;

    [Header("�C����઺����")]
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
