using System.Collections.Generic;
using System.Collections;
using Library;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    #region References
    MemoryManagement memoryManagement = new MemoryManagement();
    DataManagement dataManagement = new DataManagement();
    public Sprite lockedLevel;
    public AudioSource ButtonSound;
    #endregion

    [Header("BUTTONS")]
    public List<Button> LevelButtons;


    [Header("LANGUAGE")]
    public List<MainLanguageObject> MainLanguageObjects = new List<MainLanguageObject>();
    List<MainLanguageObject> CurrentLanguageObject = new List<MainLanguageObject>();
    public TextMeshProUGUI[] TextObjects;

    [Header("LOADING")]
    public GameObject LoadingPanel;
    public Slider LoadingSlider;

    public int level;

    void Start()
    {
        ButtonSound.volume = memoryManagement.ReadFloatData("MenuVFX");
        
        memoryManagement.SetIntData("LastLevel", level);
        int currentLevel = memoryManagement.ReadIntData("LastLevel") - 4;
        int Index = 1;
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            if (i + 1 <= currentLevel)
            {
                LevelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
                int levelIndex = Index + 4;
                LevelButtons[i].onClick.AddListener(delegate { SceneLoad(levelIndex); });
            }
            else
            {
                LevelButtons[i].GetComponent<Image>().sprite = lockedLevel;
                LevelButtons[i].enabled = false;
            }
            Index++;
        }
        
        dataManagement.LoadLanguageData();
        CurrentLanguageObject = dataManagement.ProcessReadingTempLanguageList();
        MainLanguageObjects.Add(CurrentLanguageObject[2]);
        LanguageManagement();

        
    }

    public void LanguageManagement()
    {
        if (memoryManagement.ReadStringData("Language") == "TR")

            TextObjects[0].text = MainLanguageObjects[0].LanguageObject_TR[0].Text;
        else
            TextObjects[0].text = MainLanguageObjects[0].LanguageObject_EN[0].Text;
    }
    
    public void SceneLoad(int index)
    {
        ButtonSound.Play();
        StartCoroutine(LoadAsync(index));
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


    public void BackButton()
    {
        ButtonSound.Play();
        SceneManager.LoadScene(0);
    }
    
    
    
}
