using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Tunables
    [Header("Gun Properties")]
    [SerializeField] [Tooltip("Shots per second")] float fireRate = 2f;
    [SerializeField] float damagePerHit = 1.0f;
    [SerializeField] float range = 100f;
    [SerializeField] ParticleSystem muzzleFlash = null;
    [SerializeField] GameObject hitVFXPrefab = null;
    [SerializeField] float hitVFXTimeout = 1f;
    [Header("Ammo Properties")]
    [SerializeField] Ammo ammoSlot = null;
    [SerializeField] AmmoType ammoType = AmmoType.Bullets;
    [Header("Sound Effects")]
    [SerializeField] AudioClip shotSound = null;
    [SerializeField] AudioClip emptyShotSound = null;

    // State
    bool isFiring = false;

    // Cached References
    Camera fPCamera = null;
    AudioSource audioSource = null;

    private void Start()
    {
        fPCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shotSound;
    }

    private void OnEnable()
    {
        StartCoroutine(WeaponSwitchTimeout());
    }

    private void Update()
    {
        HandleMainWeapon();
    }

    private IEnumerator WeaponSwitchTimeout()
    {
        isFiring = true;
        yield return new WaitForSeconds(1 / fireRate);
        isFiring = false;
    }

    private void HandleMainWeapon()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isFiring && ammoSlot.GetAmmoAmount(ammoType) > 0)
            {
                StartCoroutine(Shoot()); // Lock-out firing until reload complete
            }
            if (ammoSlot.GetAmmoAmount(ammoType) <= 0 && !audioSource.isPlaying)
            {
                audioSource.clip = emptyShotSound;
                audioSource.Play();
            }
        }
    }

    private IEnumerator Shoot()
    {
        isFiring = true;
        audioSource.clip = shotSound;
        audioSource.Play();
        PlayMuzzleFlash();
        ProcessRaycast();
        ammoSlot.DecrementAmmoAmount(ammoType);
        yield return new WaitForSeconds(1 / fireRate);
        isFiring = false;
    }

    private void PlayMuzzleFlash()
    {
        if (!muzzleFlash.isPlaying) { muzzleFlash.Play(); }
        else
        {
            muzzleFlash.Stop();
            muzzleFlash.Play();
        }
        
    }

    private void ProcessRaycast()
    {
        int layerMask = ~LayerMask.GetMask("Pickup");
        if (Physics.Raycast(fPCamera.transform.position, fPCamera.transform.forward, out RaycastHit hit, range, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != null)
            {
                QueueHitVFX(hit);
                DamageEnemy(hitObject);
            }
        }
    }

    private void DamageEnemy(GameObject hitObject)
    {
        EnemyHealth enemyHealth = hitObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.DecrementHitPoints(damagePerHit);
        }
    }

    private void QueueHitVFX(RaycastHit hit)
    {
        if (hitVFXPrefab != null)
        {
            GameObject hitVFX = Instantiate(hitVFXPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitVFX, hitVFXTimeout);
        }
    }

    public AmmoType GetAmmoType()
    {
        return ammoType;
    }
}
