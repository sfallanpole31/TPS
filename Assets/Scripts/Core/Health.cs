using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Header("最大血量")]
    [SerializeField] private float maxHealth = 10f;

    [Header("當前血量")]
    [SerializeField] private float currentHealth;

    /// <summary>
    /// 當受到攻擊時 觸發的委派事件
    /// </summary>
    public event Action onDamage;

    /// <summary>
    /// 當受到治癒時 觸發的委派事件
    /// </summary>
    public event Action<float> onHeal;

    /// <summary>
    /// 當人物死亡時 觸發的委派事件
    /// </summary>
    public event Action onDie;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;

    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;



        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); //避免血量少於0

        if (currentHealth > 0 && onDamage != null)
        {
            onDamage.Invoke();
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

    }

    private void HandleDeath()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            isDead = true;
            if (onDie != null)
            {
                onDie.Invoke();
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}
