using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIType {
    Passive, Scared, Aggressive
}

public enum AIState {
    Idle, Wandering, Attacking, Fleeing
}

public class NPC : MonoBehaviour {
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public string[] dropOnDeath;

    [Header("AI")]
    public AIType aiType;
    private AIState aiState;
    public float detectDistance;
    public float safeDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    //components
    private NavMeshAgent agent;
    private Animator anim;
    private SkinnedMeshRenderer[] meshRenderers;


    // get components
    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start() {
        SetState(AIState.Wandering);
    }

    void Update() {
        // player distance
        playerDistance = Vector3.Distance(transform.position, ThirdPersonController.Instance.transform.position);
        switch (aiState) {
            case AIState.Idle: PassiveUpdate(); break;
            case AIState.Wandering: PassiveUpdate(); break;
            case AIState.Attacking: AttackingUpdate(); break;
            case AIState.Fleeing: FleeingUpdate(); break;
        }
    }

    void PassiveUpdate() {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f) {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
        if (aiType == AIType.Aggressive && playerDistance < detectDistance) {
            SetState(AIState.Attacking);
        } else if (aiType == AIType.Scared && playerDistance < detectDistance) {
            SetState(AIState.Fleeing);
            agent.SetDestination(GetFleeLocation());
        }
    }
    void AttackingUpdate() {
        if (playerDistance > attackDistance) {
            agent.isStopped = false;
            agent.SetDestination(ThirdPersonController.Instance.transform.position);
        } else {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate) {
                lastAttackTime = Time.time;
                if (ThirdPersonController.Instance) {
                    ThirdPersonController.Instance.TakeDamage(damage);
                }
            }

        }
    }
    void FleeingUpdate() {
        if (playerDistance < safeDistance && agent.remainingDistance < 0.1f) {
            agent.SetDestination(GetFleeLocation());
        } else if (playerDistance > safeDistance) {
            SetState(AIState.Wandering);
        }
    }
    void SetState(AIState newState) {
        aiState = newState;
        switch (aiState) {
            case AIState.Idle: {
                    agent.speed = walkSpeed;
                    agent.isStopped = true;
                    break;
                }
            case AIState.Wandering: {
                    agent.speed = walkSpeed;
                    agent.isStopped = false;
                    break;
                }
            case AIState.Attacking: {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                    break;
                }
            case AIState.Fleeing: {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                    break;
                }
        }
    }
    void WanderToNewLocation() {
        if (aiState != AIState.Idle)
            return;
        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation() {
        NavMeshHit hit;
        // finds player
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance) {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            // no infinite loop
            if (i == 30) break;
        }
        return hit.position;
    }

    Vector3 GetFleeLocation() {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, safeDistance, NavMesh.AllAreas);
        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance) {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, safeDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }
        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPosition) {
        return Vector3.Angle(transform.position - GameObject.FindGameObjectWithTag("Player").transform.position, transform.position + targetPosition);
    }

    public void TakeDamage(int damage) {
        print("Hit");
        health -= damage;
        print(health);
        if (health <= 0) {
            Die();
        }
        StartCoroutine(DamageFlash());
        if (aiType == AIType.Passive) SetState(AIState.Fleeing);
    }

    void Die() {
        agent.isStopped = true;
        foreach (string item in dropOnDeath) {
            // change this to items instead of strings
            print(item);
        }
        if (agent.isStopped) Destroy(gameObject);
    }

    IEnumerator DamageFlash() {
        foreach (SkinnedMeshRenderer renders in meshRenderers)
            renders.material.color = new Color(1.0f, 0.6f, 0.6f);
        yield return new WaitForSeconds(0.1f);
        foreach (SkinnedMeshRenderer renders in meshRenderers)
            renders.material.color = Color.white;
    }
}

