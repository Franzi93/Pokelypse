using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captured : MonoBehaviour {

    public string givenName;
    public Job currentJob;

    public Transform target;
    public float speed = 1;
    public float spaceToPlayer = 1;
    

    // Update is called once per frame
    void Update()
    {

        //rotate to look at the player
        transform.LookAt(target.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation


        //move towards the player
        if (Vector3.Distance(transform.position, target.position) > spaceToPlayer)
        {//move if distance from target is greater than 1
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

    void rename(string _newname)
    {
        givenName = _newname;
    }
}
