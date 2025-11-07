using UnityEngine;
using UnityEngine.UI;
using Library;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Serialization;
using UnityEngine.InputSystem;

public class Preferences : MonoBehaviour
{
    #region  References
    MemoryManagement memoryManagement = new MemoryManagement();
    DataManagement dataManagement = new DataManagement();
    #endregion
    
    #region Inspector
    public TextMeshProUGUI pointText;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator savedButtonAnimator;
    AnimatorControl animatorControl;
    public GameObject ProcessCanvas;
    public List<GameObject> UIPanels;
    public List<Button> ActionButtons;
    public List<GameObject> ProcessPanels;

    #endregion

    [Header("HATS")]
    public TextMeshProUGUI hatText;
    public List<GameObject> Hats;
    public List<Button> HatButtons;

    [Header("STICKS")]
    public TextMeshProUGUI stickText;
    public List<GameObject> Sticks;
    public List<Button> StickButtons;


    [Header("MATERIALS")]
    public TextMeshProUGUI materialText;
    public Material defaultMaterial;

    public List<Material> Materials;
    public List<Button> MaterialButtons;


    [Header("ITEMS")]
    public List<Items> Items = new List<Items>();

    [Header("SOUNDS")]
    public List<AudioSource> Sounds = new List<AudioSource>();

    [Header("LANGUAGE")]
    public List<MainLanguageObject> MainLanguageObjects = new List<MainLanguageObject>();
    List<MainLanguageObject> CurrentLanguageObject = new List<MainLanguageObject>();
    public TextMeshProUGUI[] TextObjects;


    #region Variables
    int stickIndex = -1;
    int hatIndex = -1;
    int materialIndex = -1;
    int currentPanelIndex;

    string PurchaseText;
    string ItemText;
    #endregion

    void Start()
    {
        memoryManagement.SetIntData("Point", 4500);
        pointText.text = memoryManagement.ReadIntData("Point").ToString();

        dataManagement.LoadItemData();
        Items = dataManagement.ProcessReadingTempItemList();
        Debug.Log(Application.persistentDataPath + "/ItemDatas.gd");

        CurrentPanelControl(0, true);
        CurrentPanelControl(1, true);
        CurrentPanelControl(2, true);

        foreach (var sound in Sounds)
        {
            sound.volume = memoryManagement.ReadFloatData("MenuVFX");
        }
        
        animatorControl = FindFirstObjectByType<AnimatorControl>();

        dataManagement.LoadLanguageData();
        CurrentLanguageObject = dataManagement.ProcessReadingTempLanguageList();
        MainLanguageObjects.Add(CurrentLanguageObject[1]);
        LanguageManagement();
    }

     public void LanguageManagement()
    {
        if (memoryManagement.ReadStringData("Language") == "TR")
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = MainLanguageObjects[0].LanguageObject_TR[i].Text;
            }

            PurchaseText = MainLanguageObjects[0].LanguageObject_TR[5].Text;
            ItemText = MainLanguageObjects[0].LanguageObject_TR[4].Text;
            animatorControl.SavedTextLanguage(memoryManagement.ReadStringData("Language"));
        }

        else
        {
            for (int i = 0; i < TextObjects.Length; i++)
            {
                TextObjects[i].text = MainLanguageObjects[0].LanguageObject_EN[i].Text;
            }

            PurchaseText = MainLanguageObjects[0].LanguageObject_EN[5].Text;
            ItemText = MainLanguageObjects[0].LanguageObject_EN[4].Text;
            animatorControl.SavedTextLanguage(memoryManagement.ReadStringData("Language"));
        }
    }


    public void CurrentPanelControl(int currentPanelIndex, bool process = false)
    {
        switch (currentPanelIndex)
        {
            case 0:

                int savedHatIndex = memoryManagement.ReadIntData("CurrentHat");


                if (savedHatIndex == -1)
                {
                    foreach (var hat in Hats) { hat.SetActive(false); }
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = false;
                    TextObjects[5].text = PurchaseText;

                    if (!process)
                    {
                        hatIndex = -1;
                        hatText.text = ItemText;
                    }
                }

                else if (savedHatIndex >= 0 && savedHatIndex < Hats.Count)
                {
                    foreach (var hat in Hats) { hat.SetActive(false); }

                    if (Hats[savedHatIndex] != null)
                    {
                        hatIndex = savedHatIndex;
                        Hats[hatIndex].SetActive(true);
                        hatText.text = Items[hatIndex].Item_Name;
                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }
                    else
                    {

                        memoryManagement.SetIntData("CurrentHat", -1);
                        CurrentPanelControl(0, false);
                    }
                }

                else
                {
                    memoryManagement.SetIntData("CurrentHat", -1);

                    foreach (var hat in Hats) { hat.SetActive(false); }
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = false;
                    TextObjects[5].text = PurchaseText;
                    if (!process)
                    {
                        hatIndex = -1;
                        hatText.text = ItemText;
                    }
                }
                break;

            case 1:
                if (memoryManagement.ReadIntData("CurrentColor") == -1)
                {
                    if (!process)
                    {
                        materialIndex = -1;
                        materialText.text = ItemText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = false;
                        TextObjects[5].text = PurchaseText;
                    }
                    else
                    {
                        Material[] mats = skinnedMeshRenderer.materials;
                        mats[0] = defaultMaterial;
                        skinnedMeshRenderer.materials = mats;
                    }
                }
                else
                {
                    materialIndex = memoryManagement.ReadIntData("CurrentColor");
                    Material[] mats = skinnedMeshRenderer.materials;
                    mats[0] = Materials[materialIndex];
                    skinnedMeshRenderer.materials = mats;

                    materialText.text = Items[materialIndex + 6].Item_Name;
                    TextObjects[5].text = PurchaseText;
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = true;
                }
                break;

            case 2:
                int savedStickIndex = memoryManagement.ReadIntData("CurrentStick");

                if (savedStickIndex == -1)
                {
                    foreach (var stick in Sticks) { stick.SetActive(false); }
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = false;
                    TextObjects[5].text = PurchaseText;

                    if (!process)
                    {
                        stickIndex = -1;
                        stickText.text = ItemText;
                    }
                }
                else if (savedStickIndex >= 0 && savedStickIndex < Sticks.Count)
                {
                    foreach (var stick in Sticks) { stick.SetActive(false); }

                    if (Sticks[savedStickIndex] != null)
                    {
                        stickIndex = savedStickIndex;
                        Sticks[stickIndex].SetActive(true);


                        if (stickIndex + 3 < Items.Count)
                        {
                            stickText.text = Items[stickIndex + 3].Item_Name;
                        }
                        else
                        {
                            stickText.text = "HATA";
                        }

                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }
                    else
                    {
                        memoryManagement.SetIntData("CurrentStick", -1);
                        CurrentPanelControl(2, false);
                    }
                }
                else
                {
                    memoryManagement.SetIntData("CurrentStick", -1);

                    foreach (var stick in Sticks) { stick.SetActive(false); }
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = false;
                    TextObjects[5].text = PurchaseText;
                    if (!process)
                    {
                        stickIndex = -1;
                        stickText.text = ItemText;
                    }
                }
                break;
        }
    }

    public void ProcessHatButtons(string process)
    {
        Sounds[0].Play();
        if (process == "next")
        {
            if (hatIndex == -1)
            {
                hatIndex = 0;
                Hats[hatIndex].SetActive(true);
                hatText.text = Items[hatIndex].Item_Name;

                if (!Items[hatIndex].isPurchase)
                {
                    TextObjects[5].text = Items[hatIndex].Item_Point + "-" + PurchaseText;
                    ActionButtons[1].interactable = false;

                    if (memoryManagement.ReadIntData("Point") < Items[hatIndex].Item_Point)
                        ActionButtons[0].interactable = false;
                    else
                        ActionButtons[0].interactable = true;
                }
                else
                {
                    TextObjects[5].text = PurchaseText;
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = true;
                }
            }
            else
            {
                if (hatIndex < Hats.Count - 1)
                {
                    Hats[hatIndex].SetActive(false);
                    hatIndex++;
                    Hats[hatIndex].SetActive(true);
                    hatText.text = Items[hatIndex].Item_Name;

                    if (!Items[hatIndex].isPurchase)
                    {
                        TextObjects[5].text = Items[hatIndex].Item_Point + "-" + PurchaseText;
                        ActionButtons[1].interactable = false;

                        if (memoryManagement.ReadIntData("Point") < Items[hatIndex].Item_Point)
                            ActionButtons[0].interactable = false;
                        else
                            ActionButtons[0].interactable = true;
                    }
                    else
                    {
                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }
                }

            }
        }
        else if (process == "previous")
        {
            if (hatIndex != -1)
            {
                Hats[hatIndex].SetActive(false);
                hatIndex--;

                if (hatIndex != -1)
                {
                    Hats[hatIndex].SetActive(true);
                    hatText.text = Items[hatIndex].Item_Name;

                    if (!Items[hatIndex].isPurchase)
                    {
                        TextObjects[5].text = Items[hatIndex].Item_Point + "-" + PurchaseText;
                        ActionButtons[1].interactable = false;

                        if (memoryManagement.ReadIntData("Point") < Items[hatIndex].Item_Point)
                            ActionButtons[0].interactable = false;
                        else
                            ActionButtons[0].interactable = true;
                    }
                    else
                    {
                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }
                }
                else
                {
                    hatText.text = ItemText;
                    TextObjects[5].text = PurchaseText;
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = false;
                }
            }
        }

        if (hatIndex == Hats.Count - 1)
        {
            HatButtons[1].interactable = false;
        }
        else
        {
            HatButtons[1].interactable = true;
        }

        if (hatIndex == -1)
        {
            HatButtons[0].interactable = false;
        }
        else
        {
            HatButtons[0].interactable = true;
        }
    }

    public void ProcessStickButtons(string process)
    {
        Sounds[0].Play();
        if (process == "next")
        {
            if (stickIndex == -1)
            {
                stickIndex = 0;
                Sticks[stickIndex].SetActive(true);

                if (stickIndex + 3 < Items.Count)
                {
                    stickText.text = Items[stickIndex + 3].Item_Name;

                    if (!Items[stickIndex + 3].isPurchase)
                    {
                        TextObjects[5].text = Items[stickIndex + 3].Item_Point + "-" + PurchaseText;
                        ActionButtons[1].interactable = false;
                        if (memoryManagement.ReadIntData("Point") < Items[stickIndex + 3].Item_Point)
                            ActionButtons[0].interactable = false;
                        else
                            ActionButtons[0].interactable = true;
                    }
                    else
                    {
                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }
                }
                else
                {
                    stickText.text = "HATA";
                }
            }
            else
            {
                if (stickIndex + 1 < Sticks.Count)
                {
                    Sticks[stickIndex].SetActive(false);
                    stickIndex++;
                    Sticks[stickIndex].SetActive(true);

                    if (stickIndex + 3 < Items.Count)
                    {
                        stickText.text = Items[stickIndex + 3].Item_Name;

                        if (!Items[stickIndex + 3].isPurchase)
                        {
                            TextObjects[5].text = Items[stickIndex + 3].Item_Point + "-" + PurchaseText
                            ;
                            ActionButtons[1].interactable = false;

                            if (memoryManagement.ReadIntData("Point") < Items[stickIndex + 3].Item_Point)
                                ActionButtons[0].interactable = false;
                            else
                                ActionButtons[0].interactable = true;
                        }
                        else
                        {
                            TextObjects[5].text = PurchaseText;
                            ActionButtons[0].interactable = false;
                            ActionButtons[1].interactable = true;
                        }
                    }
                    else
                    {
                        stickText.text = "HATA";
                    }
                }
            }
        }
        else if (process == "previous")
        {
            if (stickIndex != -1)
            {
                Sticks[stickIndex].SetActive(false);
                stickIndex--;

                if (stickIndex != -1)
                {
                    Sticks[stickIndex].SetActive(true);

                    if (stickIndex + 3 < Items.Count)
                    {
                        stickText.text = Items[stickIndex + 3].Item_Name;

                        if (!Items[stickIndex + 3].isPurchase)
                        {
                            TextObjects[5].text = Items[stickIndex + 3].Item_Point + "-" + PurchaseText;
                            ActionButtons[1].interactable = false;

                            if (memoryManagement.ReadIntData("Point") < Items[stickIndex + 3].Item_Point)
                                ActionButtons[0].interactable = false;
                            else
                                ActionButtons[0].interactable = true;
                        }
                        else
                        {
                            TextObjects[5].text = PurchaseText;
                            ActionButtons[0].interactable = false;
                            ActionButtons[1].interactable = true;
                        }
                    }
                    else
                    {
                        stickText.text = "HATA";
                    }
                }
                else
                {
                    stickText.text = ItemText;
                    TextObjects[5].text = PurchaseText;
                    ActionButtons[0].interactable = false;

                    ActionButtons[1].interactable = false;
                }
            }
        }

        if (stickIndex == Sticks.Count - 1)
        {
            StickButtons[1].interactable = false;
        }
        else
        {
            StickButtons[1].interactable = true;
        }

        if (stickIndex == -1)
        {
            StickButtons[0].interactable = false;
        }
        else
        {
            StickButtons[0].interactable = true;
        }
    }

    public void ProcessMaterialButtons(string process)
    {
        Sounds[0].Play();
        if (process == "next")
        {
            if (materialIndex == -1)
            {
                materialIndex = 0;
                Material[] mats = skinnedMeshRenderer.materials;
                mats[0] = Materials[materialIndex];
                skinnedMeshRenderer.materials = mats;
                materialText.text = Items[materialIndex + 6].Item_Name;

                if (!Items[materialIndex + 6].isPurchase)
                {
                    TextObjects[5].text = Items[materialIndex + 6].Item_Point + "-" + PurchaseText;
                    ActionButtons[1].interactable = false;

                    if (memoryManagement.ReadIntData("Point") < Items[materialIndex + 6].Item_Point)
                        ActionButtons[0].interactable = false;
                    else
                        ActionButtons[0].interactable = true;
                }
                else
                {
                    TextObjects[5].text = PurchaseText;
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = true;
                }

            }
            else
            {
                if (materialIndex < Materials.Count - 1)
                {

                    materialIndex++;
                    Material[] mats = skinnedMeshRenderer.materials;
                    mats[0] = Materials[materialIndex];
                    skinnedMeshRenderer.materials = mats;
                    materialText.text = Items[materialIndex + 6].Item_Name;

                    if (!Items[materialIndex + 6].isPurchase)
                    {
                        TextObjects[5].text = Items[materialIndex + 6].Item_Point + "-" + PurchaseText;
                        ActionButtons[1].interactable = false;

                        if (memoryManagement.ReadIntData("Point") < Items[materialIndex + 6].Item_Point)
                            ActionButtons[0].interactable = false;
                        else
                            ActionButtons[0].interactable = true;
                    }
                    else
                    {
                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }

                }
            }
        }
        else if (process == "previous")
        {
            if (materialIndex != -1)
            {
                materialIndex--;

                if (materialIndex != -1)
                {
                    Material[] mats = skinnedMeshRenderer.materials;
                    mats[0] = Materials[materialIndex];
                    skinnedMeshRenderer.materials = mats;
                    materialText.text = Items[materialIndex + 6].Item_Name;

                    if (!Items[materialIndex + 6].isPurchase)
                    {
                        TextObjects[5].text = Items[materialIndex + 6].Item_Point + "-" + PurchaseText;
                        ActionButtons[1].interactable = false;

                        if (memoryManagement.ReadIntData("Point") < Items[materialIndex + 6].Item_Point)
                            ActionButtons[0].interactable = false;
                        else
                            ActionButtons[0].interactable = true;
                    }
                    else
                    {
                        TextObjects[5].text = PurchaseText;
                        ActionButtons[0].interactable = false;
                        ActionButtons[1].interactable = true;
                    }

                }
                else
                {
                    Material[] mats = skinnedMeshRenderer.materials;
                    mats[0] = defaultMaterial;
                    skinnedMeshRenderer.materials = mats;
                    materialText.text = ItemText;

                    TextObjects[5].text = PurchaseText;
                    ActionButtons[0].interactable = false;
                    ActionButtons[1].interactable = true;
                }
            }
        }

        if (materialIndex == Materials.Count - 1)
        {
            MaterialButtons[1].interactable = false;
        }
        else
        {
            MaterialButtons[1].interactable = true;
        }

        if (materialIndex == -1)
        {
            MaterialButtons[0].interactable = false;
        }
        else
        {
            MaterialButtons[0].interactable = true;
        }
    }

    public void ProcessPanel(int index)
    {
        Sounds[0].Play();
        CurrentPanelControl(index);
        UIPanels[0].SetActive(true);
        currentPanelIndex = index;
        ProcessPanels[currentPanelIndex].SetActive(true);
        UIPanels[1].SetActive(true);
        ProcessCanvas.SetActive(false);
    }

    public void ProcessBackButton() //Item Back Button
    {
        Sounds[0].Play();
        UIPanels[0].SetActive(false);
        ProcessCanvas.SetActive(true);
        UIPanels[1].SetActive(false);
        ProcessPanels[currentPanelIndex].SetActive(false);
        CurrentPanelControl(currentPanelIndex, true);
        currentPanelIndex = -1;
    }

    public void ProcessPurchaseButton()
    {
        Sounds[1].Play();
        switch (currentPanelIndex)
        {
            case 0:
                FinalizePurchase(hatIndex);
                break;

            case 1:
                int Index = materialIndex + 6;
                FinalizePurchase(Index);
                break;

            case 2:
                int Index2 = stickIndex + 3;
                FinalizePurchase(Index2);
                break;
        }
    }

    public void FinalizePurchase(int index)
    {
        Items[index].isPurchase = true;
        memoryManagement.SetIntData("Point", memoryManagement.ReadIntData("Point") - Items[index].Item_Point);
        TextObjects[5].text = PurchaseText;
        ActionButtons[0].interactable = false;
        ActionButtons[1].interactable = true;
        pointText.text = memoryManagement.ReadIntData("Point").ToString();
    }

    public void ProcessSaveButton()
    {
        Sounds[2].Play();
        switch (currentPanelIndex)
        {
            case 0:
                FinalizeSave("CurrentHat", hatIndex);
                break;

            case 1:
                FinalizeSave("CurrentColor", materialIndex);
                break;

            case 2:
                FinalizeSave("CurrentStick", stickIndex);
                break;
        }
    }

    public void FinalizeSave(string key, int index)
    {
        memoryManagement.SetIntData(key, index);
        ActionButtons[1].interactable = false;
        if (!savedButtonAnimator.GetBool("Save"))
            savedButtonAnimator.SetBool("Save", true);
    }

    public void MenuBackButton()
    {
        Sounds[0].Play();
        dataManagement.SaveData(Items);
        SceneManager.LoadScene(0);
    }

   
}
