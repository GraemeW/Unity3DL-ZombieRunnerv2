using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    // Tunables
    [Tooltip("Set each to unique")][SerializeField] AmmoSlot[] ammoSlots = null;

    // State
    Dictionary<AmmoType, int> ammoAmount = new Dictionary<AmmoType, int>();

    // Cached References
    HeadsUpDisplay headsUpDisplay = null;

    [System.Serializable] private class AmmoSlot
    {
        public AmmoType ammoType = AmmoType.Bullets;
        public int initialAmmoAmount = 0;
    }

    private void Start()
    {
        SetInitialAmmo();
        headsUpDisplay = FindObjectOfType<HeadsUpDisplay>();
    }

    private void SetInitialAmmo()
    {
        foreach (AmmoSlot ammoSlot in ammoSlots)
        {
            ammoAmount[ammoSlot.ammoType] = ammoSlot.initialAmmoAmount;
        }
    }

    public int GetAmmoAmount(AmmoType ammoType)
    {
        return ammoAmount[ammoType];
    }

    public void DecrementAmmoAmount(AmmoType ammoType)
    {
        ammoAmount[ammoType] = Mathf.Max(ammoAmount[ammoType] - 1, 0);
        headsUpDisplay.UpdateWeaponHUD();
    }

    public void AddAmmo(AmmoType ammoType, int ammoAmount)
    {
        this.ammoAmount[ammoType] += ammoAmount;
        headsUpDisplay.UpdateWeaponHUD();
    }
}
