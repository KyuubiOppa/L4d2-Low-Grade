using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameplayManager gameplayManager;

    void Start()
    {
        gameplayManager = GameObject.FindObjectOfType<GameplayManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            gameplayManager.GameOver();
        }
    }
}
