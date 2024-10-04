using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("最高移動速度")]
    [SerializeField] float MoveSpeed = 8;

    [Header("Shift加速度")]
    [Range(1, 3)]
    [SerializeField] float sprintSpeedModifier = 2;

    [Header("蹲下移動速度")]
    [SerializeField] float crouchSpeedModifier = 0.5f;

    [Header("旋轉速度")]
    [SerializeField] float rotateSpeed = 5f;



    [Header("下一幀移動的目標位置")]
    Vector3 targetMovemenet;

    [Header("下一幀跳躍位置")]
    Vector3 jumpDirection;

    [Header("lastFrameSpeed")]
    float lastFrameSpeed;


    [Header("跳躍時向上的力")]
    [SerializeField] float jumpForce = 15;

    [Header("在空中下施加的力")]
    [SerializeField] float gravityDownForce = 50;

    [Header("與地面的距離")]
    [SerializeField] float distanceToGround = 50;

    [SerializeField] bool isGround;

    [Header("加速度百分比")]
    [SerializeField] float addSpeedRatio = 0.1f;

    [Header("準新Icon")]
    [SerializeField] GameObject crossHair;

    bool isAim;

    Animator animator;
    Health health;
    InputController inputController;
    CharacterController characterController;

    public event Action<bool> OnAim;
    public event Action onSprint;

    private void Awake()
    {
        inputController = GameMangerSingleton.Instance.InputController;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.onDie += OnDie;
    }


    void Update()
    {
        AimBehaviour();
        MoveBehavior();
        JumpBehavior();

    }


    /// <summary>
    /// 處理瞄準動作
    /// </summary>
    private void AimBehaviour()
    {
        bool lastTimeAim = isAim;
        if (inputController.GetFireInputHold())
        {
            isAim = true;

        }
        if (inputController.GetAimInputDown())
        {
            isAim = !isAim;
        }

        if (lastTimeAim != isAim)
        {
            if (crossHair!=null)
                crossHair.SetActive(isAim);
            OnAim?.Invoke(isAim);
        }


        animator.SetBool("IsAim", isAim);

    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void JumpBehavior()
    {

        if (inputController.GetJumpInputDown() && IsGrounded() && !animator.GetBool("IsJump"))
        {
            //Debug.Log(transform.position.ToString() + "+ -" + Vector3.up.ToString()+"+"+ distanceToGround.ToString());
            animator.SetTrigger("IsJump");
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;
        }
        jumpDirection.y -= gravityDownForce * Time.deltaTime;
        jumpDirection.y = Mathf.Max(jumpDirection.y, -gravityDownForce);
        characterController.Move(jumpDirection * Time.deltaTime);
    }

    /// <summary>
    /// 檢測是否在地面
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        isGround = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void MoveBehavior()
    {
        targetMovemenet = Vector3.zero;
        targetMovemenet += inputController.GetMoveInput().z * GetCurrentCameraForward();
        targetMovemenet += inputController.GetMoveInput().x * GetCurrentCameraRight();

        //避免對角線超過1
        targetMovemenet = Vector3.ClampMagnitude(targetMovemenet, 1);

        //下一幀移動速度
        float nextFrameSpeed = 0;

        //是否按下SHIFT加速
        if (targetMovemenet == Vector3.zero)
        {
            nextFrameSpeed = 0;
        }
        else if (inputController.GetSprintInput() && !isAim)
        {
            nextFrameSpeed = 1f;
            targetMovemenet *= sprintSpeedModifier;
            SmoothRotation(targetMovemenet);
            onSprint?.Invoke();
        }
        else if (!isAim)
        {
            nextFrameSpeed = 0.5f;
            SmoothRotation(targetMovemenet);
        }

        if (isAim)
        {
            SmoothRotation(GetCurrentCameraForward());
        }

        if (lastFrameSpeed != nextFrameSpeed)
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);//線性增加速度


        animator.SetFloat("WalkSpeed", lastFrameSpeed);
        animator.SetFloat("Vertical", inputController.GetMoveInput().z);
        animator.SetFloat("Horizontal", inputController.GetMoveInput().x);

        characterController.Move(targetMovemenet * Time.deltaTime * MoveSpeed);
        //if (inputController.GetSprintInput())
        //{
        //    targetMovemenet *= sprintSpeedModifier;
        //}

        //if (targetMovemenet != Vector3.zero)
        //{ SmoothRotation(targetMovemenet); }

    }

    /// <summary>
    /// 平滑旋轉視角
    /// </summary>
    /// <param name="targetMovemenet"></param>
    private void SmoothRotation(Vector3 targetMovemenet)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMovemenet, Vector3.up), rotateSpeed * Time.deltaTime);
    }

    private Vector3 GetCurrentCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        return cameraRight;
    }

    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        return cameraForward;
    }

    private void OnDie()
    {
        animator.SetTrigger("IsDead");
    }


}
