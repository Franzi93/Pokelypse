﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public int typeID;
    public string typeName;
   
    public int level = 1;
    public int maxHP = 10;
    public int currHP = 10;

    public int strenght = 5;
    public int defense = 5;
    public int magicalStrenght = 5;
    public int magicalDefense = 5;
    public int initiative = 5;

    public int currEXP;
    public int needEXPToLvlUp = 5;
    public int givesEXP = 5;

    public Attack[] attacks;

    void startUp() {
        givesEXP *= level;
    }

    void catchCreature() {        
        if (UnityEngine.Random.Range(1, 100)<80)
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
        gameObject.AddComponent<Captured>();
    }

    public void getEXP(int _exp) {
        for (int i = 0; i < _exp; i++) {
            ++currEXP;
            if (currEXP == needEXPToLvlUp) {
                levelUp();
                currEXP = 0;
            }
        }
    }

    private void levelUp() {
        needEXPToLvlUp *= 2;
        givesEXP *= 2;
        ++level;
    }

    public void attack(Attack _a,Creature _opponent) {
        int damage;
        damage = _a.strength / 100 * strenght;
        if (_opponent.currHP - damage < 0)
        {
            _opponent.currHP = 0;
        }
        else {
            _opponent.currHP -= damage; 
        }
    }

}