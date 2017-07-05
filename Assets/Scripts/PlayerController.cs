using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public List<Creature> creatures = new List<Creature>();
    public bool inFight = false;
    public float moveSpeed;
    public UIManager manager;

    private Animator anim;

    private bool playerMoving;
    private Vector2 LastMove;
	// Use this for initialization
	void Start () {
       
        
        anim = GetComponent<Animator>();		
	}
	
	// Update is called once per frame
	void Update () {

        playerMoving = false;

		if(Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f )
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            playerMoving = true;
            LastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }

        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f,Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            playerMoving = true;
            LastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", LastMove.x);
        anim.SetFloat("LastMoveY", LastMove.y);
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.tag.Equals("Creature") && coll.gameObject.GetComponent<Captured>() == null && !inFight)
        {
            Creature c = coll.gameObject.GetComponent<Creature>();
            manager.startFight(creatures[0], c);
            inFight = true;
        }
        if (coll.tag.Equals("Spawn") )
        {
            Vector3 v = coll.GetComponent<SpawnPoint>().destination.transform.position;
            transform.position = v;
            Camera.main.transform.position = v;
        }
    }

    public void looseCreature(Creature c) {
        creatures.Remove(c);
        Destroy(c.gameObject);
    }
}
