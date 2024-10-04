using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("追趕距離")]
    [SerializeField] float chaseDistance = 10f;

    [Header("失去目標 困惑時間")]
    [SerializeField] float confuseTime = 5f;

    [Header("巡邏點")]
    [SerializeField] PatrolPath patrolPath;

    [Header("每個巡邏點等待時間")]
    [SerializeField] float wayPointToWaitTime = 3f;

    [Header("巡邏時速度")]
    [Range(0, 1)]
    [SerializeField] float patrolSpeedRatio = 0.5f;

    [Header("需要到達WayPoint的距離")]
    [SerializeField] float wayPointToStay = 3f;

    /// <summary>
    /// 上次看到玩家的時間
    /// </summary>
    private float timeSineceLastSawPlayer = Mathf.Infinity;

    /// <summary>
    /// 原點座標
    /// </summary>
    private Vector3 beginPosition;

    /// <summary>
    /// 當前目標的巡邏點
    /// </summary>
    public int currentWayPointIndex = 0;

    float timeSinceArriveWayPoint = 0;

    bool isBeHit;

    Mover mover;
    Animator animator;
    GameObject player;
    Health health;
    Fighter fighter;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<Fighter>();

        beginPosition = transform.position;
        health.onDamage += OnDamage;
        health.onDie += OnDie;
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    health.TakeDamage(10);
        //    //print("當前血量: "+health.GetCurrentHealth());
        //}

        if (health.IsDead())
            return;

        if (IsInRange() || isBeHit || fighter.actorType == Actor.Zombie) //追趕範圍內
        {
            AttackBehaviour();

        }
        else if (timeSineceLastSawPlayer < confuseTime)  //困惑動作
        {
            ConfuseBehaviour();
        }
        else
        {
            PatrolBehaviour();

        }

        UpdateTimer();
    }

    private void AttackBehaviour()
    {
        animator.SetBool("IsConfuse", false);
        timeSineceLastSawPlayer = 0;
        fighter.Attack(player.GetComponent<Health>());
    }

    /// <summary>
    /// 巡邏動作
    /// </summary>
    private void PatrolBehaviour()
    {


        Vector3 nextWayPointPosition = beginPosition;
        if (patrolPath != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                animator.SetBool("IsConfuse", true);
                timeSineceLastSawPlayer = 0;
                currentWayPointIndex = patrolPath.GetNextWayPointNumber(currentWayPointIndex);

            }


            if (timeSinceArriveWayPoint > wayPointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrolPath.GetWayPointPosition(currentWayPointIndex), patrolSpeedRatio);
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(beginPosition, 0.5f);
        }

    }

    /// <summary>
    /// 檢查是否已經抵達巡邏點
    /// </summary>
    /// <returns></returns>
    private bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrolPath.GetWayPointPosition(currentWayPointIndex)) < wayPointToStay);
    }

    /// <summary>
    /// 困惑動作
    /// </summary>
    private void ConfuseBehaviour()
    {
        mover.CancelMove();
        fighter.CancelTarget();
        animator.SetBool("IsConfuse", true);
    }

    private void UpdateTimer()
    {
        timeSineceLastSawPlayer += Time.deltaTime;
        timeSinceArriveWayPoint += Time.deltaTime;
    }

    /// <summary>
    /// 是否小於追趕距離內
    /// </summary>
    /// <returns></returns>
    private bool IsInRange()
    {
        //print("是否小於追趕距離內:" + (Vector3.Distance(transform.position, player.transform.position) < chaseDistance).ToString());
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void OnDamage()
    {
        isBeHit = true;
    }

    private void OnDie()
    {
        mover.CancelMove();
        GetComponent<CapsuleCollider>().enabled = false;
        animator.SetTrigger("IsDead");
        Destroy(gameObject,3f);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }



}
