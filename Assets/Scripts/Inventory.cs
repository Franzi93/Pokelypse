using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour {

    public int maxCapacity;
    public int currCapacity;

    [SerializeField]
    public List<Item> items;
}


