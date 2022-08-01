using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace MainMode.Items
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CrystalDust : ConsumablesItem
    {
        [SerializeField] private CrystalSheild _wallCrystalPrefab;
        [SerializeField] private Player _player;
        private Tilemap wallsTileMap1; 
        private Tilemap wallsTileMap2;
        private NavMeshSurface2d agentSurface;

        public override void Use()
        {
            FindGameObjAtScene();
            Vector3 posFrontPlayer = _player.Position + (Vector2) _player.transform.up;
            Vector3Int tilePos = wallsTileMap1.WorldToCell(posFrontPlayer);
            if (wallsTileMap1.GetTile<Tile>(tilePos))
            {
                wallsTileMap1.SetTile(tilePos, null); 
                agentSurface.BuildNavMesh();
                return;
            }
            if (wallsTileMap2.GetTile<Tile>(tilePos))
            {
                wallsTileMap2.SetTile(tilePos, null);
                agentSurface.BuildNavMesh();
                return;
            }

           /* if (wallsTileMap1.GetTile<Tile>(tilePos + Vector3Int.left))
            {
                wallsTileMap1.SetTile(tilePos + Vector3Int.left, null);
                agentSurface.BuildNavMesh();
                return;
            }
            if (wallsTileMap2.GetTile<Tile>(tilePos + Vector3Int.down))
            {
                wallsTileMap2.SetTile(tilePos + Vector3Int.down, null);
                agentSurface.BuildNavMesh();
                return;
            }*/
           
            Instantiate(_wallCrystalPrefab.gameObject, posFrontPlayer, Quaternion.identity, agentSurface.transform);
        }

        private void FindGameObjAtScene()
        {
            wallsTileMap2 = GameObject.FindWithTag("CrystalWalls2").GetComponent<Tilemap>();
            wallsTileMap1 = GameObject.FindWithTag("CrystalWalls").GetComponent<Tilemap>();
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            agentSurface = GameObject.FindWithTag("NavMesh").GetComponent<NavMeshSurface2d>();
        }
    }
}
