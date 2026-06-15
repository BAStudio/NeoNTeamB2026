using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private bool IsPaused = false;
    [SerializeField]private GameObject PauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause() 
    {
        if (!IsPaused)
        {
            Time.timeScale = 0f;
            IsPaused = true;
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        if (IsPaused)
        {
            Time.timeScale = 1f;
            IsPaused = false;
            PauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
    }

    public void CloseMenu()
    {
        Pause();
    }
}
