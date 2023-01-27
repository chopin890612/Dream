using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;

public class GameStatusManager: MonoBehaviour
{
    [SerializeField]private Dictionary<string, object> dataStreams;
    public static GameStatusManager instance;
    private string savePath;

    //[Header("TEST")]
    //public SaveStatus playerData;
    //public SaveStatus testt;
    //public string LoadPath;
    //public string Name;
    //public InputAction load;
    //public InputAction save;
    //public InputAction add;
    //public InputAction getp;
    //public InputAction printDiction;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        savePath = $"{Application.persistentDataPath}/save.txt";
        dataStreams = LoadData();

        //LoadPath = savePath;
        //load.Enable();
        //save.Enable();
        //add.Enable();
        //getp.Enable();
        //printDiction.Enable();

        //load.performed += ctx => dataStreams = LoadData();
        //save.performed += ctx => SaveData();
        //add.performed += ctx => AddNew(Name);
        //getp.performed += ctx => GetPlayer(Name);
        //printDiction.performed += ctx => PrintDic();
    }
    //void PrintDic()
    //{
    //    string text = "";
    //    foreach(KeyValuePair<string, object> pair in dataStreams)
    //    {
    //        text += string.Format("Key = {0}, Value = {1}\n", pair.Key, pair.Value);
    //    }
    //    Debug.Log(text);
    //}
    //void GetPlayer(string name)
    //{
    //    playerData = (SaveStatus)ReadStatus(name);
    //}
    //void AddNew(string name)
    //{
    //    AddStatus(name, testt);
    //}


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