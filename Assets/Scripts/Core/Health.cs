using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Header("�̤j��q")]
    [SerializeField] private float maxHealth = 10f;

    [Header("��e��q")]
    [SerializeField] private float currentHealth;

    /// <summary>
    /// ���������� Ĳ�o���e���ƥ�
    /// </summary>
    public event Action onDamage;

    /// <summary>
    /// �����v¡�� Ĳ�o���e���ƥ�
    /// </summary>
    public event Action<float> onHeal;

    /// <summary>
    /// ��H�����`�� Ĳ�o���e���ƥ�
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
        currentHealth = Mathf.Max(currentHealth, 0); //�קK��q�֩�0

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
