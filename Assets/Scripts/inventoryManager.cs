using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    [Header("UI Slots")]
    public Image[] slots;  // the white boxes in inventory UI

    [Header("UI Counter")]
    public TextMeshProUGUI ingredientCounterText;

    [Header("Ingredient Count")]
    public int ingredientsCollected = 0;
    public int totalIngredients = 8;  // Change if needed

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

    public void AddItem(string itemName, Sprite sprite)
    {
        // Fill next empty slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite == null)
            {
                slots[i].sprite = sprite;
                slots[i].color = Color.white;
                break;
            }
        }

        // Update total count
        ingredientsCollected++;
        UpdateCounterUI();

        Debug.Log($"Added {itemName} to inventory.");
    }

    private void UpdateCounterUI()
    {
        ingredientCounterText.text = $"Ingredients: {ingredientsCollected}/{totalIngredients}";
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
