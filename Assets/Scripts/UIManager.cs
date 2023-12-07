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
    [SerializeField] private GameObject mainmenuPanel;
    [SerializeField] private GameObject pauseSF;
    [SerializeField] private GameObject loadSF;
    [SerializeField] private GameObject mmSF;
    [SerializeField] private GameObject backBtn;
    [SerializeField] private Button loadbtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private TextMeshProUGUI saveBtnText;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private GameObject gameOverSF;
    [SerializeField] private TextMeshProUGUI ringsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TMP_Dropdown saves;
    private string _currentSave;
    private bool victory;

    enum BackTarget
    {
        MainMenu,
        PauseMenu,
        EndGameMenu,
    };

    private BackTarget currentBackTarget = BackTarget.MainMenu;

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
        if (!mainmenuPanel.activeSelf && !endGamePanel.activeSelf)
        {
            saveBtn.interactable = true;
            saveBtnText.text = "Save Game";
            pausePanel.SetActive(true);
            gamePanel.SetActive(false);
            endGamePanel.SetActive(false);
            loadPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseSF);
        }
    }

    public void Back()
    {
        switch (currentBackTarget)
        {
            case BackTarget.MainMenu:
                MainMenu();
                break;
            case BackTarget.PauseMenu:
                PauseScreen();
                break;
            case BackTarget.EndGameMenu:
                GameOverScreen(victory);
                break;
        }
    }
    public void LoadScreen()
    {
        if (pausePanel.activeSelf)
        {
            currentBackTarget = BackTarget.PauseMenu;
            pausePanel.SetActive(false);
        }

        if (mainmenuPanel.activeSelf)
        {
            currentBackTarget = BackTarget.MainMenu;
            mainmenuPanel.SetActive(false);
        }

        if (endGamePanel.activeSelf)
        {
            currentBackTarget = BackTarget.EndGameMenu;
            endGamePanel.SetActive(false);
        }

        loadPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loadSF);
    }

    public void MainMenu()
    {
        mainmenuPanel.SetActive(true);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        loadPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mmSF);
    }

    public void GameScreen()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        endGamePanel.SetActive(false);
        loadPanel.SetActive(false);
        mainmenuPanel.SetActive(false);
    }

    public void GameOverScreen(bool victory)
    {
        this.victory = victory;
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
        mainmenuPanel.SetActive(false);
        loadPanel.SetActive(false);
    }

    public void Save()
    {
        saveBtn.interactable = false;
        saveBtnText.text = "Saved";
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseSF);
    }


    public void UpdateSaves(List<string> savesList)
    {
        savesList.Reverse();
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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backBtn);
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