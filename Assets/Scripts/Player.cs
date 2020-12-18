using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    // Tunables
    [SerializeField] float maxHitPoints = 5f;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip deathSound = null;

    // State
    float currentHitPoints = 10f;
    bool isAlive = true;

    // Cached References
    AudioSource audioSource = null;
    HeadsUpDisplay headsUpDisplay = null;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
        LockCursor(true);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hitSound;
        headsUpDisplay = FindObjectOfType<HeadsUpDisplay>();
    }

    public void DecrementHitPoints(float hitDamage)
    {
        if (!audioSource.isPlaying && currentHitPoints > 0)
        {
            audioSource.Play();
        }
        currentHitPoints = Mathf.Clamp(currentHitPoints - hitDamage, 0f, maxHitPoints);
        headsUpDisplay.ToggleBloodSplatter();
        headsUpDisplay.UpdateHealthHUD();
        if (currentHitPoints <= 0)
        {
            QueueDeathSequence();
        }
    }

    private void QueueDeathSequence()
    {
        if (isAlive)
        {
            isAlive = false;
            audioSource.clip = deathSound;
            audioSource.Play();
            DisableWeaponsAndMovement();
            FindObjectOfType<GameSession>().LoadGameOverOverlay();
        }
    }

    private void DisableWeaponsAndMovement()
    {
        LockCursor(false);
        GetComponent<RigidbodyFirstPersonController>().enabled = false;
        WeaponController weaponController = GetComponentInChildren<WeaponController>();
        weaponController.DisableAllWeapons();
        weaponController.SetControllerActive(false);
    }

    public bool CheckIfAlive()
    {
        return isAlive;
    }

    private void LockCursor(bool lockState)
    {
        if (lockState)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public float GetHitPoints()
    {
        return currentHitPoints;
    }
}
