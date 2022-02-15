using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Trident
{
    public abstract class BaseTridentMode : MonoBehaviour
    { 
        [Header("Perfab Setting")]
        [SerializeField] protected Vector2Int sizeMap;
        [SerializeField] protected Mode setting;
        [SerializeField] protected GameObject trident;
        [SerializeField] protected GameObject spawner;
        [SerializeField] protected MarkerSetting[] tridentSetting;

        protected abstract IEnumerator RunMode();
        protected void Intializate(Tilemap tilemap)
        {
            foreach (var setting in tridentSetting)
            {
                setting.VertexList = Intializate(SizeMap(setting.VerticalMode), setting.Direction, tilemap);
            }
        }
        protected int SizeMap(bool vertical)
        {
            if (vertical)
                return sizeMap.x / 2;
            else
                return sizeMap.y / 2;
        }
        protected List<IVertex> Intializate(int sizeAxis, Vector3Int maskDirection, Tilemap tileMap)
        {
            List<IVertex> list = new List<IVertex>();
            for (int i = -sizeAxis; i < sizeAxis; i++)
            {
                var position = tileMap.GetCellCenterWorld(maskDirection * i);
                list.Add(new VertexTrident(position));
            }
            return list;
        }
        protected GameObject CreateTrindent(int offset,float warningTime)
        {
            GameObject lostTrident = null;
            foreach (var setting in tridentSetting)
            {
                var point = ChooseVertex(setting.VertexList);
                if (point != null)
                {
                    lostTrident = point.InstateObject(spawner, transform.parent);
                    lostTrident.transform.rotation = Quaternion.Euler(Vector3.forward * setting.MarkerAngle);
                    if (lostTrident.TryGetComponent<Marker>(out Marker marker))
                    {
                        marker.Constructor(setting.Angls, warningTime, setting.Direction * offset, trident);
                    }
                }
            }
            return lostTrident;
        }
        protected IVertex ChooseVertex(List<IVertex> list)
        {
            List<IVertex> unBusy = new List<IVertex>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].state == VertexState.UnBusy)
                    unBusy.Add(list[i]);
            }
            if (unBusy.Count == 0)
                return null;
            int index = Random.Range(0, unBusy.Count);
            return unBusy[index];
        }
    }
 
}
