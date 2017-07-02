using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCollection{
    [SerializeField]
    public List<Item> items = new List<Item>();

    public Item newItem() {
        int id = items.Count;
        Item i =new Item();
        i.id = id;
        i.title = "item";
        items.Add(i);
        i.sprite = Resources.Load<Sprite>("Sprites/Items/" + i.id.ToString());
        return i;
    }

    public Dictionary<int, Item> itemToDict()
    {
        Dictionary<int, Item> data = new Dictionary<int, Item>();
        foreach (Item a in items)
        {
            if (!data.ContainsKey(a.id))
            {
                data.Add(a.id, a);
            }
        }
        return data;
    }

   
}
