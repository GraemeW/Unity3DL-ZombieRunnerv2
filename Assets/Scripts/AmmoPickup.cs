using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoType ammoType = AmmoType.Bullets;
    [SerializeField] int ammoAmount = 5;
    [SerializeField] int ammoAmountJitter = 2;
    [SerializeField] AudioClip pickupSound = null;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.3f);
            }

            player.GetComponent<Ammo>().AddAmmo(ammoType, UnityEngine.Random.Range(ammoAmount, ammoAmount + ammoAmountJitter));
            Destroy(gameObject);
        }
    }
}
