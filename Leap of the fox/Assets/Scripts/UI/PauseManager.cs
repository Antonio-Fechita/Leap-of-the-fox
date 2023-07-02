using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    [SerializeField] GameObject pausePanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            OnResume();
        }
    }

    private void OnPauseEnter()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnPauseExit()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnRestartLevel()
    {
        OnResume();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnResume()
    {
        if (isPaused)
        {
            OnPauseExit();
        }
        else
        {
            OnPauseEnter();
        }

        isPaused = !isPaused;
    }



}
