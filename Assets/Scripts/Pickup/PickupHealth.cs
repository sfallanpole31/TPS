using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    [Header("�W�[����q")]
    [SerializeField] float healAmount;
    [Header("�P�����ڵ��I��m")]
    [SerializeField] GameObject pickupRoot;

    Pickup pickup;
    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();
        pickup.onPick += OnPick;
    }

    private void OnPick(GameObject player)
    {
        Health health = player.GetComponent<Health>();

        if(health)
        {
            health.Heal(healAmount);
            Destroy(pickupRoot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
