using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library
{
    public class ArithmeticOperations
    {
        public static void Multiplication(int number, List<GameObject> Players, Transform spawnPos, List<GameObject> SpawnEffects)
        {
            int loopTime = (GameManager.currentPlayerAmount * number) - GameManager.currentPlayerAmount;
            int flag = 0;
            foreach (var player in Players)
            {
                if (flag < loopTime)
                {
                    if (!player.activeInHierarchy)
                    {
                        foreach (var effect in SpawnEffects)
                        {
                            if (!effect.activeInHierarchy)
                            {
                                effect.transform.SetParent(null, true);
                                effect.SetActive(true);
                                effect.transform.position = new Vector3(spawnPos.transform.position.x, 0.23f, spawnPos.transform.position.z);
                                effect.GetComponent<ParticleSystem>().Play();
                                effect.GetComponent<AudioSource>().Play();
                                break;
                            }
                        }
                        player.transform.position = spawnPos.position + Vector3.back * 0.7f;
                        player.SetActive(true);
                        flag++;
                    }
                }
                else
                {
                    break;
                }
            }
            GameManager.currentPlayerAmount *= number;
        }

        public static void Addition(int number, List<GameObject> Players, Transform spawnPos, List<GameObject> SpawnEffects)
        {
            int flag = 0;
            foreach (var player in Players)
            {
                if (flag < number)
                {
                    if (!player.activeInHierarchy)
                    {
                        foreach (var effect in SpawnEffects)
                        {
                            if (!effect.activeInHierarchy)
                            {
                                effect.transform.SetParent(null, true);
                                effect.SetActive(true);
                                effect.transform.position = new Vector3(spawnPos.transform.position.x, 0.23f, spawnPos.transform.position.z);
                                effect.GetComponent<ParticleSystem>().Play();
                                effect.GetComponent<AudioSource>().Play();
                                break;
                            }
                        }
                        player.transform.position = spawnPos.position;
                        player.SetActive(true);
                        flag++;
                    }
                }
                else
                {
                    flag = 0;
                    break;
                }
            }
            GameManager.currentPlayerAmount += number;
        }

        public static void Subtraction(int number, List<GameObject> Players, List<GameObject> DestoryEffects)
        {
            if (GameManager.currentPlayerAmount < number)
            {
                foreach (var player in Players)
                {
                    foreach (var effect in DestoryEffects)
                    {
                        if (!effect.activeInHierarchy)
                        {
                            Vector3 newPos = new Vector3(player.transform.position.x, 0.3f, player.transform.position.z);
                            effect.SetActive(true);
                            effect.transform.position = newPos;
                            effect.GetComponent<ParticleSystem>().Play();
                            effect.GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                    player.transform.position = Vector3.zero;
                    player.SetActive(false);
                }
                GameManager.currentPlayerAmount = 1;
            }
            else
            {
                int flag = 0;
                foreach (var player in Players)
                {
                    if (flag != number)
                    {
                        if (player.activeInHierarchy)
                        {
                            foreach (var effect in DestoryEffects)
                            {
                                if (!effect.activeInHierarchy)
                                {
                                    Vector3 newPos = new Vector3(player.transform.position.x, 0.3f, player.transform.position.z);
                                    effect.SetActive(true);
                                    effect.transform.position = newPos;
                                    effect.GetComponent<ParticleSystem>().Play();
                                    effect.GetComponent<AudioSource>().Play();
                                    break;
                                }
                            }
                            player.transform.position = Vector3.zero;
                            player.SetActive(false);
                            flag++;
                        }
                    }

                    else
                    {
                        flag = 0;
                        break;
                    }
                }
                GameManager.currentPlayerAmount -= number;
            }
        }

        public static void Divison(int number, List<GameObject> Players, List<GameObject> DestoryEffects)
        {
            if (GameManager.currentPlayerAmount <= number)
            {
                foreach (var player in Players)
                {
                    foreach (var effect in DestoryEffects)
                    {
                        if (!effect.activeInHierarchy)
                        {
                            Vector3 newPos = new Vector3(player.transform.position.x, 0.3f, player.transform.position.z);
                            effect.SetActive(true);
                            effect.transform.position = newPos;
                            effect.GetComponent<ParticleSystem>().Play();
                            effect.GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                    player.transform.position = Vector3.zero;
                    player.SetActive(false);
                    GameManager.currentPlayerAmount = 1;
                }
            }
            else
            {
                int flag = 0;
                int quotient = GameManager.currentPlayerAmount / number;
                foreach (var player in Players)
                {
                    if (flag < quotient)
                    {
                        if (player.activeInHierarchy)
                        {
                            foreach (var effect in DestoryEffects)
                            {
                                if (!effect.activeInHierarchy)
                                {
                                    Vector3 newPos = new Vector3(player.transform.position.x, 0.3f, player.transform.position.z);
                                    effect.SetActive(true);
                                    effect.transform.position = newPos;
                                    effect.GetComponent<ParticleSystem>().Play();
                                    effect.GetComponent<AudioSource>().Play();
                                    break;
                                }
                            }
                            player.transform.position = Vector3.zero;
                            player.SetActive(false);
                            flag++;
                        }
                    }
                    else
                    {
                        flag = 0;
                        break;
                    }
                }

                if (GameManager.currentPlayerAmount % number == 0)
                {
                    GameManager.currentPlayerAmount /= number;
                }
                else if (GameManager.currentPlayerAmount % number == 1)
                {
                    GameManager.currentPlayerAmount /= number;
                    GameManager.currentPlayerAmount++;
                }
                else if (GameManager.currentPlayerAmount % number == 2)
                {
                    GameManager.currentPlayerAmount /= number;
                    GameManager.currentPlayerAmount += 1;
                }
            }
        }

    }

    public class MemoryManagement
    {
        public void SetStringData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetFloatData(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetIntData(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public string ReadStringData(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public float ReadFloatData(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public int ReadIntData(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public void KeyControl()
        {
            if (!PlayerPrefs.HasKey("CurrentLevel"))
            {
                PlayerPrefs.SetInt("CurrentLevel", 5);
                PlayerPrefs.SetInt("Puan", 0);
                PlayerPrefs.SetInt("CurrentHat", -1);
                PlayerPrefs.SetInt("CurrentStick", -1);
                PlayerPrefs.SetInt("CurrentMaterial", -1);
                PlayerPrefs.SetFloat("MenuVolume", 1);
                PlayerPrefs.SetFloat("MenuVFX", 1);
                PlayerPrefs.SetFloat("GameVolume", 1);
                PlayerPrefs.SetString("Language", "TR");

            }
        }

    }

    [Serializable]
    public class Items
    {
        public int Group_Index;
        public int Item_Index;
        public string Item_Name;
        public int Item_Point;
        public bool isPurchase;
    }

    public class DataManagement
    {
        #region ITEM MANAGEMENT
            List<Items> tempItemList;
        public void SaveData(List<Items> Items)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/ItemDatas.gd");
            bf.Serialize(file, Items);
            file.Close();
        }

        public void SaveDataFirstTime(List<Items> Items, List<MainLanguageObject> MainLanguageObjects) 
        {
            if (!File.Exists(Application.persistentDataPath + "/ItemDatas.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/ItemDatas.gd");
                bf.Serialize(file, Items);
                file.Close();
            }
            if (!File.Exists(Application.persistentDataPath + "/LanguageDatas.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/LanguageDatas.gd");
                bf.Serialize(file, MainLanguageObjects);
                file.Close();
            }
        }
        public void LoadItemData()
        {
            if (File.Exists(Application.persistentDataPath + "/ItemDatas.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/ItemDatas.gd", FileMode.Open);
                tempItemList = (List<Items>)bf.Deserialize(file);
                file.Close();
            }
        }

        public List<Items> ProcessReadingTempItemList()
        {
            return tempItemList;
        }


        #endregion
        
        #region  LANGUAGE MANAGEMENT   
        List<MainLanguageObject> tempLanguageList;

        public void LoadLanguageData()
        {
            if (File.Exists(Application.persistentDataPath + "/LanguageDatas.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/LanguageDatas.gd", FileMode.Open);
                tempLanguageList = (List<MainLanguageObject>)bf.Deserialize(file);
                file.Close();
            }
        }

        public List<MainLanguageObject> ProcessReadingTempLanguageList()
        {
            return tempLanguageList;
        }
    
        #endregion
        
    }

    [Serializable]
    public class MainLanguageObject
    {
        public int index;
        public List<LanguageObject_TR> LanguageObject_TR = new List<LanguageObject_TR>();
        public List<LanguageObject_TR> LanguageObject_EN = new List<LanguageObject_TR>();

    }

    [Serializable]
    public class LanguageObject_TR
    {
        public string Text;
    }

}

