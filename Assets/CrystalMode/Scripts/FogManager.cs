
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogManager : MonoBehaviour
{
    private Transform player;
    private List<List<SpriteRenderer>> groups = new List<List<SpriteRenderer>>();
    private int currentGroup;
    [SerializeField] private int groupCount;
    [SerializeField] float fadeSpeed;
    [SerializeField] private float range;
    [SerializeField] private GameObject fogObj;
    private int sizeX = 38, sizeY = 38;
    private Tilemap crystalWallsTilemap;

    private void Start()
    {
        crystalWallsTilemap = GameObject.FindWithTag("CrystalWalls").GetComponent<Tilemap>();
        FillFog();
        FillGroups();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        for (int i = 0; i < groupCount - 1; i++)
        {
            if (currentGroup == i)
            {
                foreach (var fogTile in groups[i])
                {
                    float changeValue = RayToPlayer(fogTile) ? -Time.deltaTime * fadeSpeed : Time.deltaTime * fadeSpeed;
                    ChangeTileAlphaColor(fogTile, changeValue);
                }
            }
        }
        currentGroup++;
        if (currentGroup == groupCount)
        {
            currentGroup = 0;
        }
    }

    private void ChangeTileAlphaColor(SpriteRenderer tile, float changeValue)
    {
        if (tile.color.a > 0.99f && changeValue > 0)
            return;
        if (tile.color.a < 0.01f && changeValue < 0)
            return;
        
        tile.color = new Color(tile.color.r, tile.color.g, tile.color.b,
            tile.color.a + changeValue);
    }

    bool RayToPlayer(SpriteRenderer fogTile)
    {
        Vector3 dirToPlayer = player.position - fogTile.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(fogTile.transform.position, dirToPlayer, 1000);
        return hit && hit.collider.gameObject.CompareTag("Player") && hit.distance < range;
    }
    

    private void FillGroups()
    {
        List<SpriteRenderer> currentGroup = null;
        int tilesForGroup = Mathf.RoundToInt(transform.childCount / (groupCount - 1));
        for (int i = 0, addedTilesToGroup = 0; i < transform.childCount; i++, addedTilesToGroup++)
        {
            if (addedTilesToGroup == tilesForGroup || currentGroup == null)
            {
                addedTilesToGroup = 0;
                
                groups.Add(currentGroup = new List<SpriteRenderer>());
            }
            currentGroup.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }

    public void FillFog()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i));
            }
        }
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector3Int tilePos = crystalWallsTilemap.WorldToCell(new Vector3(x - 27.5f, y - 19.5f, 0));
                if (!crystalWallsTilemap.GetTile<Tile>(tilePos))
                {
                    var fogTile = Instantiate(fogObj, transform);
                    fogTile.transform.position = new Vector3(x - 27.5f, y - 19.5f, 0);
                }
            }
        }
    }
}

