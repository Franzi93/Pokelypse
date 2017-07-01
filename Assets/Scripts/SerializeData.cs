using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SerializeData : MonoBehaviour {
    public static string inventoryJsonPath;

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

    
}
