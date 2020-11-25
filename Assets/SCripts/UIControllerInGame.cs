using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControllerInGame : MonoBehaviour
{
    public static UIControllerInGame singletone = null;

    //public GameObject player;

    public static bool gameInPause = false;


    public Image im;

    public Image pauseMenu;

    public GameObject btnRestart;

    public GameObject btnResume;




    private void Awake()
    {
        singletone = this;
    }
    

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);

        if (Player.singletone.healthPlayer <= 0)
        {
            btnResume.SetActive(false);
        }
        else 
        {
            btnResume.SetActive(true); 
        }

        gameInPause = true;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        gameInPause = false;

        pauseMenu.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ExitToMainMenu()
    {
        LevelController.singletone.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Time.timeScale = 1f;

        StartCoroutine(LoadLevel(0));
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int index)
    {
        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene(index);
        
        yield return 0;
    }
 }
