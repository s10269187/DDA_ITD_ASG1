using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI Slots")]
    public Image[] slots;
    public TextMeshProUGUI[] counts;

    [Header("UI Counter")]
    public TextMeshProUGUI ingredientCounterText;

    [Header("Ingredient Count")]
    public int ingredientsCollected = 0;
    public int totalIngredients = 8;

    [Header("Ingredient Sprites")]
    public Sprite flourSprite;
    public Sprite sugarSprite;
    public Sprite eggSprite;
    public Sprite waterSprite;
    public Sprite hawthornSprite;

    [Header("Crafted Item Sprites")]
    public Sprite biscuitSprite;
    public Sprite bangkitSprite;
    public Sprite hawFlakesSprite;

    [Header("Timer")]
    public Timer gameTimer; // Drag your Timer object here in Inspector

    private DatabaseReference db;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        // Initialize Firebase reference
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // ------------------- ADD ITEMS -------------------
    public void AddItemToSlot(Sprite sprite, bool countAsIngredient = true)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].sprite == sprite)
            {
                int currentAmount = GetItemCount(i);
                currentAmount++;
                counts[i].text = "x" + currentAmount;

                if (countAsIngredient)
                {
                    ingredientsCollected++;
                    UpdateCounterUI();
                }
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].sprite == null)
            {
                slots[i].sprite = sprite;
                counts[i].text = "x1";

                if (countAsIngredient)
                {
                    ingredientsCollected++;
                    UpdateCounterUI();
                }
                return;
            }
        }
    }

    private int GetItemCount(int index)
    {
        if (counts[index] == null) return 0;
        string c = counts[index].text.Replace("x", "").Trim();
        if (string.IsNullOrEmpty(c)) return 0;
        if (!int.TryParse(c, out int value)) return 0;
        return value;
    }

    private void UpdateCounterUI()
    {
        ingredientCounterText.text = $"Ingredients: {ingredientsCollected}/{totalIngredients}";
    }

    // ------------------- HELPER CHECK -------------------
    public bool HasIngredients(Sprite ingredient, int required)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].sprite != null && slots[i].sprite == ingredient)
            {
                int currentAmount = GetItemCount(i);
                return currentAmount >= required;
            }
        }
        return false;
    }

    public void DeductIngredient(Sprite ingredient, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].sprite != null && slots[i].sprite == ingredient)
            {
                int currentAmount = GetItemCount(i);
                currentAmount -= amount;

                if (currentAmount <= 0)
                {
                    slots[i].sprite = null;
                    counts[i].text = "";
                }
                else
                {
                    counts[i].text = "x" + currentAmount;
                }

                CleanInventory();
                return;
            }
        }
    }

    // ------------------- CLEAN INVENTORY -------------------
    private void CleanInventory()
    {
        int nextFreeIndex = 0;
        Sprite[] currentSprites = new Sprite[slots.Length];
        string[] currentCounts = new string[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].sprite != null && !string.IsNullOrEmpty(counts[i].text) && counts[i].text != "x0")
            {
                currentSprites[nextFreeIndex] = slots[i].sprite;
                currentCounts[nextFreeIndex] = counts[i].text;
                nextFreeIndex++;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < nextFreeIndex)
            {
                slots[i].sprite = currentSprites[i];
                counts[i].text = currentCounts[i];
            }
            else
            {
                slots[i].sprite = null;
                counts[i].text = "";
            }
        }
    }

    // ------------------- RECIPE CHECKS -------------------
    public bool CanCraftBiscuit() => HasIngredients(flourSprite, 1) && HasIngredients(sugarSprite, 1);
    public bool CanCraftBangkit() => HasIngredients(flourSprite, 1) && HasIngredients(eggSprite, 1) && HasIngredients(sugarSprite, 1);
    public bool CanCraftHawFlakes() => HasIngredients(waterSprite, 1) && HasIngredients(hawthornSprite, 1) && HasIngredients(sugarSprite, 1);

    // ------------------- CRAFTING -------------------
    public void CraftBiscuit()
    {
        if (!CanCraftBiscuit())
        {
            Debug.Log("❌ Not enough ingredients for Biscuit Gem");
            return;
        }

        DeductIngredient(flourSprite, 1);
        DeductIngredient(sugarSprite, 1);
        AddItemToSlot(biscuitSprite, false);
        Debug.Log("🍪 Crafted Biscuit Gem!");
        CheckAllCrafted();
    }

    public void CraftBangkit()
    {
        if (!CanCraftBangkit())
        {
            Debug.Log("❌ Not enough ingredients for Bangkit");
            return;
        }

        DeductIngredient(flourSprite, 1);
        DeductIngredient(eggSprite, 1);
        DeductIngredient(sugarSprite, 1);
        AddItemToSlot(bangkitSprite, false);
        Debug.Log("🍘 Crafted Bangkit!");
        CheckAllCrafted();
    }

    public void CraftHawFlakes()
    {
        if (!CanCraftHawFlakes())
        {
            Debug.Log("❌ Not enough ingredients for Haw Flakes");
            return;
        }

        DeductIngredient(waterSprite, 1);
        DeductIngredient(hawthornSprite, 1);
        DeductIngredient(sugarSprite, 1);
        AddItemToSlot(hawFlakesSprite, false);
        Debug.Log("🍬 Crafted Haw Flakes!");
        CheckAllCrafted();
    }

    // ------------------- CHECK ALL CRAFTED -------------------
    private void CheckAllCrafted()
    {
        bool hasBiscuit = false;
        bool hasBangkit = false;
        bool hasHawFlakes = false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].sprite != null)
            {
                if (slots[i].sprite == biscuitSprite) hasBiscuit = true;
                if (slots[i].sprite == bangkitSprite) hasBangkit = true;
                if (slots[i].sprite == hawFlakesSprite) hasHawFlakes = true;
            }
        }

        if (hasBiscuit && hasBangkit && hasHawFlakes)
        {
            if (gameTimer != null)
                gameTimer.StopTimer();

            Debug.Log("⏱ All items crafted! Timer stopped.");

            float elapsedTime = gameTimer != null ? gameTimer.GetElapsedTime() : 0f;
            var player = new Player();
            player.id = "S1026";
            player.name = "Lee Jia Lu";
            player.time = elapsedTime;

            var db = FirebaseDatabase.DefaultInstance.RootReference;

            string json = JsonUtility.ToJson(player);
            db.Child("players").Child(player.id).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("Player created successfully!");
            });
        }
    }
}

