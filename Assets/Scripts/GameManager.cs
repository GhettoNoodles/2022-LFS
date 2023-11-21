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

    public void DamagePlayer()
    {
        _currentHP--;
        uiManager.SetHealth(_currentHP);
        if (_currentHP <= 0)
        {
            Debug.Log("Game Over");
        }
        else
        {
            player.transform.position = activeCP.GetPosition();
            activeCP.Spawn();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}