using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { ENEMY, PLAYER }

public class FightStateMachine : MonoBehaviour{

    public PlayerController controll;
    public Creature playerCreature;
    public PlayerController player;
    public Creature enemy;

    public Dialog fightDialog;
    public GameObject fightMenuUI;
    FightUI fight;

    public bool playerWon = false;

    public TurnState state;

    
    void OnEnable() {
        
        controll.enabled = false;
        state = playerCreature.initiative > enemy.initiative ? TurnState.PLAYER : TurnState.ENEMY;
        fight = GetComponent<FightUI>();
        fight.player = playerCreature;
        fight.setUpAttackButtons();
        fight.updateHealth();
    }
	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case (TurnState.ENEMY):
                fightMenuUI.SetActive(false);
                enemy.attack(enemy.attacks[0], playerCreature);
                fight.updateHealth();
                fight.switchUI(fightUIState.OPTIONS); 
                state = TurnState.PLAYER;
                break;
            case (TurnState.PLAYER):
                fightMenuUI.SetActive(true);
                //waits for user input
                break;
            default: break;
        }
        if (playerCreature.currHP <= 0)
        {
            playerWon = false;
            endFight();
            player.looseCreature(playerCreature);
        }
        if ( enemy.currHP <= 0)
        {
            playerWon = true;
            endFight();
            Destroy(enemy.gameObject);
        }
    }

    void endFight() {
        if (playerWon)
        {
            playerCreature.getEXP(enemy.givesEXP);
        }
        else {
        }
        controll.enabled = true;
        controll.inFight = false;
        gameObject.SetActive(false);
    }
    public void playerAttack(int _attackIndex)
    {
        playerCreature.attack(playerCreature.attacks[_attackIndex], enemy);
        state = TurnState.ENEMY;
    }


    public void catchCreature(){
        if (Random.Range(1, 10) > 5)
        {
            player.creatures.Add(enemy);
            Captured c = enemy.gameObject.AddComponent<Captured>();
            c.target = player.transform;
            playerWon = true;
            endFight();
        }
        else {
            state = TurnState.ENEMY;
        }
    }
}
