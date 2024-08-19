using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool paused;
    public static bool mapPaused;
    [SerializeField] UnityEvent disableAll;
    [SerializeField] Animator pauseMenuAnimator;
    [SerializeField] GameObject[] mapElements;
    void Start()
    {
        paused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            mapPaused = true;
            TogglePause();
        }
    }
    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            SFXController.PlaySFX("PauseMenuOpen");
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            if (mapPaused)
            {
                pauseMenuAnimator.Play("MapMenu", 0, 1f);
                mapPaused = false;
            }
            else
            {
                pauseMenuAnimator.Play("LeftMenu", 0, 1f);
            }
        }

        if (!paused)
        {
            mapPaused = false;
            SFXController.PlaySFX("PauseMenuClose");
            Time.timeScale = 1f;
            disableAll.Invoke();
            pauseMenuAnimator.Play("LeftMenu", 0, 1f);
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
        }
    }

    public void gotMap()
    {
        foreach (GameObject mapElement in mapElements)
        {
            mapElement.SetActive(true);
        }
    }
}
