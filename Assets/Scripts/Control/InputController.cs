using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float vertical;
    public float horizontal;



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //鎖定鼠標
        Cursor.visible = false;//隱藏鼠標

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
        move = Vector3.ClampMagnitude(move, 1);//限制向量長度不超過設定值，限制速度 不要讓玩家移動速度過快
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
    /// 取得MouseX的Axis
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
    /// 取得MouseY的Axis
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
    /// 取得MouseX的Axis
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
    /// 是否可以處理Input
    /// </summary>
    public bool CanProcessInput()
    {
        // 如果Cursor狀態不在鎖定中就不能處理Input
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
    /// 是否按空白鍵
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
    /// 是否按下滑鼠右鍵
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
    /// 是否按下滑鼠左鍵
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
    /// 是否持續按下滑鼠左鍵
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
    /// 是否放開滑鼠左鍵
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
    /// 取得是否按下R
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
    /// 取得是否按下切換武器
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
