using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;


public class InfoManager : MonoBehaviour
{
    static InfoManager instance;
    public static InfoManager Instance;

    public Vector2 NewPlayerPosition { get; set; }
    public float PlayerMaxHealth { get; set; }
    public float PlayerHealth { get; set; }
    public float PlayerMagic { get; set; }
    public float Strength { get; set; }
    public string SceneName { get; set; }
    public int AmmoLeft { get; set; }
    public int CommonKeys { get; set; }
    public int UnCommonKeys { get; set; }
    public int BossKeys { get; set; }
    public int Money { get; set; }
    public List<Item> items;

    public List<int> EnemiesID;
    public List<int> ChestsID;
    public List<int> PotsID;

    public Dictionary<string, bool> chests = new Dictionary<string, bool>();
    public Dictionary<string, bool> buttons = new Dictionary<string, bool>();
    public Dictionary<string, DoorType> doors = new Dictionary<string, DoorType>();

    [Header("House Access Bool")]
    public bool canEnterHouse3 = false;
    public bool canEnterHouse4 = false;

    [Header("Current Checkpoint Index")]
    public int currentCheckpointIndex;

    [Header("Previous Room Check")]
    public bool isFromHouseInterior = false;
    public bool isFromForestToIsland = false;
    public bool isFromHouseHouse1 = false;
    public bool isFromHouseHouse2 = false;
    public bool isFromHouseHouse3 = false;
    public bool isFromHouseHouse4 = false;

    [Header("Shop Check")]
    public bool hasBoughtArrowForest = false;
    public bool hasBoughtHealthForest = false;
    public bool hasBoughtPotionArrowForest = false;
    public bool hasBoughtArrowIsland = false;
    public bool hasBoughtHealthIsland = false;
    public bool hasBoughtPotionArrowIsland = false;
    public bool hasBoughtPotionHealthIsland = false;

    [Header("Current Mission Index")]
    public int currentMissionIndex;

    [Header("Dialogue Flag")]
    public bool hasSeenOpeningDialogue = false;

    // Define scenes that require the "Transition" object
    private string[] scenesWithTransition = { "MainMenu", "Main", "HouseInterior", "Cave", "CreditScene", };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += InitiateLevel;

        //147 is the total enemies in the game
        for (int i = 0; i < 111; i++)
        {
            EnemiesID.Add(i);
        }

        //9 Is the total of chests in this game
        for (int i = 0; i < 11; i++)
        {
            ChestsID.Add(i);
        }

        for (int i = 0; i < 24; i++)
        {
            PotsID.Add(i);
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            Player.instance.SavePlayer();
        }
    }

    private void Update()
    {
        ForestLevelEnemiesCheck();
    }

    private void ForestLevelEnemiesCheck()
    {
        if (currentMissionIndex != 1) return;
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (EnemiesID[0] >= 21) SetMissionIndex(2);
        }
    }

    public void NewListAfterNewGame()
    {
        EnemiesID.Clear();
        ChestsID.Clear();
        PotsID.Clear();
        for (int i = 0; i < 111; i++)
        {
            EnemiesID.Add(i);
        }

        for (int i = 0; i < 11; i++)
        {
            ChestsID.Add(i);
        }

        for (int i = 0; i < 24; i++)
        {
            PotsID.Add(i);
        }
    }

    public void OverwriteEnemiesID(List<int> newEnemiesListID)
    {
        EnemiesID.Clear();
        foreach (int id in newEnemiesListID)
        {
            EnemiesID.Add(id);
        }
    }

    public void OverwriteChestsID(List<int> newChestsListID)
    {
        ChestsID.Clear();
        foreach (int id in newChestsListID)
        {
            ChestsID.Add(id);
        }
    }

    public void OverwritePotsID(List<int> newPotsListID)
    {
        PotsID.Clear();
        foreach (int id in newPotsListID)
        {
            PotsID.Add(id);
        }
    }

    public void RemoveEnemiesID(int value)
    {
        EnemiesID.Remove(value);
    }

    public void RemoveChestID(int value)
    {
        ChestsID.Remove(value);
    }

    public void RemovePotID(int value)
    {
        PotsID.Remove(value);
    }

    public void SetMissionIndex(int index)
    {
        currentMissionIndex = index;
        MissionDisplay.GetInstance().SetMissionIndex(currentMissionIndex);
    }

    public void InitiateLevel(Scene scene, LoadSceneMode mode)
    {
        //GameObject.FindWithTag("Transition").GetComponent<UnityEngine.UI.Image>().enabled = true;

        Debug.Log("Initiating level for scene: " + scene.name);

        bool sceneRequiresTransition = Array.Exists(scenesWithTransition, s => s == scene.name);

        if (sceneRequiresTransition)
        {
            Time.timeScale = 1;
            
            // Try to find the GameObject with the tag "Transition"
            GameObject transitionObject = GameObject.FindWithTag("Transition");

            if (transitionObject != null)
            {
                // Try to get the UnityEngine.UI.Image component
                UnityEngine.UI.Image transitionImage = transitionObject.GetComponent<UnityEngine.UI.Image>();

                if (transitionImage != null)
                {
                    // Enable the image component
                    transitionImage.enabled = true;
                }
                else
                {
                    Debug.LogError("Image component not found on the object with tag 'Transition'.");
                }
            }
            else
            {
                // Optionally log a message instead of an error
                Debug.LogWarning("Object with tag 'Transition' not found in scene: " + scene.name);
            }
        }
        

    }

    public void UpdateStats()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Inventory playerInventory = player.GetComponent<Inventory>();
        PlayerMaxHealth = player.maxHealth;
        PlayerHealth = player.currHealth;
        items = playerInventory.MyItems;
        CommonKeys = playerInventory.commonKeys;
        UnCommonKeys = playerInventory.uncommonKeys;
        BossKeys = playerInventory.bossKeys;
        Money = playerInventory.money;
        AmmoLeft = playerInventory.currentAmmo;
    }

    public void NewGameStats(bool isRetry = false)
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Inventory playerInventory = player.GetComponent<Inventory>();

        // Setel semua atribut player ke nilai awal
        player.maxHealth = 6f;
        player.currHealth = 6f;
        player.strength = 1f;

        // Reset atribut lain di InfoManager ke nilai awal
        PlayerMaxHealth = player.maxHealth;
        PlayerHealth = player.currHealth;

        // Reset atribut inventori ke nilai awal
        playerInventory.MyItems.Clear();  // Mengosongkan item
        playerInventory.commonKeys = 0;
        playerInventory.uncommonKeys = 0;
        playerInventory.bossKeys = 0;
        playerInventory.money = 0;
        playerInventory.currentAmmo = 0;

        // Assign nilai-nilai yang direset ke atribut InfoManager
        items = playerInventory.MyItems;
        CommonKeys = playerInventory.commonKeys;
        UnCommonKeys = playerInventory.uncommonKeys;
        BossKeys = playerInventory.bossKeys;
        Money = playerInventory.money;
        AmmoLeft = playerInventory.currentAmmo;
        if(!isRetry) NewPlayerPosition = Vector3.zero;

        canEnterHouse3 = false;
        canEnterHouse4 = false;

        if(!isRetry)
        {
            hasBoughtArrowForest = false;
            hasBoughtHealthForest = false;
            hasBoughtPotionArrowForest = false;
            hasBoughtArrowIsland = false;
            hasBoughtHealthIsland = false;
            hasBoughtPotionArrowIsland = false;
            hasBoughtPotionHealthIsland = false;

            hasSeenOpeningDialogue = false;

            currentCheckpointIndex = 0;
            currentMissionIndex = 0;
            player.SetHasSaveDataValue(false);
        }

    }

    public void LoadStats()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            PlayerMaxHealth = data.maxHealth;
            //PlayerHealth = data.currHealth;
            PlayerHealth = PlayerMaxHealth;
            Strength = data.strength;
            AmmoLeft = data.currentAmmo;
            CommonKeys = data.commonKeys;
            UnCommonKeys = data.uncommonKeys;
            BossKeys = data.bossKeys;
            Money = data.money;

            EnemiesID.Clear();
            ChestsID.Clear();
            PotsID.Clear();

            foreach (var enemyID in data.savedEnemiesID)
            {
                EnemiesID.Add(enemyID);
            }

            foreach (var chestID in data.savedChestsID)
            {
                ChestsID.Add(chestID);
            }

            foreach (var potsID in data.savedPotsID)
            {
                PotsID.Add(potsID);
            }

            canEnterHouse3 = data.canEnterHouse3;
            canEnterHouse4 = data.canEnterHouse4;

            hasBoughtArrowForest = data.hasBoughtArrowForest;
            hasBoughtHealthForest = data.hasBoughtHealthForest;
            hasBoughtPotionArrowForest = data.hasBoughtPotionArrowForest;
            hasBoughtArrowIsland = data.hasBoughtArrowIsland;
            hasBoughtHealthIsland = data.hasBoughtHealthIsland;
            hasBoughtPotionArrowIsland = data.hasBoughtPotionArrowIsland;
            hasBoughtPotionHealthIsland = data.hasBoughtPotionHealthIsland;

            currentCheckpointIndex = data.currentCheckpointIndex;
            currentMissionIndex = data.currentMissionIndex;
            NewPlayerPosition = data.newPlayerPosition;

            hasSeenOpeningDialogue = data.hasSeenOpeningDialogue;


            SaveManager.GetInstance().UpdateSavedDuration((int)data.savedTimerDuration);

            // Setel posisi player
            Vector3 position = new Vector3(data.position[0], data.position[1]);
            // Anda perlu mencari objek Player di dalam scene dan mengatur posisinya
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null)
            {
                player.transform.position = position;
            }
            else
            {
                Debug.LogError("Player object not found. Make sure the player object has the 'Player' tag.");
            }

            SceneName = data.sceneName;

            if (!string.IsNullOrEmpty(data.sceneName) && data.sceneName != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene(data.sceneName);
                
            }

            //buat object - object seperti chest switch door
            //Chest[] chestsArray = FindObjectsOfType<Chest>();
            //foreach (Chest currentChest in chestsArray)
            //{
            //    chests[currentChest.name] = currentChest.isOpen;
            //}
            UIManager.instance.UpdateAllUI();

        }
    }


}
