using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject pauseSF;
    [SerializeField] private GameObject gameOverSF;
    [SerializeField] private TextMeshProUGUI ringsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultText;


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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseSF);
    }

    public void GameScreen()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        endGamePanel.SetActive(false);
    }

    public void GameOverScreen(bool victory)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverSF);
        int rings = GameManager.Instance.GetRings();
        int hp = GameManager.Instance.GetHP();
        
        //Format Score for EndGame
        resultText.text = victory ? "You Win!" : "You Lose.";
        scoreText.text = "Rings:\t" + rings
            + "\nHP:\t\t"+ hp +" x 4\nTotal:\t"+(rings+4*hp);
        
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        endGamePanel.SetActive(true);
    }
}