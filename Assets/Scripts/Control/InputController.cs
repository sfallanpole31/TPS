using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float vertical;
    public float horizontal;



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //��w����
        Cursor.visible = false;//���ù���

    }
    private void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        CheckCursorState();
    }
    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = Vector3.ClampMagnitude(move, 1);//����V�q���פ��W�L�]�w�ȡA����t�� ���n�����a���ʳt�׹L��
        return move;
    }
    private void CheckCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
    /// <summary>
    /// ���oMouseX��Axis
    /// </summary>
    /// <returns></returns>
    public float GetMouseXAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }
    /// <summary>
    /// ���oMouseY��Axis
    /// </summary>
    /// <returns></returns>
    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }
    /// <summary>
    /// ���oMouseX��Axis
    /// </summary>
    /// <returns></returns>
    public float GetMouseScrollWheelAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }
    /// <summary>
    /// �O�_�i�H�B�zInput
    /// </summary>
    public bool CanProcessInput()
    {
        // �p�GCursor���A���b��w���N����B�zInput
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public bool GetSprintInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }

    /// <summary>
    /// �O�_���ť���
    /// </summary>
    /// <returns></returns>
    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.Space);

        }
        return false;
    }

    /// <summary>
    /// �O�_���U�ƹ��k��
    /// </summary>
    /// <returns></returns>
    public bool GetAimInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }

    /// <summary>
    /// �O�_���U�ƹ�����
    /// </summary>
    /// <returns></returns>
    public bool GetFireInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(0);
        }
        return false;
    }

    /// <summary>
    /// �O�_������U�ƹ�����
    /// </summary>
    /// <returns></returns>
    public bool GetFireInputHold()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }
        return false;
    }

    /// <summary>
    /// �O�_��}�ƹ�����
    /// </summary>
    /// <returns></returns>
    public bool GetFireInputUp()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    /// <summary>
    /// ���o�O�_���UR
    /// </summary>
    public bool GetReloadInputDown()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        return false;
    }

    /// <summary>
    /// ���o�O�_���U�����Z��
    /// </summary>
    /// <returns></returns>
    public int GetSwitchWeaponInput()
    {
        if(CanProcessInput())
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                return -1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                return 1;
            }
        }
        return 0;
    }

}
