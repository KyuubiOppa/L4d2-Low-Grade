using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun1 : MonoBehaviour
{
    [Header("Settings Gun")]
    public float damage = 10f;
    public float range = 100f;
    public float reloadTime = 0.5f; //ระยะเวลา Reload

    [Header("Bullet")]
    public int maxBullet = 25; // จำนวนกระสุนสูงสุด
    public int currentBullet;  // จำนวนกระสุนที่มี

    [Header("Referience & UI")]
    public Camera fpsCam; 
    public TMP_Text bulletCountText; //จำนวนกระสุนที่มี
    public ParticleSystem fireEffect;
    public GameObject impactEffect;

    [Header("Settings Other")]
    public float fireRate = 15f;
    public float impactForce = 30f;

    [Header("Bullet Sound")]
    public int volumeShootAudio;
    public AudioSource bulletAudioSource;
    public AudioClip bulletSound;

    [Header("Reload Sound")]
    public int volumeReloadAudio;
    public AudioSource reloadAudioSource;
    public AudioClip reloadSound;

    private float nextTimeToFire = 0f;

    void Start()
    {
        currentBullet = maxBullet;

        bulletAudioSource = gameObject.AddComponent<AudioSource>();
        bulletAudioSource.clip = bulletSound;
    }

    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        SettingVolume();
        UpdateBulletCountUI();
    }

    void Shoot()
    {
        if (currentBullet > 0) 
        {
            fireEffect.Play();
            currentBullet--; 

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                Enemy2 enemy2 = hit.transform.GetComponent<Enemy2>();
                if (enemy2 != null)
                {
                    enemy2.TakeDamage(damage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
            if (bulletAudioSource != null && bulletSound != null)
            {
                bulletAudioSource.PlayOneShot(bulletSound);
            }

            UpdateBulletCountUI(); 
        }
    }

    /////////////////////////////////////// Reload ///////////////////////////////////////////
    public void Reload()
    {
        if (currentBullet < maxBullet)
        {
            StartCoroutine(ReloadCoroutine());
        }
        else
        {
            Debug.Log("Already fully loaded.");
        }
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);

        int bulletsToReload = maxBullet - currentBullet;
        currentBullet += bulletsToReload;
        UpdateBulletCountUI();

        if (reloadAudioSource != null && reloadSound != null)
        {
            reloadAudioSource.PlayOneShot(reloadSound);
        }
    }

    /////////////////////////////////////// UI Bullet ///////////////////////////////////////////
    void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = currentBullet + "/" + maxBullet;
        }
    }
    /////////////////////////////////////// Audio ///////////////////////////////////////////
    void SettingVolume()
    {
        bulletAudioSource.volume = (float)volumeShootAudio / 100f;
        reloadAudioSource.volume = (float)volumeReloadAudio / 100f;
    }
    /////////////////////////////////////// Ability ///////////////////////////////////////////
    public void ApplyDoubleDamage(int boost, float cooldown)
    {
        StartCoroutine(DoubleDamageEffect(boost, cooldown));
    }

    public IEnumerator DoubleDamageEffect(int boost, float cooldown)
    {
        damage *= boost;
        Debug.Log(boost + ": Damage increased!");

        float countdown = cooldown;
        while (countdown > 0)
        {
            Debug.Log("Cooldown: " + countdown.ToString("F1")); // แสดง cooldown ที่มีทศนิยมหนึ่งตำแหน่ง
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        damage /= boost;
        Debug.Log("Double damage cooldown finished. Damage returned to normal.");
    }
}
