using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static GameObject instance;
    AudioSource menuMusic;
    void Start()
    {
        menuMusic = GetComponent<AudioSource>();
        menuMusic.volume = PlayerPrefs.GetFloat("MenuVolume");
        DontDestroyOnLoad(menuMusic);

        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        menuMusic.volume = PlayerPrefs.GetFloat("MenuVolume");
        
    }
}
