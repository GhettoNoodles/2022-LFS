using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int startHP;
    [SerializeField] private GameObject player;
    private Checkpoint activeCP;
    private int _rings;
    private int _currentHP;
    
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
    public void SetActiveCP(Checkpoint cp)
    {
        activeCP = cp;
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
        AudioManager.Instance.Burn();
        if (_currentHP <= 0)
        {
            EndGame(false);
        }
        else
        {
            //Respawn player
            player.transform.position = activeCP.GetPosition();
            activeCP.Spawn();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}