using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Library;
using Unity.VisualScripting;

public class SettingsManager : MonoBehaviour
{
    #region References
    public List<Slider> Sliders = new List<Slider>();
    MemoryManagement memoryManagement = new MemoryManagement();
    DataManagement dataManagement = new DataManagement();
    #endregion

    [Header("LANGUAGE")]
    public List<MainLanguageObject> MainLanguageObjects = new List<MainLanguageObject>();
    List<MainLanguageObject> CurrentLanguageObject = new List<MainLanguageObject>();
    public TextMeshProUGUI[] TextObjects;
    int currentLanguageIndex;


    [Header("BUTTONS")]
    public Button[] ActionButtons;
    public AudioSource ButtonSound;


    void Start()
    {
        Sliders[0].value = memoryManagement.ReadFloatData("MenuVolume");
        Sliders[1].value = memoryManagement.ReadFloatData("MenuVFX");
        Sliders[2].value = memoryManagement.ReadFloatData("GameVolume");

        dataManagement.LoadLanguageData();
        CurrentLanguageObject = dataManagement.ProcessReadingTempLanguageList();
        MainLanguageObjects.Add(CurrentLanguageObject[4]);
        LanguageManagement();
        ControlLanguageSelection();
        
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

    public void ControlLanguageSelection()
    {
        if (memoryManagement.ReadStringData("Language") == "TR")
        {
            currentLanguageIndex = 0;
            ActionButtons[0].interactable = false;
            TextObjects[5].gameObject.SetActive(false);
        }
        else
        {
            currentLanguageIndex = 1;
            ActionButtons[1].interactable = false;
            TextObjects[6].gameObject.SetActive(false);
        }

    }
    
    public void ChangeLanguage(string state)
    {
        if (state == "Next")
        {
            currentLanguageIndex = 1;
            ActionButtons[1].interactable = false;
            ActionButtons[0].interactable = true;
            TextObjects[6].gameObject.SetActive(true);
            TextObjects[5].gameObject.SetActive(false);

            memoryManagement.SetStringData("Language", "EN");
            LanguageManagement();


        }
        else
        {
            currentLanguageIndex = 0;
            ActionButtons[0].interactable = false;
            ActionButtons[1].interactable = true;

            TextObjects[5].gameObject.SetActive(true);
            TextObjects[6].gameObject.SetActive(false);

            memoryManagement.SetStringData("Language", "TR");
            LanguageManagement();

        }
    }
    public void AdjustVolume(string adjust)
    {
        switch (adjust)
        {
            case "MenuVolume":
                memoryManagement.SetFloatData("MenuVolume", Sliders[0].value);
                break;

            case "MenuVFX":
                memoryManagement.SetFloatData("MenuVFX", Sliders[1].value);

                break;

            case "GameVolume":
                memoryManagement.SetFloatData("GameVolume", Sliders[2].value);

                break;
        }
    }
    
    public void ProcessBackButton()
    {
        ButtonSound.Play();
        SceneManager.LoadScene(0);
        
    }
}
