using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDead : DamageBase
{
    [SerializeField]private GameObject GameOverUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FullyBodyHealth = 100f;
        GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FullyBodyHealth == 0f) 
        {
            GameOver();
            GameOverUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            FullyBodyHealth -= 50f;
            Debug.Log("Players health: " + FullyBodyHealth);
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
