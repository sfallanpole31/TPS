using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actor
{
    Melee,//�i��
    Acher,//���{
    Zombie,//�ľW
    Boss,//BOSS
}

public class Fighter : MonoBehaviour
{
    [Header("�����ˮ`")]
    [SerializeField] float attackDamage = 10f;

    [Header("�����Z��")]
    [SerializeField] float attackRange = 2f;

    [Header("�����ɶ����Z")]
    [SerializeField] float timeBetweenAttack = 2f;

    [Header("�n��X��Projectile")]
    [SerializeField] Projectile throwProjectile;
    [Header("�ⳡ�y��")]
    [SerializeField] Transform hand;
    public Actor actorType;

    Mover mover;
    Animator animator;
    Health health;
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.onDie += OnDie;
    }


    // Update is called once per frame
    void Update()
    {
        if (targetHealth == null || targetHealth.IsDead())
            return;

        if (IsInAttackRange())
        {
            //mover.CancelMove();
            AttackBehaviour();
        }
        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position, 1f);
        }
        UpdateTimer();

    }

    /// <summary>
    /// �ˬd�O�_�b��������ʵe
    /// </summary>
    /// <returns></returns>
    private bool CheckHasAttack()
    {
        baseLayer = animator.GetCurrentAnimatorStateInfo(0);
        if (baseLayer.fullPathHash == Animator.StringToHash("Base Layer.Enemy Attack"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    /// <summary>
    /// �����欰
    /// </summary>
    private void AttackBehaviour()
    {
        transform.LookAt(targetHealth.transform);
        if (timeSinceLastAttack > timeBetweenAttack)
        {
            timeSinceLastAttack = 0;
            TriggerAttack();
        }
    }

    private void TriggerAttack()
    {

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");




    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    public void Attack(Health target)
    {
        targetHealth = target;
    }

    public void CancelTarget()
    {
        targetHealth = null;
    }

    private void OnDie()
    {
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Hit()
    {
        if (targetHealth == null || actorType != Actor.Melee && actorType != Actor.Zombie)
            return;

        if (IsInAttackRange())
            targetHealth.TakeDamage(attackDamage);
    }

    private void Shoot()
    {
        if (targetHealth == null || actorType != Actor.Acher)
            return;

        if (throwProjectile != null)
        {
            Projectile newProjectile = Instantiate(throwProjectile, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }

}
