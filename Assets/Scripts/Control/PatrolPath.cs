using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [Header("�����I�b�|")]
    [SerializeField] float wayPointGizomsRadius = 1f;
    /// <summary>
    /// ���o�U�@�Ө����I
    /// </summary>
    /// <param name="wayPointNumber"></param>
    /// <returns></returns>
    public int GetNextWayPointNumber(int wayPointNumber)
    {
        if (wayPointNumber + 1 > transform.childCount - 1)
        {
            return 0;
        }
        return wayPointNumber + 1;
    }
    /// <summary>
    /// ���o�����I��m
    /// </summary>
    /// <param name="wayPointNumber"></param>
    /// <returns></returns>
    public Vector3 GetWayPointPosition(int wayPointNumber)
    {
        return transform.GetChild(wayPointNumber).position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.blue;
            int j = GetNextWayPointNumber(i);
            Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
            Gizmos.DrawSphere(GetWayPointPosition(i), wayPointGizomsRadius);
        }
    }

}
