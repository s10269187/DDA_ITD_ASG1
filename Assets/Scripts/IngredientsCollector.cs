// using UnityEngine;
// using System.Collections.Generic;

// public class IngredientsCollector : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     public List<string> inventory = new List<string>();

//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.touchCount == 0) return;

//         Touch touch = Input.GetTouch(0);
//         if (touch.phase != TouchPhase.Began) return;

//         Ray ray = Camera.main.ScreenPointToRay(touch.position);
        
//         if (Physics.Raycast(ray, out RaycastHit hit))
//         {
//             if (hit.collider.CompareTag("Ingredient"))
//             {
//                 inventory.Add(hit.collider.name);
//                 Destroy(hit.collider.gameObject);
//                 Debug.Log($"Collected: {hit.collider.name}");
//             }
//         }        
//     }
// }
