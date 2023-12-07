using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int startHP;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private List<Vector3> ringPositions;
    [SerializeField] private List<Vector3> ringRotations;
    [SerializeField] private List<Checkpoint> checkpoints;

    public List<Ring> _rings;
    private Checkpoint _activeCp;
    private Checkpoint _defaultCp;
    private int _ringsCollected;
    private int _currentHP;
    public int ringAmt;
    private GameState defaultGameState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            ringAmt = ringPositions.Count;
            bool[] temp =new bool[ringAmt];
            Array.Fill(temp, true);
            defaultGameState = new GameState();
            defaultGameState.ringStates = temp;
            defaultGameState.playerPos = player.transform.position;
            defaultGameState.playerRings = 0;
            defaultGameState.playerHP = startHP;
            defaultGameState.activeCP = 0;
            defaultGameState.playerVelocity = Vector3.zero;
        }
    }

    private void Start()
    {
        Time.timeScale = 0f;
        uiManager.MainMenu();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        _currentHP = startHP;
        uiManager.SetHealth(_currentHP);
        uiManager.SetRing(_ringsCollected);
        for (int i = 0; i < ringAmt; i++)
        {
            var temp = Instantiate(ringPrefab, ringPositions[i], Quaternion.Euler(ringRotations[i]));
            _rings.Add(temp.GetComponent<Ring>());
        }

        uiManager.GameScreen();
    }

    public void SetActiveCP(Checkpoint cp)
    {
        _activeCp = cp;
    }

    public void IncreaseRings()
    {
        _ringsCollected++;
        uiManager.SetRing(_ringsCollected);
    }

    public int GetRings()
    {
        return _ringsCollected;
    }

    public void SaveGame()
    {
        uiManager.Save();
        bool[] ringStates = new bool[ringAmt];
        for (int i = 0; i < ringAmt; i++)
        {
            ringStates[i] = _rings[i].gameObject.activeSelf;
        }

        SaveManager.Instance.SaveGame(_activeCp.cpNum, ringStates, player.transform.position, _ringsCollected,
            _currentHP,playerRB.velocity);
    }

    public void LoadButton()
    {
        LoadGame(SaveManager.Instance.readGameStateFromFile(uiManager.GetSelectedSave()));
    }
    public void LoadGame(GameState gs)
    {
        //hp
        _currentHP = gs.playerHP;
        uiManager.SetHealth(_currentHP);
        //cp
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.Deactivate();
        }
        _activeCp = checkpoints[gs.activeCP];
        _activeCp.Activate();
        //rings
        for (int i = 0; i < ringAmt; i++)
        {
            _rings[i].gameObject.SetActive(gs.ringStates[i]);
        }

        _ringsCollected = gs.playerRings;
        uiManager.SetRing(_ringsCollected);
        
        //player
        player.transform.position = gs.playerPos;
        playerRB.velocity = gs.playerVelocity;
        ResumeGame();
        uiManager.GameScreen();
    }

    public void DamagePlayer()
    {
        _currentHP--;
        uiManager.SetHealth(_currentHP);
        AudioManager.Instance.Burn();
        if (_currentHP <= 0)
        {
            EndGame(false);
        }
        else
        {
            //Respawn player
            player.transform.position = _activeCp.GetPosition();
            _activeCp.Spawn();
        }
    }

    public int GetHP()
    {
        return _currentHP;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        uiManager.PauseScreen();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        uiManager.GameScreen();
    }

    public void EndGame(bool victory)
    {
        Time.timeScale = 0;
        //Play Audios
        if (victory)
        {
            AudioManager.Instance.Win();
        }
        else
        {
            AudioManager.Instance.Lose();
        }

        uiManager.GameOverScreen(victory);
    }

    public void ResetGame()
    {
        LoadGame(defaultGameState);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}