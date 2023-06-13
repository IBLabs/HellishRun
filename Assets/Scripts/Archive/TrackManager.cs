using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject tilePrefab; // Prefab for the track tile
    public int numberOfTiles = 5; // Number of tiles to maintain
    public float tileLength = 10f; // Length of each tile
    public float speed = 5f; // Speed of tile movement

    private Queue<GameObject> tiles; // Queue to hold the tiles

    void Start()
    {
        tiles = new Queue<GameObject>();

        // Spawn initial tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            GameObject tile = Instantiate(tilePrefab,
                new Vector3(i * tileLength, transform.position.y, transform.position.z), transform.rotation, transform);
            tiles.Enqueue(tile);
        }
    }

    void Update()
    {
        // Move each tile
        foreach (GameObject tile in tiles)
        {
            // tile.transform.Translate(0, 0, -speed * Time.deltaTime);
            tile.transform.position -= Vector3.left * (-speed * Time.deltaTime);
        }

        // Check if the first tile has left the screen
        if (tiles.Peek().transform.position.x <= -tileLength)
        {
            GameObject oldTile = tiles.Dequeue();

            oldTile.transform.position = new Vector3(
                (numberOfTiles - 1) * tileLength,
                transform.position.y,
                transform.position.z
            );

            tiles.Enqueue(oldTile);
        }
    }
}