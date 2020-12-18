using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Tunables
    [SerializeField] float maxHitPoints = 10f;
    [SerializeField] AudioClip enemyDeathSound = null;

    // State
    float currentHitPoints = 10f;
    bool isAlive = true;

    // Cached References
    EnemyAI enemyAI = null;
    Animator animator = null;
    AudioSource audioSource = null;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void QueueDeathSequence()
    {
        audioSource.clip = enemyDeathSound;
        audioSource.Play();
        enemyAI.DisableAI();
        StopPhysicsInteractions();
        isAlive = false;
        animator.SetBool("isAlive", false);
    }

    private void StopPhysicsInteractions()
    {
        Rigidbody enemyRigidbody = GetComponent<Rigidbody>();
        enemyRigidbody.useGravity = false;
        enemyRigidbody.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    public void DecrementHitPoints(float hitDamage)
    {
        //BroadcastMessage("SetProvoked", true); -- per lectures, unused because string literal
        enemyAI.SetProvoked(true);
        currentHitPoints -= hitDamage;
        if (currentHitPoints <= 0)
        {
            QueueDeathSequence();
        }
    }

    public bool GetAliveStatus()
    {
        return isAlive;
    }
}
