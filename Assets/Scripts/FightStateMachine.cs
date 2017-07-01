using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TurnState { ENEMY, PLAYER }

public class FightStateMachine : MonoBehaviour{

    public Creature player;
    public Creature enemy;

    public Dialog fightDialog;
    public AttackUI attackUI;

    public GameObject fightUI;

    public bool playerWon = false;

    private TurnState state;


	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case (TurnState.ENEMY):
                enemy.attack(enemy.attacks[0], player);
                state = TurnState.PLAYER;
                break;
            case (TurnState.PLAYER):

                //waits for user input
                break;
            default: break;
        }
	}

    void startFight() {
        fightUI.SetActive(true);
        state = player.initiative > enemy.initiative ? TurnState.PLAYER : TurnState.ENEMY;
    }
    void endFight() {
        fightUI.SetActive(false);
        if (playerWon)
        {
            player.getEXP(enemy.givesEXP);
        }
        else {

        }
    }
}
