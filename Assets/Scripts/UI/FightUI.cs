using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour {

    public Creature player;
    public GameObject[] buttons;

    public Slider healthSlider;
    public Slider expSlider;
    public Text healthText;


    public void setUpAttackButtons() {
        for(int i = 0; i<buttons.Length;i++) {
            if (player.attacks.Count>i) {
                buttons[i].GetComponent<Button>().interactable = true;
                buttons[i].GetComponentInChildren<Text>().text = player.attacks[i].attackName;
            } else {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponentInChildren<Text>().text = "-";
            }
        }
    }

    public void updateHealth() {
        healthText.text = player.currHP + "/" + player.maxHP;
        healthSlider.maxValue = player.maxHP;
        healthSlider.value = player.currHP;
        expSlider.maxValue = player.needEXPToLvlUp;
        expSlider.value = player.currEXP;
    }
    

}
