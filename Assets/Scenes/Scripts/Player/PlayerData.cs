using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float currHealth;
    public float maxHealth;
    public float strength;
    public float speed;
    public int money;
    public int commonKeys;
    public int uncommonKeys;
    public int bossKeys;
    public int currentAmmo;
    public int maxAmmo;
    public float savedTimerDuration;
    public float[] position;
    public string sceneName; // Menyimpan nama scene
    public List<int> savedEnemiesID = new List<int>();
    public List<int> savedChestsID = new List<int>();
    public List<int> savedPotsID = new List<int>();

    public bool canEnterHouse3;
    public bool canEnterHouse4;

    public int currentCheckpointIndex;
    public int currentMissionIndex;
    public Vector2 newPlayerPosition;

    public bool hasBoughtArrowForest = false;
    public bool hasBoughtHealthForest = false;
    public bool hasBoughtPotionArrowForest = false;
    public bool hasBoughtArrowIsland = false;
    public bool hasBoughtHealthIsland = false;
    public bool hasBoughtPotionArrowIsland = false;
    public bool hasBoughtPotionHealthIsland = false;

    public bool hasSeenOpeningDialogue = false;

    public PlayerData(Player player)
    {
        currHealth = player.currHealth;
        maxHealth = player.maxHealth;
        strength = player.strength;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            speed = playerMovement.Speed;
        }

        if(Timer.GetInstance() != null) savedTimerDuration = Timer.GetInstance().CountdownTime;
        money = player.GetComponent<Inventory>().money;
        commonKeys = player.GetComponent<Inventory>().commonKeys;
        uncommonKeys = player.GetComponent<Inventory>().uncommonKeys;
        bossKeys = player.GetComponent<Inventory>().bossKeys;
        currentAmmo = player.GetComponent<Inventory>().currentAmmo;
        maxAmmo = player.GetComponent<Inventory>().maxAmmo;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        foreach (var enemyID in InfoManager.Instance.EnemiesID) 
        {
            savedEnemiesID.Add(enemyID);
        }

        foreach (var chestID in InfoManager.Instance.ChestsID)
        {
            savedChestsID.Add(chestID);
        }

        foreach (var potID in InfoManager.Instance.PotsID)
        {
            savedPotsID.Add(potID);
        }

        canEnterHouse3 = InfoManager.Instance.canEnterHouse3;
        canEnterHouse4 = InfoManager.Instance.canEnterHouse4;

        currentCheckpointIndex = InfoManager.Instance.currentCheckpointIndex;
        currentMissionIndex = InfoManager.Instance.currentMissionIndex;
        newPlayerPosition = InfoManager.Instance.NewPlayerPosition;

        hasBoughtArrowForest = InfoManager.Instance.hasBoughtArrowForest;
        hasBoughtHealthForest = InfoManager.Instance.hasBoughtHealthForest;
        hasBoughtPotionArrowForest = InfoManager.Instance.hasBoughtPotionArrowForest;
        hasBoughtArrowIsland = InfoManager.Instance.hasBoughtArrowIsland;
        hasBoughtHealthIsland = InfoManager.Instance.hasBoughtHealthIsland;
        hasBoughtPotionArrowIsland = InfoManager.Instance.hasBoughtPotionArrowIsland;
        hasBoughtPotionHealthIsland = InfoManager.Instance.hasBoughtPotionHealthIsland;

        hasSeenOpeningDialogue = InfoManager.Instance.hasSeenOpeningDialogue;

        // Menyimpan nama scene
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
}
