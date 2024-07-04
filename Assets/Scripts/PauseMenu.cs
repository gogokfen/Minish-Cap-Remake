using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool paused;
    [SerializeField] UnityEvent disableAll;
    [SerializeField] Animator pauseMenuAnimator;
    //[SerializeField] goToMapButton 
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
            pauseMenuAnimator.Play("LeftMenu", 0, 1f);
        }

        if (!paused)
        {
            SFXController.PlaySFX("PauseMenuClose");
            Time.timeScale = 1f;
            disableAll.Invoke();
            pauseMenuAnimator.Play("LeftMenu", 0, 1f);
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
        }
    }
}
