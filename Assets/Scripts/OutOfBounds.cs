using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.gameObject.transform.position = transform.parent.transform.position;
            player.gameObject.transform.rotation = transform.parent.transform.rotation;
        }
    }
}
