using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAI : MonoBehaviour {
    public float roamRadius;
    Vector3 startPosition;
    private NavMeshAgent2D _nav;

    public float timerMax = 5;
    public float timerMin = 1;
    private float timeTillNextMove;

    void Start()
    {
        _nav = GetComponent<NavMeshAgent2D>();
        startPosition = transform.position;
        timeTillNextMove = Random.Range(timerMin, timerMax);
    }
    void Update() {
        if (timeTillNextMove <= 0)
        {
            FreeRoam();
            timeTillNextMove = Random.Range(timerMin, timerMax);
        }
        else {
            timeTillNextMove -= Time.deltaTime;
        }

    }

    void FreeRoam()
    {
        
            
        Vector2 randomDirection = Random.insideUnitCircle * roamRadius;
        randomDirection += (Vector2)startPosition;
            
        //NavMeshHit hit;
        //NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector2 finalPosition = randomDirection;
        _nav.destination = finalPosition;
    
    }
}
