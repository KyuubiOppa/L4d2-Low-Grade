using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    [Header("Settings Gun")]
    public int damage = 5;
    public float reloadTime = 0.5f; //ระยะเวลา Reload

    [Header("Bullet")]
    public int maxBullet = 25; // จำนวนกระสุนสูงสุด
    public int currentBullet;

    [Header("Referience & UI")]
    public Transform bulletSpawnPoint; //ตำแหน่งปล่อยกระสุน
    public GameObject bulletPrefab; //กระสุน
    public Image crosshair; //ตำแหน่ง Crosshair
    public TMP_Text bulletCountText; //จำนวนกระสุนที่มี

    [Header("Bullet Sound")]
    public int volumeShootAudio;
    public AudioSource bulletAudioSource;
    public AudioClip bulletSound;

    [Header("Reload Sound")]
    public int volumeReloadAudio;
    public AudioSource reloadAudioSource;
    public AudioClip reloadSound;

    [Header("Settings Other")]
    public float bulletSpeed = 10; //ความเร็วการเคลื่อนที่ของกระสุน
    public float shootInterval = 0.1f; //เวลาระหว่างการยิง

    //Raycast
    private RaycastHit hit;
    private Ray ray;

    //กดค้างเพื่อยิง
    private bool isShooting = false;

    void Start()
    {
        isShooting = false;

        currentBullet = maxBullet;

        bulletAudioSource = gameObject.AddComponent<AudioSource>();
        bulletAudioSource.clip = bulletSound;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            StartCoroutine(ShootRoutine());
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
        if (Input.GetKeyDown(KeyCode.R) && !isShooting)
        {
            Reload();
        }

        SettingVolume();
        UpdateBulletCountUI();
    }

    /////////////////////////////////////// Shoot ///////////////////////////////////////////
    IEnumerator ShootRoutine()
    {
        while (isShooting && currentBullet > 0)
        {
            Shoot();
            currentBullet--; 
            yield return new WaitForSeconds(shootInterval);
        }
    }
    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - bulletSpawnPoint.position;
            direction.Normalize();
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = direction * bulletSpeed;

            if (bulletAudioSource != null && bulletSound != null)
            {
                bulletAudioSource.PlayOneShot(bulletSound);
            }
        }
        else
        {
            Vector3 direction = bulletSpawnPoint.forward;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = direction * bulletSpeed;

            if (bulletAudioSource != null && bulletSound != null)
            {
                bulletAudioSource.PlayOneShot(bulletSound);
            }
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
