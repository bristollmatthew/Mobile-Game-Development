using System;
using System.IO;
using UnityEngine;

public class GameSaveScript : MonoBehaviour
{
    public static GameSaveScript First;
    
    [Serializable]
    public class saveData
    {
        public long saveCounter;
        public string saveID;

        public int highscore;

        public float playtimeSeconds;

        public saveData()
        {
            this.saveCounter = 0;
            this.saveID = null;
            this.highscore = 0;
            this.playtimeSeconds = 0;
        }
    }
    public saveData currentData;

    string json;
    public string filePath;

    public void SaveGame(saveData data)
    {
        string jsonString = JsonUtility.ToJson(data);
        data.saveCounter++;
        // data.saveID = 
        File.WriteAllText(filePath, jsonString );
        

        /*PlayerPrefs.SetString("GameSave", jsonString);
        PlayerPrefs.Save();*/
    }
    public saveData LoadGame()
    {
        if (File.Exists(filePath)) 
        {
            string jsonString = File.ReadAllText(filePath);
            saveData data = JsonUtility.FromJson<saveData>(jsonString);
            return data;
        }
        return new saveData();
    }
    void Awake()
    {
        //ClearSave(); //REMOVE
        /*if (First != null)
        {
            Destroy(this.gameObject);
            return;

        }*/
        filePath = UnityEngine.Application.persistentDataPath + "/playerdata.json";
        First = this;
        DontDestroyOnLoad(this.gameObject);
        currentData = LoadGame();
        Debug.Log(currentData);
    }
    void Paused()
    {
       SaveGame(currentData);
    }
    private void OnApplicationFocus(bool focus)
    {
        SaveGame(currentData);
    }
    private void OnApplicationPause(bool pause)
    {
        SaveGame(currentData);
    }
    public void ClearSave()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
