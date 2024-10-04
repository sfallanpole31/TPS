using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFighter : MonoBehaviour
{
    [Header("攻擊傷害")]
    [SerializeField] float attackDamage = 10f;

    [Header("跳躍攻擊傷害")]
    [SerializeField] float jumpAttackDamage = 50f;
    
    [Header("攻擊距離")]
    [SerializeField] float attackRange = 2f;

    [Header("跳躍攻擊距離")]
    [SerializeField] float jumpAttackRange = 2f;

    [Header("攻擊時間間距")]
    [SerializeField] float timeBetweenAttack = 2f;

    [Header("要丟出的Projectile")]
    [SerializeField] Projectile throwProjectile;

    [Header("手部座標")]
    [SerializeField] Transform hand;

    [Header("角色攻擊類型")]
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
    /// 檢查是否在播放攻擊動畫
    /// </summary>
    /// <returns></returns>
    private bool CheckHasAttack()
    {
        baseLayer = animator.GetCurrentAnimatorStateInfo(0);
        if (baseLayer.fullPathHash == Animator.StringToHash("Mutant Swiping"))
        {
            print("沒有在播放攻擊動畫");
            return false;
            
        }
        else
        {
            print("正在播放攻擊動畫");
            return true;
           
        }
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    /// <summary>
    /// 攻擊行為
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
