using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public float moveSpeed = 5f;

    [Header("Referience")]
    public Transform Target;

    private GameplayManager gameplayManager;

    void Start()
    {
        gameplayManager = GameObject.FindObjectOfType<GameplayManager>();

        currentHealth = maxHealth;
        FindPlayer();
    }

    void Update()
    {
        Chase();
    }

    public void Chase()
    {
        if (Target != null)
        {
            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        gameplayManager.EnemyDied();
    }
}
