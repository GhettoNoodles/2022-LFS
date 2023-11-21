using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int startHP;
    [SerializeField] private GameObject player;
    private Checkpoint activeCP;
    private int _rings;
    private int _currentHP;

    public void SetActiveCP(Checkpoint cp)
    {
        activeCP = cp;
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _currentHP = startHP;
        uiManager.SetHealth(_currentHP);
        uiManager.SetRing(_rings);
    }

    public void IncreaseRings()
    {
        _rings++;
        uiManager.SetRing(_rings);
    }

    public int GetRings()
    {
        return _rings;
    }

    public void DamagePlayer()
    {
        _currentHP--;
        uiManager.SetHealth(_currentHP);
        if (_currentHP <= 0)
        {
            EndGame(false);
        }
        else
        {
            player.transform.position = activeCP.GetPosition();
            activeCP.Spawn();
        }
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
        uiManager.GameOverScreen(victory);
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}