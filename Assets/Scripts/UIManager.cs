using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject pauseSF;
    [SerializeField] private GameObject loadSF;
    [SerializeField] private Button loadbtn;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private GameObject gameOverSF;
    [SerializeField] private TextMeshProUGUI ringsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TMP_Dropdown saves;
    private string _currentSave;

    public void SetRing(int rings)
    {
        ringsText.text = "Rings: " + rings;
    }

    public void SetHealth(int hp)
    {
        healthText.text = "HP: " + hp;
    }

    public void PauseScreen()
    {
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        loadPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseSF);
    }


    public void LoadScreen()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        loadPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loadSF);
    }

    public void GameScreen()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        endGamePanel.SetActive(false);
        loadPanel.SetActive(false);
    }

    public void GameOverScreen(bool victory)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverSF);
        int rings = GameManager.Instance.GetRings();
        int hp = GameManager.Instance.GetHP();

        //Format Score for EndGame
        resultText.text = victory ? "You Win!" : "You Lose.";
        scoreText.text = "<#FF9500>Rings:\t" + rings
                                             + "\n<#8C0000>HP:\t\t" + hp + " x 4\n<#000000>Total:\t" + (rings + 4 * hp);

        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(true);
    }

    public void UpdateSaves(List<string> savesList)
    {
        if (savesList.Count > 0)
        {
            saves.ClearOptions();
            saves.AddOptions(savesList);
            SetCurrentSave();
            loadbtn.interactable = true;
            deleteBtn.interactable = true;
            saves.interactable = true;
        }
        else
        {
            saves.ClearOptions();
            saves.interactable = false;
            loadbtn.interactable = false;
            deleteBtn.interactable = false;
        }
    }

    public void SetCurrentSave()
    {
        int value = saves.value;
        _currentSave = saves.options[value].text;
        Debug.Log(_currentSave);
    }

    public string GetSelectedSave()
    {
        return _currentSave;
    }

    public void DeleteCurrentSave()
    {
        SaveManager.Instance.DeleteSave(GetSelectedSave());
    }
}