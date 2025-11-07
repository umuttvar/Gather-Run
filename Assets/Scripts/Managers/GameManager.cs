using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Library;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Android.Gradle.Manifest;

public class GameManager : MonoBehaviour
{

    [Header("PLAYERS")]
    public GameObject mainPlayer;
    public List<GameObject> Enemies;
    public List<GameObject> SubPlayers;
    public GameObject endPoint;

    [Header("EFFECTS")]
    public List<GameObject> SpawnEffects;
    public List<GameObject> DestroyEffects;
    public List<GameObject> DecalEffects;

    [Header("HATS")]
    public List<GameObject> Hats;

    [Header("STICKS")]
    public List<GameObject> Sticks;

    [Header("MATERIALS")]
    public List<Material> Materials;
    public Material defaultMaterial;

    [Header("LANGUAGE")]
    public List<MainLanguageObject> MainLanguageObjects = new List<MainLanguageObject>();
    List<MainLanguageObject> CurrentLanguageObject = new List<MainLanguageObject>();
    public TextMeshProUGUI[] TextObjects;

    [Header("PANELS")]
    public List<GameObject> ProcessPanels = new List<GameObject>();
    public GameObject LoadingPanel;

    public Slider LoadingSlider;

    [Header("AUDIOS")]
    public List<AudioSource> Sounds = new List<AudioSource>();
    public Slider GameVolumeSlider;

    #region References

    public SkinnedMeshRenderer skinnedMeshRenderer;
    Scene scene;
    MemoryManagement memoryManagement = new MemoryManagement();
    DataManagement dataManagement = new DataManagement();
    #endregion

    #region Variables
    public static int currentPlayerAmount = 1;
    public int currentEnemyAmount;
    public bool isGameOver;
    public bool isEndGame;
    #endregion

    void Awake()
    {
        GameVolumeSlider.value = memoryManagement.ReadFloatData("GameVolume");
        Sounds[0].volume = memoryManagement.ReadFloatData("GameVolume");
        Sounds[1].volume = memoryManagement.ReadFloatData("MenuVFX");

        Destroy(GameObject.FindWithTag("MenuMusic"));
        CreateEnemy();
        ItemControl();

        dataManagement.LoadLanguageData();
        CurrentLanguageObject = dataManagement.ProcessReadingTempLanguageList();
        MainLanguageObjects.Add(CurrentLanguageObject[5]);
        LanguageManagement();
    }
    void Start()
    {
        CreateEnemy();
        scene = SceneManager.GetActiveScene();
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
    public void EndGameState()
    {
        if (isGameOver) return;

        if (isEndGame)
        {
            Debug.Log(currentPlayerAmount);
            if (currentEnemyAmount == 0)
            {
                isGameOver = true;
                foreach (var enemy in Enemies)
                {
                    if (enemy.activeInHierarchy)
                    {
                        enemy.GetComponent<Animator>().SetBool("Attack", false);
                    }
                }

                foreach (var player in SubPlayers)
                {
                    if (player.activeInHierarchy)
                    {
                        player.GetComponent<Animator>().SetBool("Attack", false);
                    }
                }

                mainPlayer.GetComponent<Animator>().SetBool("Attack", false);
                memoryManagement.SetIntData("Puan", memoryManagement.ReadIntData("Puan") + 600);

                if (scene.buildIndex == memoryManagement.ReadIntData("LastLevel")) { }
                memoryManagement.SetIntData("LastLevel", (memoryManagement.ReadIntData("LastLevel") + 1));

                ProcessPanels[2].SetActive(true);
            }

            else if (currentPlayerAmount < currentEnemyAmount || currentPlayerAmount == currentEnemyAmount)
            {
                
                isGameOver = true;
                ProcessPanels[3].SetActive(true);

            }

        }

    }

    public void CreateEnemy()
    {
        for (int i = 0; i < currentEnemyAmount; i++)
        {
            Enemies[i].SetActive(true);
        }
    }
    public void EnemyTrigger()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.activeInHierarchy)
            {
                enemy.GetComponent<Enemy>().ProcessAttack();
            }
        }

        isEndGame = true;
        EndGameState();
    }
    public void SubPlayerManagement(string operationType, int number, Transform spawnPos)
    {
        switch (operationType)
        {
            case "Multiplication":
                ArithmeticOperations.Multiplication(number, SubPlayers, spawnPos, SpawnEffects);
                break;

            case "Addition":
                ArithmeticOperations.Addition(number, SubPlayers, spawnPos, SpawnEffects);
                break;

            case "Subtraction":
                ArithmeticOperations.Subtraction(number, SubPlayers, DestroyEffects);
                break;

            case "Divison":
                ArithmeticOperations.Divison(number, SubPlayers, DestroyEffects);
                break;
        }
    }
    public void ProcessDestroyEfect(Vector3 spawnPlayerPos, bool isHammer = false, bool isEnemy = false)
    {
        if (!isHammer)
        {
            foreach (var effect in DestroyEffects)
            {
                if (!effect.activeInHierarchy)
                {
                    effect.SetActive(true);
                    effect.transform.position = spawnPlayerPos;
                    effect.GetComponent<ParticleSystem>().Play();
                    effect.GetComponent<AudioSource>().Play();
                    if (isEnemy)
                    {

                        currentEnemyAmount = Math.Max(0, currentEnemyAmount - 1);
                    }
                    else
                    {
                        currentPlayerAmount = Math.Max(0, currentPlayerAmount - 1);
                    }
                    break;
                }
            }
        }

        if (isHammer)
        {
            foreach (var effect in DecalEffects)
            {
                if (!effect.activeInHierarchy)
                {
                    effect.SetActive(true);
                    effect.transform.position = spawnPlayerPos;
                    StartCoroutine(DecalEffectRoutine(effect));
                    break;
                }
            }
        }
        if (!isGameOver) EndGameState();
    }
    IEnumerator DecalEffectRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(4f);
        effect.SetActive(false);
    }

    public void ItemControl()
    {
        if (memoryManagement.ReadIntData("CurrentHat") != -1)
            Hats[memoryManagement.ReadIntData("CurrentHat")].SetActive(true);

        if (memoryManagement.ReadIntData("CurrentStick") != -1)
            Sticks[memoryManagement.ReadIntData("CurrentStick")].SetActive(true);

        if (memoryManagement.ReadIntData("CurrentColor") != -1)
        {
            Material[] mats = skinnedMeshRenderer.materials;
            mats[0] = Materials[memoryManagement.ReadIntData("CurrentColor")];
            skinnedMeshRenderer.materials = mats;
        }
        else
        {
            Material[] mats = skinnedMeshRenderer.materials;
            mats[0] = defaultMaterial;
            skinnedMeshRenderer.materials = mats;
        }

    }

    public void ButtonState(string state)
    {
        Sounds[1].Play();
        Time.timeScale = 0;
        switch (state)
        {
            case "Settings":
                ProcessPanels[1].SetActive(true);
                break;

            case "Exit":
                ProcessPanels[0].SetActive(true);
                break;

            case "Continue":
                ProcessPanels[0].SetActive(false);
                Time.timeScale = 1;
                break;

            case "Replay":
                SceneManager.LoadScene(scene.buildIndex);
                Time.timeScale = 1;
                ProcessPanels[0].SetActive(false);
                break;

            case "MainMenu":
                SceneManager.LoadScene(0);
                Time.timeScale = 1;
                break;
        }
        
    }

    public void Settings(string state)
    {
        switch (state)
        {
            case "Exit":
                ProcessPanels[1].SetActive(false);
                Time.timeScale = 1;
                break;
        }
    }

    public void AdjustGameVolume()
    {
        memoryManagement.SetFloatData("GameVolume", GameVolumeSlider.value);
        Sounds[0].volume = GameVolumeSlider.value;
    }

    public void NextLevelButton()
    {
        StartCoroutine(LoadAsync(scene.buildIndex + 1));
    }
}


