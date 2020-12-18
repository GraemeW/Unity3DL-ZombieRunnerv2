using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeadsUpDisplay : MonoBehaviour
{
    // Tunables
    [SerializeField] TextMeshProUGUI ammoText = null;
    [SerializeField] TextMeshProUGUI healthText = null;
    [SerializeField] Image bloodSplatter = null;
    [SerializeField] float splatterDuration = 0.2f;

    // Cached References
    Ammo ammo = null;
    WeaponController weaponController = null;
    Player player = null;

    private void Start()
    {
        ammo = FindObjectOfType<Ammo>();
        weaponController = FindObjectOfType<WeaponController>();
        player = FindObjectOfType<Player>();
        UpdateWeaponHUD();
        UpdateHealthHUD();
        bloodSplatter.gameObject.SetActive(false);
    }

    public void UpdateWeaponHUD()
    {
        AmmoType ammoType = weaponController.GetCurrentWeaponAmmoType();
        ammoText.text = ammo.GetAmmoAmount(ammoType).ToString();
    }

    public void UpdateHealthHUD()
    {
        healthText.text = player.GetHitPoints().ToString();
    }

    public void ToggleBloodSplatter()
    {
        StartCoroutine(QueueBloodSplatter());
    }

    private IEnumerator QueueBloodSplatter()
    {
        bloodSplatter.gameObject.SetActive(true);
        yield return new WaitForSeconds(splatterDuration);
        bloodSplatter.gameObject.SetActive(false);
    }
}
