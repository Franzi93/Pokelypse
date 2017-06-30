using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public string typeName;
    public string givenName;
    public int level;

    public int strenght;
    public int defense;
    public int magicalStrenght;
    public int magicalDefense;
    public int initiative;


    public Job currentJob;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void rename(string _newname) {
        givenName = _newname;
    }

    void catchCreature() {
        if (true)
        {
            catchSuccess();
        }
        else {
            catchFailed();
        }
    }

    private void catchFailed()
    {
        throw new NotImplementedException();
    }

    private void catchSuccess()
    {
        throw new NotImplementedException();
    }
}
