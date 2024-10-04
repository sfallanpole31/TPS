using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProjectileType
{
    Collider,
    Particle,
}

public class Projectile : MonoBehaviour
{
    [Header("子彈形式")]
    [SerializeField] ProjectileType type;
    [Header("擊中特效")]
    [SerializeField] GameObject hitParticle;
    [Header("擊中特效存活時間")]
    [SerializeField] float particleLifeTime = 2f;
    [Header("子彈速度")]
    [SerializeField] float projectileSpeed;
    [Header("Projectile存活時間")]
    [SerializeField] float maxLifeTime = 3f;
    [Header("子彈下墜力度")]
    [SerializeField] float gravityDownForce = 0f;

    [Header("子彈傷害")]
    [SerializeField]
    float damage = 40f;

    GameObject owner;
    bool canAttack;
    Vector3 currentVelocity;

    private void OnEnable()
    {
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += currentVelocity * Time.deltaTime;

        if (gravityDownForce > 0)
        {
            currentVelocity += gravityDownForce * Vector3.down * Time.deltaTime;
        }
    }

    public void Shoot(GameObject gameObject)
    {
        owner = gameObject;
        currentVelocity = transform.forward * projectileSpeed;
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner || !canAttack)
            return;

        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && type == ProjectileType.Collider)
        {
            Health targetHealth = other.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);

            }
        }


        HitEffect(transform.position);
        Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == owner || !canAttack)
            return;

        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && type == ProjectileType.Particle)
        {
            Health targetHealth = other.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);

            }
        }
        HitEffect(transform.position);
    }

    private void HitEffect(Vector3 hitPoint)
    {
        if (hitParticle)
        {
            GameObject newParticleInstance = Instantiate(hitParticle, hitPoint, transform.rotation);
            if (particleLifeTime > 0)
            {
                Destroy(newParticleInstance, particleLifeTime);
            }
        }
    }
}
