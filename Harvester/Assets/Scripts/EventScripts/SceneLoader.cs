using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of the SceneLoader!");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        
    }

    public void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }

    public void UnloadScene(int sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
