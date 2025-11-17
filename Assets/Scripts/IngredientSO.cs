using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ingredient")]
public class IngredientSO : ScriptableObject {
    public string ingredientID; // unique id, e.g. "coconut_milk"
    public string displayName;
    public Sprite icon;
    [TextArea] public string description;
    public GameObject worldPrefab; // AR pickup prefab (with collider)
}
