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

    public void AddItemToSlot(int slotIndex, Sprite sprite)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError("Invalid slot index!");
            return;
        }

        slots[slotIndex].sprite = sprite;

        ingredientsCollected++;
        slotIndex++;

        Debug.Log($"Added item to slot {slotIndex}");
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
