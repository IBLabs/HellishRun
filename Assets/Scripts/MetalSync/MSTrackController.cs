using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTrackController : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public GameObject prefab;
        public float length;
    }

    public List<Tile> tiles; // The list of tile prefabs
    public int initialTileCount = 5; // The number of tiles to initially spawn
    public float speed = 5.0f; // The speed at which the tiles move

    private List<GameObject> activeTiles; // The list of currently active tiles
    private float totalLength;

    private void Start()
    {
        // Initialize the activeTiles list
        activeTiles = new List<GameObject>();
        totalLength = 0.0f;

        // Spawn the initial tiles
        for (int i = 0; i < initialTileCount; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        // Move the tiles
        foreach (GameObject tile in activeTiles)
        {
            tile.transform.position -= Vector3.forward * (speed * Time.deltaTime);
        }

        // If the first tile has moved past its length, remove it and spawn a new tile
        Tile firstTile = tiles[0];
        if (activeTiles[0].transform.position.z <= -firstTile.length)
        {
            totalLength -= firstTile.length;
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);

            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        // Select a tile randomly
        Tile tileToSpawn = tiles[Random.Range(0, tiles.Count)];

        // Create a new tile at the end of the current track
        GameObject tile = Instantiate(tileToSpawn.prefab, new Vector3(0, 0, totalLength), Quaternion.identity, transform);
        activeTiles.Add(tile);

        // Update the total length of the track
        totalLength += tileToSpawn.length;
    }
}