using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI Slots")]
    public Image[] slots;             // Slot images
    public TextMeshProUGUI[] counts;  // TMP counters next to each slot

    [Header("UI Counter")]
    public TextMeshProUGUI ingredientCounterText;

    [Header("Ingredient Count")]
    public int ingredientsCollected = 0;
    public int totalIngredients = 8;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToSlot(Sprite sprite)
    {
        // 1️⃣ First check if this ingredient already exists in inventory
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite == sprite)
            {
                // increase count
                int currentAmount = int.Parse(counts[i].text.Replace("x", ""));
                currentAmount++;
                counts[i].text = "x" + currentAmount;

                ingredientsCollected++;
                UpdateCounterUI();

                Debug.Log($"Updated existing item count in slot {i}");
                return;
            }
        }

        // 2️⃣ If it doesn't exist, find an empty slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite == null)
            {
                slots[i].sprite = sprite;
                counts[i].text = "x1";

                ingredientsCollected++;
                UpdateCounterUI();

                Debug.Log($"Added NEW item to slot {i}");
                return;
            }
        }

        Debug.LogWarning("No empty inventory slots available!");
    }

    private void UpdateCounterUI()
    {
        ingredientCounterText.text = $"Ingredients: {ingredientsCollected}/{totalIngredients}";
    }
}
