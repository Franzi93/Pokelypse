using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public FightStateMachine fightUI;

    public void startFight(Creature _player, Creature _enemy) {
        fightUI.enemy = _enemy;
        fightUI.player = _player;
        fightUI.gameObject.SetActive(true);
    }

	public void closeMenu(GameObject menu) {
        menu.SetActive(false);
    }
    public void openMenu(GameObject menu) {
        menu.SetActive(true);
    }

    public static void openDialog(string[] text, Dialog dialogBox) {
        dialogBox.lines = text;
        dialogBox.gameObject.SetActive(true);
    }

}
