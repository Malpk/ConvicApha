using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Underworld
{
    public class MaskShape
    {
        private readonly TermMode[,] shape;
        private readonly Point[,] points;
        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.right, Vector2Int.up,Vector2Int.zero,Vector2Int.one
        };
        private TetrisMode mode;

        private Vector2Int _curretPOsition;
        private List<Point> _priviusShape = new List<Point>();

        public delegate void OutToMap(MaskShape shape);
        public event OutToMap OutTioMapAction;

        public MaskShape(Point[,] points, Shape shape,TetrisMode mode)
        {
            this.mode = mode;
            this.points = points;
            this.shape = NormalizeShape(shape);
            _curretPOsition = DefinePosition(this.shape, points);
        }
        public int Delth => _curretPOsition.x;
        private Vector2Int DefinePosition(TermMode[,] shape, Point[,] points)
        {
            var position = Random.Range(0, (points.GetLength(1)) - shape.GetLength(1) + 1);
            return Vector2Int.up * position;
        }
        private TermMode[,] NormalizeShape(Shape shape)
        {
            var index = Random.Range(0, 2);
            TermMode[,] newShape = index == 1 ? Rotate(shape.ShapeMap) : shape.ShapeMap;
            index = Random.Range(0, directions.Length);
            newShape = Mirror(newShape, directions[index]);
            return newShape;
        }
        private TermMode[,] Rotate(TermMode[,] shape)
        {
            var newShape = new TermMode[shape.GetLength(1), shape.GetLength(0)];
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    newShape[j, i] = shape[i,j];
                }
            }
            return newShape;
        }
        private TermMode[,] Mirror(TermMode[,] shape, Vector2Int direction)
        {
            shape = direction.x == 1 ? MirrorX(shape) : shape;
            shape = direction.y == 1 ? MirrorY(shape) : shape;
            return shape;
        }
        private TermMode[,] MirrorX(TermMode[,] shape)
        {
            var lostElement = shape.GetLength(1) - 1;
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; i < shape.GetLength(1) / 2; i++)
                {
                    var temp = shape[i, j];
                    shape[i, j] = shape[i, lostElement - j];
                    shape[i, lostElement - j] = temp;
                }
            }
            return shape;
        }
        private TermMode[,] MirrorY(TermMode[,] shape)
        {
            var lostElement = shape.GetLength(0) - 1;
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; i < shape.GetLength(1) / 2; i++)
                {
                    var temp = shape[i, j];
                    shape[i, j] = shape[lostElement - i, j];
                    shape[lostElement - i, j] = temp;
                }
            }
            return shape;
        }
        private List<Point> DrawShape(Vector2Int startPosition, int curretDethDeadLine)
        {
            var list = new List<Point>();
            var y = shape.GetLength(0) - 1;
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                int delth = 0;
                for (int i = 0; i < shape.GetLength(0); i++)
                {
                    if (startPosition.x - y + i >= 0)
                    {
                        var curretPosition = startPosition.x - delth + curretDethDeadLine;
                        if (shape[delth, j] == TermMode.AttackMode && curretPosition < points.GetLength(1))
                        {
                            var point = points[startPosition.y + j, startPosition.x - delth];
                            list.Add(point);
                            point.SetAtiveObject(true);
                        }
                        delth++;
                    }
                }
            }
            return list;
        }

        public void MoveUpdate()
        {
            if (CheakOutToMap(_curretPOsition,mode.CurretDeadLineHeight))
                return;
            var newDrawShape = DrawShape(_curretPOsition, mode.CurretDeadLineHeight);
            foreach (var point in newDrawShape)
            {
                if (!_priviusShape.Remove(point))
                {
                    point.Animation.StartTile();
                    point.Animation.IdleMode();
                }
            }
            if (_priviusShape != null)
            {
                foreach (var point in _priviusShape)
                {
                    point.SetAtiveObject(false);
                }
            }
            _priviusShape = newDrawShape;
            _curretPOsition += Vector2Int.right; 
           
        }

        private bool CheakOutToMap(Vector2Int curretPosition,int curretDelth)
        {
            if (curretPosition.x - shape.Length + curretDelth > points.GetLength(0) - 1)
            {
                if (OutTioMapAction != null)
                    OutTioMapAction(this);
                return true;
            }
            return false;
        }
    }
}