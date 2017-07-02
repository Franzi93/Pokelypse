using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum attackType {PHYSICAL, MAGICAL }

public class Attack : ScriptableObject {

    public int id;
    public string attackName;
    public int hitChance;
    public int strength;
    public attackType aType;
    public Image icon;
}



