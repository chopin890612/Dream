using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;

public class GameStatusManager: MonoBehaviour
{
    public SaveStatus playerData;
    public SaveStatus testt;
    public string LoadPath;

    private Dictionary<string, object> dataStreams;
    private string savePath;

    public InputAction press;

    private void Start()
    {
        savePath = $"{Application.persistentDataPath}/save.txt";
    }
    public object ReadStatus(string name)
    {
        return dataStreams[name];
    }
    public void AddStatus(string name, object status)
    {
        dataStreams.Add(name, status);
    }
    public void SaveData()
    {
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, dataStreams);
        }
    }
    public Dictionary<string, object> LoadData()
    {
        if (!File.Exists(savePath))
        {
            return new Dictionary<string, object>();
        }

        using (var stream = File.Open(savePath, FileMode.Open))
        {
            var formmater = new BinaryFormatter();
            return (Dictionary<string, object>)formmater.Deserialize(stream);
        }
    }
    public string Load()
    {
        using (var stream = File.Open(savePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (string)formatter.Deserialize(stream);
        }
    }
}