﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace MainMode.Items
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CrystalDust : ConsumablesItem
    {
        [SerializeField] private CrystalSheild wallCrystalPrefab;
        [SerializeField] private GameObject dustAnim;
        [SerializeField] private GameObject blockPutParticle;
        [SerializeField] private GameObject crashAnimation;
        private Tilemap wallsTileMap;
        
        private NavMeshSurface2d agentSurface;
        private Player _player;

        public override string Name => "Кристальная пыль";


        protected override void UseConsumable()
        {
            FindGameObjAtScene();
            
            var AnimObj = Instantiate(dustAnim, _player.Position, _player.transform.rotation);
            AnimObj.transform.Rotate(Vector3.forward, 180);
            
            Vector3 posFrontPlayer = _player.Position + (Vector2) _player.transform.up;
            Vector3Int tilePos = wallsTileMap.WorldToCell(posFrontPlayer);

            RaycastHit2D hit2D = Physics2D.Raycast(posFrontPlayer - transform.forward * 10, transform.forward, 1000);
            if (hit2D)
            {
                if (hit2D.collider.gameObject.GetComponent<Zombie>())
                {
                    
                    hit2D.collider.gameObject.GetComponent<Zombie>().Kill();
                    return;
                }
            }
            if (wallsTileMap.GetTile<Tile>(tilePos))
            {
                FogManager fogManager = GameObject.FindWithTag("FogManager").GetComponent<FogManager>();
                fogManager.PutFog(tilePos);
                
                wallsTileMap.SetTile(tilePos, null);
                Instantiate(crashAnimation,  new Vector3((float) (tilePos.x + 0.5), (float) (tilePos.y + 0.5), 0), Quaternion.identity);
                
                agentSurface.UpdateNavMesh(agentSurface.navMeshData);
            }
            else
            {
                Instantiate(wallCrystalPrefab.gameObject, posFrontPlayer, Quaternion.identity, agentSurface.transform);
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
