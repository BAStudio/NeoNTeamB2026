using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Diagnostics;

public class SaveSystem : MonoBehaviour
{
    static public SaveSystem instance;
    private void Awake()
    {
        instance = this;
    }


    public void SaveSection<T>(string saveFolder, T data)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFolder);
        File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
    }

    public T LoadSection<T>(string saveFolder)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFolder + ".json");
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
    }
}
