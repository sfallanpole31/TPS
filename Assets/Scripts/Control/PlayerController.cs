using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̰����ʳt��")]
    [SerializeField] float MoveSpeed = 8;

    [Header("Shift�[�t��")]
    [Range(1, 3)]
    [SerializeField] float sprintSpeedModifier = 2;

    [Header("�ۤU���ʳt��")]
    [SerializeField] float crouchSpeedModifier = 0.5f;

    [Header("����t��")]
    [SerializeField] float rotateSpeed = 5f;



    [Header("�U�@�V���ʪ��ؼЦ�m")]
    Vector3 targetMovemenet;

    [Header("�U�@�V���D��m")]
    Vector3 jumpDirection;

    [Header("lastFrameSpeed")]
    float lastFrameSpeed;


    [Header("���D�ɦV�W���O")]
    [SerializeField] float jumpForce = 15;

    [Header("�b�Ť��U�I�[���O")]
    [SerializeField] float gravityDownForce = 50;

    [Header("�P�a�����Z��")]
    [SerializeField] float distanceToGround = 50;

    [SerializeField] bool isGround;

    [Header("�[�t�צʤ���")]
    [SerializeField] float addSpeedRatio = 0.1f;

    [Header("�ǷsIcon")]
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
    /// �B�z�˷ǰʧ@
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
    /// ���D
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
    /// �˴��O�_�b�a��
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        isGround = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
    }

    /// <summary>
    /// ����
    /// </summary>
    private void MoveBehavior()
    {
        targetMovemenet = Vector3.zero;
        targetMovemenet += inputController.GetMoveInput().z * GetCurrentCameraForward();
        targetMovemenet += inputController.GetMoveInput().x * GetCurrentCameraRight();

        //�קK�﨤�u�W�L1
        targetMovemenet = Vector3.ClampMagnitude(targetMovemenet, 1);

        //�U�@�V���ʳt��
        float nextFrameSpeed = 0;

        //�O�_���USHIFT�[�t
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
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);//�u�ʼW�[�t��


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
    /// ���Ʊ������
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
