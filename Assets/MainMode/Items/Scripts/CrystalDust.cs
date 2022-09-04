﻿using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace MainMode.Items
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CrystalDust : ConsumablesItem
    {
        [SerializeField] private CrystalSheild _wallCrystalPrefab;
        [SerializeField] GameObject dustAnim;
        [SerializeField] private GameObject wallBreakParticle;
        [SerializeField] private GameObject blockPutParticle;
        private Tilemap wallsTileMap;
        private NavMeshSurface2d agentSurface;
        private Player _player;

        public override void Use()
        {
            FindGameObjAtScene();
            Vector3 posFrontPlayer = _player.Position + (Vector2) _player.transform.up;
            var AnimObj = Instantiate(dustAnim, _player.Position, _player.transform.rotation);
            AnimObj.transform.Rotate(Vector3.forward, 180);
            Vector3Int tilePos = wallsTileMap.WorldToCell(posFrontPlayer);
            if (wallsTileMap.GetTile<Tile>(tilePos))
            {
                wallsTileMap.SetTile(tilePos, null);
                Instantiate(blockPutParticle, posFrontPlayer, Quaternion.identity);
                agentSurface.BuildNavMesh();
            }
            else
            {
                Instantiate(_wallCrystalPrefab.gameObject, posFrontPlayer, Quaternion.identity, agentSurface.transform);
                Instantiate(blockPutParticle, posFrontPlayer, Quaternion.identity);
            }
        }

        private void FindGameObjAtScene()
        {
            wallsTileMap = GameObject.FindWithTag("CrystalWalls").GetComponent<Tilemap>();
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            agentSurface = GameObject.FindWithTag("NavMesh").GetComponent<NavMeshSurface2d>();
        }
    }
}
