using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Serialization;

[Serializable]
public class GameState
{
    [JsonProperty("active_cp")] public int activeCP;
    [JsonProperty("ring_transforms")] public bool[] ringStates = new bool[8];
    [JsonProperty("player_position")] public Vector3 playerPos;
    [JsonProperty("player_rings")] public int playerRings;
    [JsonProperty("player_hp")] public int playerHP;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private int saveNum;
    public string dataPath = "";
    public string savesListPath = "";
    private GameState _gameState = new GameState();
    public List<string> saves;
    [SerializeField] private UIManager uim;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        savesListPath = Application.persistentDataPath + "/" + "game_saves" + ".json";
        if (!File.Exists(savesListPath))
        {
            saves = new List<string>();
        }
        else
        {
            string jsonData = System.IO.File.ReadAllText(savesListPath);
            saves = JsonConvert.DeserializeObject<List<string>>(jsonData);
        }

        
        uim.UpdateSaves(saves);
    }


    public void SaveGame(int activeCheckpoint, bool[] rings, Vector3 playerPos,
        int ringsCollected, int playerHP)
    {
        string saveName = DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss");
        //saveName= saveName.Replace(' ', '_');
        //saveName = saveName.Replace('\\', '-');
        dataPath = Application.persistentDataPath + "/" + "game_save_" + saveName + ".json";
       
        Debug.Log("DATA PATH: " + dataPath);
        //checkpoints
        _gameState.activeCP = activeCheckpoint;
        //rings
        for (int i = 0; i < GameManager.Instance.ringAmt; i++)
        {
            _gameState.ringStates[i] = rings[i];
        }

        //playerpos
        _gameState.playerPos = playerPos;
        _gameState.playerRings = ringsCollected;
        _gameState.playerHP = playerHP;
        string output = JsonConvert.SerializeObject(_gameState, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        System.IO.File.WriteAllText(dataPath, output);
        if (!saves.Contains(saveName))
        {
            saves.Add(saveName);
            output = JsonConvert.SerializeObject(saves);
            System.IO.File.WriteAllText(savesListPath, output);
            uim.UpdateSaves(saves);
        }
        Debug.Log("Data written.");
    }

    public GameState readGameStateFromFile(string saveName)
    {
        dataPath = Application.persistentDataPath + "/" + "game_save_" + saveName + ".json";
        string jsonData = System.IO.File.ReadAllText(dataPath);
        return JsonConvert.DeserializeObject<GameState>(jsonData);
    }

    public void DeleteSave(string saveName)
    {
        if (saves.Contains(saveName))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/" + "game_save_" + saveName + ".json");
            saves.Remove(saveName);
            uim.UpdateSaves(saves);
            string  output = JsonConvert.SerializeObject(saves);
            System.IO.File.WriteAllText(savesListPath, output);
            uim.UpdateSaves(saves);
        }
    }
}