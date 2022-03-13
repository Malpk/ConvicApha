using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public abstract class GameMode : MonoBehaviour
    {
        protected Coroutine startMode = null;

        public bool StatusWork => startMode != null;

        protected void TurnOnPoints(Point[,] map)
        {
            foreach (var point in map)
            {
                point.SetAtiveObject(true);
            }
        }
        protected void ActivateTile(Point[,] map)
        {
            foreach (var point in map)
            {
                point.SetAtiveObject(true);
                point.Animation.StartTile();
            }
        }
        protected Point TurnOffPoints(Point[,] map)
        {
            int i = map.GetLength(0) - 1;
            int j = map.GetLength(1) - 1;
            foreach (var point in map)
            {
                point.Animation.Stop();
            }
            return map[i, j];
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PoolTerm>(out PoolTerm term))
            {
                term.SetActiveMode(false);
                term.Stop();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PoolTerm>(out PoolTerm term))
            {
                term.SetActiveMode(true);
                term.StartTile();
            }
        }
    }
}