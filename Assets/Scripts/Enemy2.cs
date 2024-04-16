using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    [Header("Settings")]
    public float health = 50f;
    public int currentHealth;
    public float moveSpeed = 5f;

    [Header("Referience")]
    public Transform Target;

    [Header("Zombie Sound")]
    public int volumeZombieAudio;
    public AudioSource zombieAudioSource;

    private GameplayManager gameplayManager;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        gameplayManager = GameObject.FindObjectOfType<GameplayManager>();
        FindPlayer();
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        Chase();
        SettingVolume();
    }

    public void Chase()
    {
        if (Target != null)
        {
            navMeshAgent.SetDestination(Target.position);
        }
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Target = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        gameplayManager.EnemyDied();
    }

    void SettingVolume()
    {
        zombieAudioSource.volume = (float)volumeZombieAudio / 100f;
    }
}
