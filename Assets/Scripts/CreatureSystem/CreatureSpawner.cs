using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour {

    public List<Creature> creaturesToSpawn = new List<Creature>();
    public int maxSpawns = 2;
    BoxCollider2D spawnArea;
    private List<Creature> spawnedCreatures = new List<Creature>();
    private int currSpawns = 0;
    private float maxY;
    private float maxX;
    private Vector2 maxSpawnPos;

    // Use this for initialization
    void Start () {
        spawnArea = GetComponent<BoxCollider2D>();
        maxSpawnPos = new Vector2(spawnArea.size.x / 2, spawnArea.size.y / 2);
        spawn();
       
	}
    
	// Update is called once per frame
	void spawn () {
        do
        {
            int i = Random.Range(0, creaturesToSpawn.Count );
            Vector3 pos = new Vector3(Random.Range(-maxSpawnPos.x, maxSpawnPos.x), Random.Range(-maxSpawnPos.y, maxSpawnPos.y), 0);
            creaturesToSpawn[i].transform.localPosition = pos;
            Creature spawn = Instantiate(creaturesToSpawn[i],transform);
            spawnedCreatures.Add(spawn);
            ++currSpawns;

        }
        while (currSpawns<maxSpawns);

    }
}
