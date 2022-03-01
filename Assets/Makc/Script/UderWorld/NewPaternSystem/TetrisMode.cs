using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SwitchModeComponent;

namespace Underworld
{
    public class TetrisMode : MonoBehaviour,ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private int _maxDeadLineHeight;
        [SerializeField] private int _minDistanseBothShape;
        [SerializeField] private int _countShape;
        [SerializeField] private float _shapeSpeed;
        [SerializeField] private float _delayMoveDeadLine;
        [SerializeField] private bool _inversMode;
        [Header("Layout Setting")]
        [SerializeField] private Sprite _mapShape;
        [SerializeField] private Shape[] _shape;

        private MapBuilder _builder;
        private Point [,] _points;
        private List<MaskShape> _listShaps = new List<MaskShape>();
        private List<MaskShape> _deleteShape = new List<MaskShape>();
        private Coroutine _coroutine;
        private List<Point> _deadLine = new List<Point>();


        public int CurretDeadLineHeight { get; private set; }

        public bool IsAttackMode => _coroutine!=null;

        public void Constructor(SwitchMods swictMode)
        {
            if (_coroutine != null)
                return;
            _builder = swictMode.builder;
            _points = _builder.Map;
            foreach (var shape in _shape)
            {
                shape.CreateShape(_mapShape.texture, _inversMode);
            }
            _coroutine = StartCoroutine(ShapeCreater());
            StartCoroutine(MoveShape());
            StartCoroutine(MoveDeadLine());
        }
        private IEnumerator ShapeCreater()
        {
            for (int i = 0; i < _countShape; i++)
            {
                int index = Random.Range(0, _shape.Length);
                var newShape = new MaskShape(_points, _shape[index],this);
                newShape.OutTioMapAction += AddInDelateList;
                _listShaps.Add(newShape);
                yield return new WaitWhile(() => newShape.Delth < _minDistanseBothShape);
            }
            yield return null;
        }
        private IEnumerator MoveShape()
        {
            yield return new WaitWhile(() => _listShaps.Count == 0);
            while (_listShaps.Count > 0)
            {
                foreach (var shape in _listShaps)
                {
                    shape.MoveUpdate();
                }
                yield return new WaitForSeconds(_shapeSpeed);
                DeleyShape();
            }
            Debug.Log("stop");
            var lostPoint = _builder.TurnOffAllTile();
            yield return new WaitWhile(() => lostPoint.IsActive);
            Destroy(gameObject);
            _coroutine = null;
            yield return null;
        }
        private IEnumerator MoveDeadLine()
        {
            var maxDelth = _points.GetLength(0) - 1;
            yield return new WaitWhile(() => _listShaps.Count == 0);
            for (int i = 0; i < _maxDeadLineHeight && _listShaps.Count > 0; i++)
            {
                CurretDeadLineHeight = i;
                yield return new WaitForSeconds(_delayMoveDeadLine);
                for (int j = 0; j < _points.GetLength(0); j++)
                {
                    _points[maxDelth - i, j].SetAtiveObject(true);
                    _points[maxDelth - i, j].Animation.StartTile();
                    _deadLine.Add(_points[maxDelth - i, j]);
                }
            }
        }
        public void OffPoint(List<Point> listPoints)
        {
            for (int i = 0; i < listPoints.Count; i++)
            {
                if (!_deadLine.Contains(listPoints[i]))
                    listPoints[i].SetAtiveObject(false);
            }
        }
        private void AddInDelateList(MaskShape shape)
        {
            _deleteShape.Add(shape);
            shape.OutTioMapAction -= AddInDelateList;
        }
        private void DeleyShape()
        {
            foreach (var shape in _deleteShape)
            {
                _listShaps.Remove(shape);
            }
            _deleteShape.Clear();
        }
    }
}