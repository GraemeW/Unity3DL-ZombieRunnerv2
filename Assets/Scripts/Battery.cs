using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] float batteryCharge = 60f;
    [SerializeField] AudioClip pickupSound = null;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            ConsumeBattery(player);
        }
    }

    private void ConsumeBattery(Player player)
    {
        Flashlight flashlight = player.GetComponentInChildren<Flashlight>();
        if (flashlight != null)
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.3f);
            }

            flashlight.AddToBattery(batteryCharge);
            Destroy(gameObject);
        }
    }
}
