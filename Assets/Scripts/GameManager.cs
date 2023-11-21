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
    private Vector3 startPos;
    private int _rings;
    private int _currentHP;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _currentHP = startHP;
        startPos = player.transform.position;
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
            player.transform.position = startPos;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}