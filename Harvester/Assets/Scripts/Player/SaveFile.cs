using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    
    //This is the Save file instance
    public int health = 3;
    public int maxJumps = 1;
    public bool upgradeWallGrab = false;
    public bool upgradeChargeJump = false;
    public bool upgradeDoubleJump = false;

    //public Vector3 playerPosition = new Vector3(0, 0, 0);
    public int currentScene = 2;
    public static SaveFile instance = null;
    //private static bool created = false;

    public static SaveFile Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of the player!");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SceneLoader.Instance.LoadScene(currentScene);
        LoadPlayer();
    }

    public void SavePlayer() //call this to save data
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer() // call this to load data
    {
        PlayerSaveFile playerData = SaveSystem.loadPlayerCyn();

        health = playerData.playerHealth;
        maxJumps = playerData.maxJumps;
        upgradeChargeJump = playerData.upgradeChargeJump;
        upgradeWallGrab = playerData.upgradeWallGrab;
        upgradeDoubleJump = playerData.upgradeDoubleJump;

        //playerPosition = playerData.playerPosition;
        currentScene = playerData.currentScene;


    }
    
}
