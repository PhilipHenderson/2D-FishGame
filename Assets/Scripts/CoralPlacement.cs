using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralPlacement : MonoBehaviour
{
    public GameObject coralPrefab;  // Prefab of the coral object
    public List<Sprite> coralSprites;  // List of coral sprites
    public int numberOfCorals = 10;  // Number of coral objects to spawn
    public Vector2 scaleRange;  // Scale range for the coral objects

    private List<GameObject> spawnedCorals = new List<GameObject>();  // List to store spawned coral objects

    private void Start()
    {
        // Spawn coral objects
        for (int i = 0; i < numberOfCorals; i++)
        {
            SpawnCoral();
        }
    }

    public void SpawnCoral()
    {
        // Generate random position within the specified range
        float randomX = Random.Range(-10.0f, 10.0f);
        Vector3 spawnPosition = new Vector3(randomX, -5.0f, 0.0f);

        // Choose a random coral sprite from the list
        int randomIndex = Random.Range(0, coralSprites.Count);
        Sprite coralSprite = coralSprites[randomIndex];

        // Create coral object
        GameObject coralObject = Instantiate(coralPrefab, spawnPosition, Quaternion.identity);
        coralObject.transform.SetParent(transform);  // Set the parent object as the parent of the coral object

        // Change the sprite of the coral object
        SpriteRenderer spriteRenderer = coralObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = coralSprite;
        }
        else
        {
            Debug.LogWarning("Coral object does not have a SpriteRenderer component.");
        }

        // Randomly scale the coral object within the specified range
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        coralObject.transform.localScale = new Vector3(randomScale, randomScale, 1.0f);

        // Add the spawned coral object to the list
        spawnedCorals.Add(coralObject);
    }

    public void DestroyAllCorals()
    {
        // Iterate through the spawned coral objects and destroy them
        foreach (GameObject coralObject in spawnedCorals)
        {
            Destroy(coralObject);
        }

        // Clear the list of spawned coral objects
        spawnedCorals.Clear();
    }

    public void RegenerateCoral()
    {
        DestroyAllCorals();
        // Spawn coral objects
        for (int i = 0; i < numberOfCorals; i++)
        {
            SpawnCoral();
        }
    }
}
