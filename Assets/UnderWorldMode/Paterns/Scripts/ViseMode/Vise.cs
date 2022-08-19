using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class Vise : MonoBehaviour,IPause
    {
        private bool _isPause;
        private Coroutine _startMove = null;
        
        private Vector2[] _way = null;

        private Transform _leftVise;
        private Transform _rightVise;
        private List<HandTermTile> _termList = new List<HandTermTile>();
        
        public bool IsMoveVise => _startMove != null;


        #region Create Vise
        public bool CreateVise(GameObject tile,Point[,] map, ViseTypeWork typeWork)
        {
            if (!_leftVise || !_rightVise)
            {
                transform.rotation = Quaternion.Euler(Vector3.forward * GetAngel(typeWork));
                if(_leftVise)
                    _leftVise = CreateVise(map, 0, tile);
                if(_rightVise)
                    _rightVise = CreateVise(map, map.GetLength(0) - 1, tile);
                _way = GetWay(typeWork, map, _leftVise.position);
                return true;
            }
            return false;
        }
        #region Behaviour Vise
        private float GetAngel(ViseTypeWork type)
        {
            switch (type)
            {
                case ViseTypeWork.Vertical:
                    return 0;
                default:
                    return 90;
            }
        }
        private Vector2[] GetWay(ViseTypeWork type, Point[,] map, Vector2 holder)
        {
            if (type == ViseTypeWork.Horizontal)
            {
                var way = new Vector2[map.GetLength(1)];
                for (int i = 0; i < way.Length; i++)
                {
                    way[i] = new Vector2(map[0, i].Position.x, holder.y);
                }
                return way;
            }
            else
            {
                var way = new Vector2[map.GetLength(0)];
                for (int i = 0; i < way.Length; i++)
                {
                    way[i] = new Vector2(holder.x, map[i, 0].Position.y);
                }
                return way;
            }
        }
        #endregion
        #region Create
        private Transform CreateVise(Point[,] map, int position, GameObject tile)
        {
            var holder = GetHolder("vise");
            holder.position = Vector3.right * map[0, position].Position.x;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                if (CreateTerm(tile, map[i, position].Position, out HandTermTile term))
                {
                    _termList.Add(term);
                    term.transform.parent = holder;
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
        private bool CreateTerm(GameObject tile, Vector2 position, out HandTermTile term)
        {
            var instateTile = Instantiate(tile, position, Quaternion.identity);
            if (instateTile.TryGetComponent(out HandTermTile handTerm))
            {
                term = handTerm;
                return true;
            }
            term = null;
            return false;
        }
        #endregion
        #endregion

        public bool Run(float moveDelay)
        {
            if (_startMove != null)
                return false;
            foreach (var term in _termList)
            {
                term.SetMode(true);
            }
            _startMove = StartCoroutine(MoveVise(moveDelay));
            return true;
        }
        #region Work
        public void Activate()
        {
            foreach (var term in _termList)
            {
                term.Activate(FireState.Stay);
            }
        }
        public IEnumerator Deactivate()
        {
            foreach (var term in _termList)
            {
                term.Deactivate();
            }
            yield return new WaitWhile(() => _termList[_termList.Count - 2].IsActive);
        }
        private IEnumerator MoveVise(float moveDelay)
        {
            for (int i = 0; i < _way.Length; i++)
            {
                _leftVise.localPosition = _way[i];
                _rightVise.localPosition = _way[_way.Length - 1 - i];
                yield return WaitTime(moveDelay);
            }
            yield return StartCoroutine(Hide());
            _startMove = null;
        }
        private IEnumerator Hide()
        {
            foreach (var term in _termList)
            {
                term.SetMode(false);
            }
            yield return new WaitWhile(() => _termList[_termList.Count - 1].IsShow);
        }
        #endregion
        protected IEnumerator WaitTime(float duration)
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return new WaitWhile(() => _isPause);
                yield return null;
                progress += Time.deltaTime / duration;
            }
        }

        public void Pause()
        {
            _isPause = true;
            foreach (var term in _termList)
            {
                term.Pause();
            }
        }

        public void UnPause()
        {
            _isPause = false;
            foreach (var term in _termList)
            {
                term.UnPause();
            }
        }
    }
}