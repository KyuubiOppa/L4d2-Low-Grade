using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float destroyDelay = 5f;

    private Gun gun;

    void Start()
    {
        StartCoroutine(DestroyBulletAfterDelay());
    }

    void Update()
    {
        FindGun();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DealDamage(collision.gameObject);

            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = Vector3.zero;
            }
            Destroy(gameObject);
        }
    }

    public void DealDamage(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }

    public void FindGun()
    {
        gun = GameObject.FindObjectOfType<Gun>();
        if (gun != null)
        {
            damage = gun.damage;
        }
    }

    IEnumerator DestroyBulletAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}