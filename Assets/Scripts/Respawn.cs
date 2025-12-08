using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject itemPrefab;     // prefab to respawn
    public float respawnTime = 10f;
    public float delayTime = 10f;
    public int counter = 0;
    public int maxCounter = 2;
    public Ingredient ingredientInfo;

    void Start() 
    {
        if (ingredientInfo == null)
            ingredientInfo = GetComponent<Ingredient>();   
            
        gameObject.SetActive(false);              
        Invoke(nameof(RespawnItem), delayTime);
        Debug.Log("Spawn timer started");
    }

    public void CollectItem()
    {
        if(counter < maxCounter)
        {
            // Update Inventory
            InventoryManager.Instance.AddItem(ingredientInfo.ingredientName, ingredientInfo.ingredientSprite);        
            // Hide or destroy collected object
            gameObject.SetActive(false);

            // Start respawn timer
            Invoke(nameof(RespawnItem), respawnTime);
            counter++;
        }
        else{
            Debug.Log("No more Spawn");
        }
    }

    void RespawnItem()
    {
        gameObject.SetActive(true);
    }
}
