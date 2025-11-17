// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;

// public class InventoryManager : MonoBehaviour {
//     public static InventoryManager I { get; private set; }

//     // store counts (or true/false if you only want single collect)
//     private Dictionary<string, int> items = new Dictionary<string, int>();

//     void Awake() {
//         if (I == null) { I = this; DontDestroyOnLoad(gameObject); }
//         else Destroy(gameObject);
//     }

//     public void AddIngredient(IngredientSO ing, int amount = 1) {
//         if (ing == null) return;
//         if (!items.ContainsKey(ing.ingredientID)) items[ing.ingredientID] = 0;
//         items[ing.ingredientID] += amount;
//         // notify UI
//         CraftingUIController.I?.OnInventoryChanged();
//     }

//     public bool HasIngredient(IngredientSO ing, int amount = 1) {
//         if (ing == null) return false;
//         return items.ContainsKey(ing.ingredientID) && items[ing.ingredientID] >= amount;
//     }

//     public bool HasAll(RecipeSO recipe) {
//         if (recipe == null) return false;
//         return recipe.requiredIngredients.All(ing => HasIngredient(ing));
//     }

//     public void ConsumeForRecipe(RecipeSO recipe) {
//         if (recipe == null) return;
//         foreach (var ing in recipe.requiredIngredients) {
//             if (items.ContainsKey(ing.ingredientID)) {
//                 items[ing.ingredientID] = Mathf.Max(0, items[ing.ingredientID] - 1);
//             }
//         }
//         CraftingUIController.I?.OnInventoryChanged();
//     }

//     // expose for UI debug / listing
//     public Dictionary<string,int> GetSnapshot() {
//         return new Dictionary<string,int>(items);
//     }
// }
