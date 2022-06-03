using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveFile 
{
    //constuctor for save file
    public int playerHealth;
    public int maxJumps;
    public bool upgradeChargeJump;
    public bool upgradeWallGrab;
    public bool upgradeDoubleJump;

    //public Vector3 playerPosition;
    public int currentScene = 0;

    public PlayerSaveFile(SaveFile playerSave)
    {
        playerHealth = playerSave.health;
        maxJumps = playerSave.maxJumps;
        upgradeChargeJump = playerSave.upgradeChargeJump;
        upgradeWallGrab = playerSave.upgradeWallGrab;
        upgradeDoubleJump = playerSave.upgradeDoubleJump;

        //playerPosition = playerSave.playerPosition;
        currentScene = playerSave.currentScene;

    }
    
}
