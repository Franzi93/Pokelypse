using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SerializeData : MonoBehaviour {
    public static string inventoryJsonPath;
    private static string itemsJsonPath = (Directory.GetCurrentDirectory() + "/Assets/Resources/items.json");


    public static Inventory loadJson()
    {
        if (!File.Exists(inventoryJsonPath))
        {
            return new Inventory();
        }
        string dataAsJson = File.ReadAllText(inventoryJsonPath);
        if (dataAsJson.Equals(""))
        {
            return new Inventory();
        }
        Inventory curData = JsonUtility.FromJson<Inventory>(dataAsJson);
        return curData;
    }

    public static void saveJson(Inventory i)
    {
        string Json = JsonUtility.ToJson(i);
        if (!File.Exists(inventoryJsonPath))
        {
            File.Create(inventoryJsonPath);
        }
        File.WriteAllText(inventoryJsonPath, Json);
    }
    public static ItemCollection loadJsonitem()
    {
        if (!File.Exists(itemsJsonPath))
        {
            return new ItemCollection();
        }
        string dataAsJson = File.ReadAllText(itemsJsonPath);
        if (dataAsJson.Equals(""))
        {
            return new ItemCollection();
        }
        ItemCollection curData = JsonUtility.FromJson<ItemCollection>(dataAsJson);
        return curData;
    }

    public static void saveJson(ItemCollection i)
    {
        string Json = JsonUtility.ToJson(i);
        if (!File.Exists(itemsJsonPath))
        {
            File.Create(itemsJsonPath);
        }
        File.WriteAllText(itemsJsonPath, Json);
    }


}
