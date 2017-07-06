using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captured : MonoBehaviour {

    public string givenName;
    public Job currentJob;

    public Transform target;
    public float spaceToPlayer = 1;
    private NavMeshAgent2D nav;

    void Start() {
        nav = GetComponent<NavMeshAgent2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(transform.position, target.position) > spaceToPlayer)
        {//move if distance from target is greater than 1
            nav.destination = target.position;
        }
    }

    void rename(string _newname)
    {
        givenName = _newname;
    }
}
