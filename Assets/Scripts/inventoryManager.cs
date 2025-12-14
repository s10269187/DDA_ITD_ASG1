/// <summary>
/// InventoryManager.cs
/// This script manages the player's inventory system,
/// allowing addition, removal, and tracking of ingredients and crafted snacks.
/// spawning of crafted snacks in AR space is also handled here. The game
/// timer functionality is integrated to stop when all snacks are crafted.
/// </summary>
/// <author> Leong Ming Hui </author>
/// <date> 8/12/2025 </date>
/// <StudentID> S10267664J </StudentID>
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class InventoryManager : MonoBehaviour
{
    /// Singleton instance for global access
    public static InventoryManager Instance;

    [Header("UI Slots")]    // inventory slots for sprites and counts 
    public Image[] slots;     
    public TextMeshProUGUI[] counts;    // text to show count of each item

    [Header("UI Counter")]  // for ingredients and snacks crafted
    public TextMeshProUGUI ingredientCounterText;
    public TextMeshProUGUI snacksCraftedText;

    [Header("Ingredient Count")]    // to track ingredients collected
    public int ingredientsCollected = 0;
    public int totalIngredients = 8;

    [Header("Snacks Crafted")]  // to track snacks crafted
    public int snacksCrafted = 0;
    public int totalSnacks = 3;

    [Header("Ingredient Sprites")]  // sprites for ingredients
    public Sprite flourSprite;
    public Sprite sugarSprite;
    public Sprite eggSprite;
    public Sprite waterSprite;
    public Sprite hawthornSprite;

    [Header("Crafted Item Sprites")]    // sprites for crafted snacks
    public Sprite biscuitSprite;
    public Sprite bangkitSprite;
    public Sprite hawFlakesSprite;

    [Header("Timer")]
    public Timer gameTimer; // reference to the Timer script

    private DatabaseReference db;   // Firebase database reference

    public Transform arCamera;   // Reference to the AR camera for spawning snacks       
    public GameObject biscuitPrefab;    
    public GameObject bangkitPrefab;
    public GameObject hawFlakesPrefab;

    /// ------------------- SPAWN SNACK -------------------
    /// Spawns the crafted snack prefab in front of the AR camera
    void SpawnSnack(GameObject prefab)
    {
        Vector3 spawnPos = arCamera.position + arCamera.forward * 0.5f - new Vector3(0, 0.1f, 0);  // 0.5m in front of camera, slightly down
        Instantiate(prefab, spawnPos, Quaternion.LookRotation(arCamera.forward));   // face the camera
    }


    /// Initializes the singleton instance and Firebase database reference
    private void Awake()
    {
        if (Instance == null)   // first instance
            Instance = this;    // allow static access

        // Initialize Firebase reference
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }


    /// ------------------- ADD ITEMS -------------------
    ///
    /// Adds an item to the inventory slots
    /// If the item already exists, increments its count
    /// 
    public void AddItemToSlot(Sprite sprite, bool countAsIngredient = true) // default true for ingredients
    {
        for (int i = 0; i < slots.Length; i++)  // check if item already exists
        {
            if (slots[i] != null && slots[i].sprite == sprite)  // item exists
            {
                int currentAmount = GetItemCount(i);    // get current count
                currentAmount++;
                counts[i].text = "x" + currentAmount;   // update count text

                if (countAsIngredient)  // only count if it's an ingredient
                {
                    ingredientsCollected++; // increment ingredient count
                    UpdateCounterUI();
                }
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)  // find empty slot
        {
            if (slots[i] != null && slots[i].sprite == null)    // empty slot
            {
                slots[i].sprite = sprite;   // add sprite
                counts[i].text = "x1";      // set count to 1

                if (countAsIngredient)
                {
                    ingredientsCollected++; 
                    UpdateCounterUI();
                }
                return;
            }
        }
    }


    /// Gets the current count of the item in the specified slot index
    /// Returns 0 if the slot is empty or count text is invalid
    private int GetItemCount(int index)
    {
        if (counts[index] == null) return 0;    // no count text
        string c = counts[index].text.Replace("x", "").Trim();  // remove 'x' prefix
        if (string.IsNullOrEmpty(c)) return 0;  // empty count
        if (!int.TryParse(c, out int value)) return 0;  // invalid number
        return value;   // return parsed count
    }

    /// Updates the UI text for ingredients collected and snacks crafted
    private void UpdateCounterUI()
    {
        ingredientCounterText.text = $"Ingredients: {ingredientsCollected}/{totalIngredients}";
        snacksCraftedText.text = $"Snacks Crafted: {snacksCrafted}/{totalSnacks}";
    }

    /// ------------------- HELPER CHECK -------------------
    ///
    /// Checks if the inventory has the required amount of the specified ingredient
    /// 
    public bool HasIngredients(Sprite ingredient, int required)
    {
        for (int i = 0; i < slots.Length; i++)  // search slots
        {
            if (slots[i] != null && slots[i].sprite != null && slots[i].sprite == ingredient)   // found ingredient
            {
                int currentAmount = GetItemCount(i);
                return currentAmount >= required;   // check if enough
            }
        }
        return false;   // ingredient not found
    }

    /// ------------------- DEDUCT INGREDIENTS -------------------
    /// 
    /// Deducts the specified amount of the ingredient from the inventory
    /// 
    public void DeductIngredient(Sprite ingredient, int amount)
    {
        for (int i = 0; i < slots.Length; i++)  
        {
            if (slots[i] != null && slots[i].sprite != null && slots[i].sprite == ingredient)   
            {
                int currentAmount = GetItemCount(i);
                currentAmount -= amount;    // deduct amount

                if (currentAmount <= 0)     // remove item if count is zero or less
                {
                    slots[i].sprite = null;  // clear sprite
                    counts[i].text = "";     // clear count text
                }
                else
                {
                    counts[i].text = "x" + currentAmount;   // update count text
                }

                CleanInventory();   // clean up inventory to remove gaps
                return;
            }
        }
    }


    /// ------------------- CLEAN INVENTORY -------------------
    ///
    /// Cleans the inventory by removing gaps between items
    /// after items have been deducted from the inventory after crafting
    /// 
    private void CleanInventory()
    {
        int nextFreeIndex = 0;  // next free slot index
        Sprite[] currentSprites = new Sprite[slots.Length]; // to hold current sprites
        string[] currentCounts = new string[slots.Length];  // to hold current counts

        for (int i = 0; i < slots.Length; i++)  // gather existing items
        {
            if (slots[i] != null && slots[i].sprite != null && !string.IsNullOrEmpty(counts[i].text) && counts[i].text != "x0") // valid item
            {
                currentSprites[nextFreeIndex] = slots[i].sprite;    // store sprite
                currentCounts[nextFreeIndex] = counts[i].text;      // store count
                nextFreeIndex++;    
            }
        }

        for (int i = 0; i < slots.Length; i++)  // refill slots without gaps
        {
            if (i < nextFreeIndex)  // has item
            {
                slots[i].sprite = currentSprites[i];    // set sprite        
                counts[i].text = currentCounts[i];      // set count
            }
            else    // empty slot
            {
                slots[i].sprite = null;  // clear sprite
                counts[i].text = "";    // clear count text
            }
        }
    }

    /// ------------------- RECIPE CHECKS -------------------
    /// 
    /// Checks if the player has enough ingredients to craft each snack
    ///
    public bool CanCraftBiscuit() => HasIngredients(flourSprite, 1) && HasIngredients(sugarSprite, 1);
    public bool CanCraftBangkit() => HasIngredients(flourSprite, 1) && HasIngredients(eggSprite, 1) && HasIngredients(sugarSprite, 1);
    public bool CanCraftHawFlakes() => HasIngredients(waterSprite, 1) && HasIngredients(hawthornSprite, 1) && HasIngredients(sugarSprite, 1);

    /// ------------------- CRAFTING -------------------
    /// 
    /// Crafts the specified snack if enough ingredients are available
    /// Deducts ingredients, adds crafted snack to inventory, spawns snack in AR space
    /// 
    public void CraftBiscuit()
    {
        if (!CanCraftBiscuit())
        {
            Debug.Log(" Not enough ingredients for Biscuit Gem");
            return;
        }

        DeductIngredient(flourSprite, 1);   
        DeductIngredient(sugarSprite, 1);
        AddItemToSlot(biscuitSprite, false);
        SpawnSnack(biscuitPrefab);

        snacksCrafted++;
        UpdateCounterUI();

        Debug.Log(" Crafted Biscuit Gem!");
        CheckAllCrafted();
    }

    public void CraftBangkit()
    {
        if (!CanCraftBangkit())
        {
            Debug.Log(" Not enough ingredients for Bangkit");
            return;
        }

        DeductIngredient(flourSprite, 1);
        DeductIngredient(eggSprite, 1);
        DeductIngredient(sugarSprite, 1);
        AddItemToSlot(bangkitSprite, false);
        SpawnSnack(bangkitPrefab);

        snacksCrafted++;
        UpdateCounterUI();

        Debug.Log(" Crafted Bangkit!");
        CheckAllCrafted();
    }

    public void CraftHawFlakes()
    {
        if (!CanCraftHawFlakes())
        {
            Debug.Log(" Not enough ingredients for Haw Flakes");
            return;
        }

        DeductIngredient(waterSprite, 1);
        DeductIngredient(hawthornSprite, 1);
        DeductIngredient(sugarSprite, 1);
        AddItemToSlot(hawFlakesSprite, false);
        SpawnSnack(hawFlakesPrefab);

        snacksCrafted++;
        UpdateCounterUI();

        Debug.Log(" Crafted Haw Flakes!");
        CheckAllCrafted();
    }

    /// ------------------- CHECK ALL CRAFTED -------------------
    /// 
    /// Checks if all snacks have been crafted
    /// Stops the game timer and records player data to Firebase
    /// 
    private void CheckAllCrafted()
    {
        bool hasBiscuit = false;
        bool hasBangkit = false;
        bool hasHawFlakes = false;

        for (int i = 0; i < slots.Length; i++)  // check inventory for crafted snacks
        {
            if (slots[i] != null && slots[i].sprite != null)    // valid slot
            {
                if (slots[i].sprite == biscuitSprite) hasBiscuit = true;    // check for each snack
                if (slots[i].sprite == bangkitSprite) hasBangkit = true;
                if (slots[i].sprite == hawFlakesSprite) hasHawFlakes = true;
            }
        }

        if (hasBiscuit && hasBangkit && hasHawFlakes)   // all crafted
        {
            if (gameTimer != null)  
                gameTimer.StopTimer();

            Debug.Log(" All items crafted! Timer stopped.");

            float elapsedTime = gameTimer != null ? gameTimer.GetElapsedTime() : 0f;    
            var player = new Player();
            player.id = "S1026";
            player.name = "Lee Jia Lu";
            player.time = elapsedTime;

            var db = FirebaseDatabase.DefaultInstance.RootReference;    // get database reference

            string json = JsonUtility.ToJson(player);
            db.Child("players").Child(player.id).SetRawJsonValueAsync(json).ContinueWith(task =>    // write player data
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player created successfully!");
                }
            });
        }
    }

    /// ------------------- DELETE PLAYER DATA -------------------
    ///
    /// Deletes the player data from Firebase database
    ///
    public void DeletePlayer()
    {
        var db = FirebaseDatabase.DefaultInstance.RootReference;
        var player = new Player(); 
        player.id = "S1026";

        db.Child("players").Child(player.id).RemoveValueAsync().ContinueWith(task =>    // delete player data
        {
            if (task.IsCompleted)
                Debug.Log("Player deleted successfully!");
        });
    }
}

