using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public abstract class GameMode : MonoBehaviour
    {
        protected Coroutine startMode = null;

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
                point.Animation.Activate();
            }
        }
        protected Point TurnOffPoints(Point[,] map)
        {
            Point lostPoint = null;
            foreach (var point in map)
            {
                point.Animation.Deactivate();
                lostPoint = point;
            }
            return lostPoint;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PoolTerm>(out PoolTerm term) && startMode!=null)
            {
                term.SetMode(false);
                term.Deactivate();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PoolTerm>(out PoolTerm term) && startMode != null)
            {
                term.SetMode(true);
                //term.Activate();
            }
        }
    }
}