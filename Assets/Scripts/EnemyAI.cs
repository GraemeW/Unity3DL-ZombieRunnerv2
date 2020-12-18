using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Tunables
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 2f;
    [SerializeField] AudioClip[] ambientEnemySounds = null;
    [SerializeField] float timeBetweenAmbientAudio = 6.0f;

    // Cached References
    NavMeshAgent navMeshAgent = null;
    Player target = null;
    Animator animator = null;
    EnemyAttack enemyAttack = null;
    EnemyHealth enemyHealth = null;
    AudioSource audioSource = null;

    // State
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(EnemyAmbientAudio());
    }

    private void Update()
    {
        if (enemyHealth.GetAliveStatus())
        {
            distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            CheckForProvoked();
            EngageTarget();
        }
    }

    private IEnumerator EnemyAmbientAudio()
    {
        while (!isProvoked && enemyHealth.GetAliveStatus())
        {
            if (!audioSource.isPlaying)
            {
                AudioClip audioClip = ambientEnemySounds[(int)UnityEngine.Random.Range(0, ambientEnemySounds.Length)];
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            yield return new WaitForSeconds(timeBetweenAmbientAudio);
        }
    }

    private void CheckForProvoked()
    {
        if (!target.CheckIfAlive())
        {
            SetProvoked(false);
            return;
        }
        if (distanceToTarget < chaseRange)
        {
            SetProvoked(true);
        }
    }

    private void EngageTarget()
    {
        if (isProvoked)
        {
            FaceTarget();
            if (distanceToTarget >= navMeshAgent.stoppingDistance)
            {
                ChaseTarget();
            }

            if (distanceToTarget <= navMeshAgent.stoppingDistance)
            {
                AttackTarget();
            }
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.y));
        transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ChaseTarget()
    {
        animator.SetBool("isAttacking", false);
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void AttackTarget()
    {
        animator.SetBool("isAttacking", true);
    }

    public void SetProvoked(bool provoked)
    {
        if (!isProvoked)
        {
            animator.SetTrigger("move");
        }
        animator.SetBool("hasTarget", provoked);
        enemyAttack.SetCurrentTarget(target);
        isProvoked = provoked;
    }

    public void DisableAI()
    {
        SetProvoked(false);
        navMeshAgent.enabled = false;
        enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        // Display chase range when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
