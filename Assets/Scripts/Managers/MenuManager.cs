using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Library;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("EXIT PANEL")]
    public GameObject exitPanel;

    [Header("ITEMS")]
    public List<Items> DefaultItems = new List<Items>();

    [Header("LANGUAGE")]
    public List<MainLanguageObject> DefaultLanguageObjects = new List<MainLanguageObject>();
    public List<MainLanguageObject> MainLanguageObjects = new List<MainLanguageObject>();
    List<MainLanguageObject> CurrentLanguageObject = new List<MainLanguageObject>();
    public TextMeshProUGUI[] TextObjects;

    #region References
    MemoryManagement memoryManagement = new MemoryManagement();
    DataManagement dataManagement = new DataManagement();
    public AudioSource ButtonSound;

    #endregion

    [Header("LOADING")]
    public GameObject LoadingPanel;
    public Slider LoadingSlider;


    void Start()
    {
        memoryManagement.KeyControl();
        dataManagement.SaveDataFirstTime(DefaultItems, DefaultLanguageObjects);
        ButtonSound.volume = memoryManagement.ReadFloatData("MenuVFX");

        dataManagement.LoadLanguageData();
        CurrentLanguageObject = dataManagement.ProcessReadingTempLanguageList();
        MainLanguageObjects.Add(CurrentLanguageObject[0]);
        LanguageManagement();
       
    }

    public void LoadScene(int sceneIndex)
    {
        ButtonSound.Play();
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void PlayButton()
    {
        ButtonSound.Play();
        StartCoroutine(LoadAsync(memoryManagement.ReadIntData("CurrentLevel")));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingPanel.SetActive(true);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            LoadingSlider.value = progress;
            yield return null;
        }

    }
    
    public void ExitButtonState(string state)
    {
        ButtonSound.Play();
        if (state == "Yes")
        {
            Application.Quit();
        }

        else if (state == "Exit")
        {
            exitPanel.SetActive(true);

        }
        else
        {
            exitPanel.SetActive(false);
        }
    }
    public void LanguageManagement()
    {
        if (memoryManagement.ReadStringData("Language") == "TR")
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = MainLanguageObjects[0].LanguageObject_TR[i].Text;
            }
        }

        else
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = MainLanguageObjects[0].LanguageObject_EN[i].Text;
            }
        }        
    } 
}
