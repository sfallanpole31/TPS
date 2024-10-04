using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFighter : MonoBehaviour
{
    [Header("�����ˮ`")]
    [SerializeField] float attackDamage = 10f;

    [Header("���D�����ˮ`")]
    [SerializeField] float jumpAttackDamage = 50f;
    
    [Header("�����Z��")]
    [SerializeField] float attackRange = 2f;

    [Header("���D�����Z��")]
    [SerializeField] float jumpAttackRange = 2f;

    [Header("�����ɶ����Z")]
    [SerializeField] float timeBetweenAttack = 2f;

    [Header("�n��X��Projectile")]
    [SerializeField] Projectile throwProjectile;

    [Header("�ⳡ�y��")]
    [SerializeField] Transform hand;

    [Header("�����������")]
    [SerializeField] Actor actorType;


    RootMover mover;
    Animator animator;
    Health health;
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<RootMover>();
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
            mover.CancelMove();
            AttackBehaviour("Attack");
        }
        else if(IsInJumpAttackRange())
        {
            mover.CancelMove();
            AttackBehaviour("JumpAttack");
        }
        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position);
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
        if (baseLayer.fullPathHash == Animator.StringToHash("Mutant Swiping"))
        {
            print("�S���b��������ʵe");
            return false;
            
        }
        else
        {
            print("���b��������ʵe");
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
    private void AttackBehaviour(string attackName)
    {
        transform.LookAt(targetHealth.transform);
        if (timeSinceLastAttack > timeBetweenAttack)
        {
            timeSinceLastAttack = 0;
            TriggerAttack(attackName);
        }
    }

    private void TriggerAttack(string attackName)
    {

        animator.ResetTrigger(attackName);
        animator.SetTrigger(attackName);




    }

    private bool IsInJumpAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < jumpAttackRange;
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



    private void Hit()
    {
        //if (targetHealth == null ||  actorType != Actor.Boss)
        //    return;

        if (IsInAttackRange())
            targetHealth.TakeDamage(attackDamage);
    }

    private void JumpHit()
    {
        if (targetHealth ==  null || actorType != Actor.Boss)
            return;

        if (IsInJumpAttackRange())
            targetHealth.TakeDamage(jumpAttackDamage);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, jumpAttackRange);
    }
}
