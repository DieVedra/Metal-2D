using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController singletone = null;
    //++++++++++++++++++++++++++++++++++++++++

    public int lastSceneIndex;

    AudioSource mainMenuTheme;

    Animator animUI;

    public Image image;

    public bool reLoadedScene = false;

    //+++++++++++++++++++++++++++++++++++++++++

    private void Awake()
    {
        singletone = this;
    }
    void Start()
    {
        mainMenuTheme = GetComponent<AudioSource>();
        animUI = GetComponent<Animator>();
        reLoadedScene = true;
    }
    void Update()
    {
        MenuMusicVolUp();
    }
    void MenuMusicVolUp()
    {
        if (mainMenuTheme.volume < 1)
        {
            mainMenuTheme.volume += Time.deltaTime;
        }
    }
    public void StartGame()
    {
        animUI.SetBool("StartMenuBack", true);
        animUI.SetBool("TransitChooseLVL", false);

        if (lastSceneIndex == 0)
        {
            lastSceneIndex += 1;
        }

        StartCoroutine(LoadLevelAcyng(lastSceneIndex));
    }
    public void LVLNumber(int index)
    {
        animUI.SetBool("MenuChooseLVLBack", true);

        animUI.SetBool("ItLVL", false);

        lastSceneIndex = index;

        StartCoroutine(LoadLevelAcyng(index));
    }
    public void LevelChooseButton()
    {
        animUI.SetBool("StartMenuBack", true);
        animUI.SetBool("TransitChooseLVL", true);
        animUI.SetBool("MenuChooseLVLBack", false);
    }
    public void LevelChooseButtonBack()
    {
        animUI.SetBool("MenuChooseLVLBack", true);
        animUI.SetBool("ItLVL", true);
        animUI.SetBool("StartMenuBack", false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    IEnumerator LoadLevelAcyng(int levelIndex)
    {
        yield return new WaitForSeconds(0.8f);

        image.gameObject.SetActive(true);

        AsyncOperation asyngLoad = SceneManager.LoadSceneAsync(levelIndex);

        asyngLoad.allowSceneActivation = false;

        while (!asyngLoad.isDone)
        {
            image.fillAmount = asyngLoad.progress;
            if (asyngLoad.progress >= .9f && !asyngLoad.allowSceneActivation /*&& nextScene*/)
            {
                asyngLoad.allowSceneActivation = true;
            }

            yield return 0;
        }
    }
}
