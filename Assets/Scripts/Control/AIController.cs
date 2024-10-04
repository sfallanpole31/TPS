using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("�l���Z��")]
    [SerializeField] float chaseDistance = 10f;

    [Header("���h�ؼ� �x�b�ɶ�")]
    [SerializeField] float confuseTime = 5f;

    [Header("�����I")]
    [SerializeField] PatrolPath patrolPath;

    [Header("�C�Ө����I���ݮɶ�")]
    [SerializeField] float wayPointToWaitTime = 3f;

    [Header("���ޮɳt��")]
    [Range(0, 1)]
    [SerializeField] float patrolSpeedRatio = 0.5f;

    [Header("�ݭn��FWayPoint���Z��")]
    [SerializeField] float wayPointToStay = 3f;

    /// <summary>
    /// �W���ݨ쪱�a���ɶ�
    /// </summary>
    private float timeSineceLastSawPlayer = Mathf.Infinity;

    /// <summary>
    /// ���I�y��
    /// </summary>
    private Vector3 beginPosition;

    /// <summary>
    /// ��e�ؼЪ������I
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
        //    //print("��e��q: "+health.GetCurrentHealth());
        //}

        if (health.IsDead())
            return;

        if (IsInRange() || isBeHit || fighter.actorType == Actor.Zombie) //�l���d��
        {
            AttackBehaviour();

        }
        else if (timeSineceLastSawPlayer < confuseTime)  //�x�b�ʧ@
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
    /// ���ްʧ@
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
    /// �ˬd�O�_�w�g��F�����I
    /// </summary>
    /// <returns></returns>
    private bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrolPath.GetWayPointPosition(currentWayPointIndex)) < wayPointToStay);
    }

    /// <summary>
    /// �x�b�ʧ@
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
    /// �O�_�p��l���Z����
    /// </summary>
    /// <returns></returns>
    private bool IsInRange()
    {
        //print("�O�_�p��l���Z����:" + (Vector3.Distance(transform.position, player.transform.position) < chaseDistance).ToString());
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
