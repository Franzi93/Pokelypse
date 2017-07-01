using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackUI : MonoBehaviour {

    public Creature player;
    public GameObject[] buttons;

    public void setUpAttackButtons() {
        for(int i = 0; i<buttons.Length;i++) {
            if (player.attacks[i] != null) {
                buttons[i].GetComponent<Button>().interactable = true;
                buttons[i].GetComponent<Text>().text = player.attacks[i].attackName;
            } else {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponent<Text>().text = "-";
            }
        }
    }

}
