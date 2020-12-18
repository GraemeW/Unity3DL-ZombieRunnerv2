using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Tunables
    [SerializeField] Weapon startingWeapon = null;

    // State
    Weapon currentWeapon = null;
    Weapon[] weaponListing = null;
    int previousScrollIndex = 0;
    int scrollIndex = 0;
    bool weaponsActive = true;

    // Cached References
    HeadsUpDisplay headsUpDisplay = null;

    private void Start()
    {
        InitializeStartingWeapon();
        headsUpDisplay = FindObjectOfType<HeadsUpDisplay>();
    }

    private void InitializeStartingWeapon()
    {
        currentWeapon = startingWeapon;
        weaponListing = GetComponentsInChildren<Weapon>();
        DisableAllWeapons();
        EnableCurrentWeapon();
    }

    private void Update()
    {
        previousScrollIndex = scrollIndex;
        ProcessAlphaKeys();
        ProcessScrollWheel();
        
        if (scrollIndex != previousScrollIndex && weaponsActive)
        {
            SetWeaponActive();
        }
    }

    private void ProcessAlphaKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            scrollIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            scrollIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            scrollIndex = 2;
        }
    }
    
    private void ProcessScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (scrollIndex >= transform.childCount - 1)
            {
                scrollIndex = 0;
            }
            else
            {
                scrollIndex++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (scrollIndex <= 0)
            {
                scrollIndex = transform.childCount - 1;
            }
            else
            {
                scrollIndex--;
            }
        }
    }

    private void SetWeaponActive()
    {
        currentWeapon.gameObject.SetActive(false);
        weaponListing[scrollIndex].gameObject.SetActive(true);
        currentWeapon = weaponListing[scrollIndex];
        headsUpDisplay.UpdateWeaponHUD();
    }

    public void EnableCurrentWeapon()
    {
        currentWeapon.gameObject.SetActive(true);
    }

    public void DisableAllWeapons()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void SetControllerActive(bool isActive)
    {
        weaponsActive = isActive;
    }

    public AmmoType GetCurrentWeaponAmmoType()
    {
        return currentWeapon.GetAmmoType();
    }
}
