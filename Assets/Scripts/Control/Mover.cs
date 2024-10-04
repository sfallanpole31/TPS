using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [Header("最大移動速度")]
    [SerializeField] float maxSpeed = 6f;
    [SerializeField] float animatorChangeRatio = 0.1f;
    NavMeshAgent navMeshAgent;
    float nextSpeed;
    float lastFrameSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity); //將全局NavMesh速度變量 轉換成local速度變量
        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animatorChangeRatio);



        this.GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed / maxSpeed);
    }

    public void MoveTo(Vector3 desination, float speedRatio)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = desination;

    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }
}
