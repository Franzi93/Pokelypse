using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { ENEMY, PLAYER }

public class FightStateMachine : MonoBehaviour{

    public PlayerController controll;
    public Creature player;
    public Creature enemy;

    public Dialog fightDialog;
    public GameObject attackUI;
    FightUI att;

    public bool playerWon = false;

    public TurnState state;

    
    void OnEnable() {
        controll.enabled = false;
        state = player.initiative > enemy.initiative ? TurnState.PLAYER : TurnState.ENEMY;
        att = GetComponent<FightUI>();
        att.player = player;
        att.setUpAttackButtons();
        att.updateHealth();
    }
	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case (TurnState.ENEMY):
                attackUI.gameObject.SetActive(false);
                enemy.attack(enemy.attacks[0], player);
                att.updateHealth();
                state = TurnState.PLAYER;
                break;
            case (TurnState.PLAYER):
                attackUI.gameObject.SetActive(true);
                //waits for user input
                break;
            default: break;
        }
        if (player.currHP <= 0 || enemy.currHP <= 0) {
            endFight();
        }
	}

    void endFight() {
        if (playerWon)
        {
            player.getEXP(enemy.givesEXP);
            Destroy(enemy.gameObject);
        }
        else {
           
        }
        controll.enabled = true;
        controll.inFight = false;
        gameObject.SetActive(false);
    }
    public void playerAttack(int _attackIndex)
    {
        player.attack(player.attacks[_attackIndex], enemy);
        state = TurnState.ENEMY;
    }
}
