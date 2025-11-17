using UnityEngine;

[CreateAssetMenu(menuName = "Game/Recipe")]
public class RecipeSO : ScriptableObject {
    public string recipeID;
    public string displayName;
    public Sprite resultIcon;
    public GameObject resultPrefab; // final food prefab to spawn in AR
    public IngredientSO[] requiredIngredients; // order not important
    [TextArea] public string originInfo;
    [TextArea] public string funFacts;
}
