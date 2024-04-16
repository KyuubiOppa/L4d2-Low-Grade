using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : MonoBehaviour
{
    public int boostDamage;
    public float doubleDamageCooldown;

    private Gun1 gun1;
    private Gun gun;

    void Start()
    {
        gun = GameObject.FindObjectOfType<Gun>();
        gun1 = GameObject.FindObjectOfType<Gun1>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (null != gun)
            {
                gun.ApplyDoubleDamage(boostDamage, doubleDamageCooldown);
            }
            
            if (null != gun1)
            {
                gun1.ApplyDoubleDamage(boostDamage, doubleDamageCooldown);
            }
            Destroy(gameObject);
        }
    }
}
