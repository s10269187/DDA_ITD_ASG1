/// <summary>
/// Respawn.cs
/// This script handles the respawning time of prefabs
/// When player collects the prefab, it will respawn,
/// allowing them to collect the item until the quantity limit
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 08/12/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;

/// <summary>
/// Handles respawning of collectible items with a limited spawn count
/// and adds the collected sprite to the player's inventory
/// </summary>
public class Respawn : MonoBehaviour
{
    /// <summary>
    /// The sprite representing the ingredient to be added to the inventory
    /// </summary>
    public Sprite ingredientSprite;
    
    /// <summary>
    /// Prefab to respawn when the item reappears
    /// </summary>
    public GameObject itemPrefab;     // prefab to respawn

    /// <summary>
    /// Time delay before the item respawns after being collected
    /// </summary>
    public float respawnTime = 10f;

    /// <summary>
    /// Initial delay before the item first appears in the scene
    /// </summary>
    public float delayTime = 10f;

    
    /// <summary>
    /// Current number of times the item has respawned
    /// </summary>
    public int counter = 0;

    /// <summary>
    /// Maximum number of times the item is allowed to respawn
    /// </summary>
    public int maxCounter = 2;
     // tell inventory where to place this sprite

    /// <summary>
    /// Hides the item at the start and begins the initial spawn timer
    /// </summary>
    void Start()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RespawnItem), delayTime);
        Debug.Log("Spawn timer started");
    }

    /// <summary>
    /// Called when the player collects the item
    /// Adds the sprite to the inventory and handles respawn logic
    /// </summary>
    public void CollectItem()
    {
        if (counter < maxCounter)
        {
            // Hide or destroy collected object
            gameObject.SetActive(false); 
            InventoryManager.Instance.AddItemToSlot(ingredientSprite);


            // Start respawn timer
            Invoke(nameof(RespawnItem), respawnTime);
            counter++;
        }
        else
        {
            Debug.Log("No more Spawn");
        }
    }

    /// <summary>
    /// Reactivates the item in the scene, making it collectible again
    /// </summary>
    void RespawnItem()
    {
        gameObject.SetActive(true);
    }
}
