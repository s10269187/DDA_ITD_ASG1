using UnityEngine;

public class IngredientsSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * (1f + i * 0.5f);
            Instantiate(ingredientPrefab, spawnPos, Quaternion.identity);
        }        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
