using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDatabase {
    [SerializeField]
    public List<Item> items = new List<Item>();

    public void newItem() {
        int id = items.Count;

        items.Add(new Item(id, "item", true));
    }
}
[System.Serializable]
public class Item
{
    public int id { get; set; }
    public string title { get; set; }
    public bool stackable { get; set; }
    public int stacksizemax { get; set; }
    public int value { get; set; }
    public int vitality { get; set; }

    public Item(int _id, string _title, bool _stackable)
    {
        this.id = _id;
        this.title = _title;
        this.stackable = _stackable;
    }

}