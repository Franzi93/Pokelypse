using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

    ItemCollection coll;

    void Start() {
        coll = SerializeData.loadJsonitem();
    }

    public Item FetchItemByID(int _id)
    {
        return coll.items.First(item => item.id == _id);
    }
}
