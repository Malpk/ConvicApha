using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Trident
{
    public class DeadLineHolder : MonoBehaviour
    {
        [SerializeField] private float _instateDelay;

        private DeadlinePoint[] _points;

        private void Awake()
        {
            _points = GetComponentsInChildren<DeadlinePoint>();
        }
        public void InstateTrident(int angel, TridentSetting trident)
        {
            var point = ChoosePoint (GetListPoint(_points));
            if (point != null)
            {
                point.TurnOn(trident, angel, _instateDelay);
            }
        }

        private DeadlinePoint ChoosePoint(List<DeadlinePoint> points)
        {
            if (points.Count > 0)
            {
                int index = Random.Range(0, points.Count);
                return points[index];
            }
            else
            {
                return null;
            }
        }
        private List<DeadlinePoint> GetListPoint(DeadlinePoint[] points)
        {
            List<DeadlinePoint> unoccupied = new List<DeadlinePoint>();
            for (int i = 0; i < points.Length; i++)
            {
                if (!points[i].isBusy)
                    unoccupied.Add(points[i]);
            }
            return unoccupied;
        }

      
    }
}