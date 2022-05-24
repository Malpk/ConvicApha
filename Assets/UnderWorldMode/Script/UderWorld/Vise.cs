using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class Vise : MonoBehaviour
    {
        private Vector3 [] _targetPosition = new Vector3[2];
        private Coroutine _startMove = null;
        private Transform[] _vise = null;
        private List<PoolTerm> _termList = new List<PoolTerm>();

        public bool IsMoveVise => _startMove != null;

        public bool CreateVise(GameObject tile,Point[,] map, float angel)
        {
            if (_vise != null)
                return false;
            _vise = new Transform[2];
            _vise[0] = CreateVise(map, 0, tile);
            _vise[1] = CreateVise(map, map.GetLength(0) - 1, tile);
            transform.rotation = Quaternion.Euler(Vector3.forward * angel);
            _targetPosition[1] = _vise[0].position;
            _targetPosition[0] = _vise[1].position;
            return true;
        }
        public bool StartMove(float delayOffset,float offset)
        {
            if (_startMove != null)
                return false;
            foreach (var term in _termList)
            {
                term.SetActiveMode(true);
            }
            _startMove = StartCoroutine(MoveVise(delayOffset, offset));
            return false;
        }
        private IEnumerator MoveVise(float delayOffset, float offset)
        {
            while (_vise[0].position != _targetPosition[0]
                              && _vise[1].position != _targetPosition[1])
            {
                yield return new WaitForSeconds(delayOffset);
                for (int i = 0; i < _vise.Length; i++)
                {
                    _vise[i].position = Vector3.MoveTowards(_vise[i].position, _targetPosition[i], offset);
                }
            }
            yield return StartCoroutine(DeactiveVise());
            _startMove = null;
        }
        private Transform CreateVise(Point[,] map,int position, GameObject tile)
        {
            var holder = GetHolder("vise");
            holder.position = Vector3.right * map[0, position].Position.x;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                var instateTile = Instantiate(tile, map[i, position].Position, Quaternion.identity);
                instateTile.transform.parent = holder;
                if (instateTile.TryGetComponent<PoolTerm>(out PoolTerm poolTerm))
                {
                    _termList.Add(poolTerm);
                }
                else
                {
                    throw new System.NullReferenceException($" {tile.name} is not component \"PoolTerm \"");
                }
            }
            return holder;
        }
        private Transform GetHolder(string name)
        {
            var holder = new GameObject(name).transform;
            holder.parent = transform;
            return holder;
        }
        public IEnumerator SetIdleMode()
        {
            foreach (var term in _termList)
            {
                 StartCoroutine(term.WarningMode());
            }
            yield return new WaitWhile(() => _termList[_termList.Count - 2].IsActive);
        }
        private IEnumerator DeactiveVise()
        {
            foreach (var term in _termList)
            {
                term.Stop();
            }
            yield return new WaitWhile(() => _termList[_termList.Count - 1].IsActive);
            ResetPosition();
        }
        private void ResetPosition()
        {
            _vise[0].position = _targetPosition[1];
            _vise[1].position = _targetPosition[0];
        }
        public void SetFireMode()
        {
            foreach (var term in _termList)
            {
                term.StartTile();
            }
        }
    }
}