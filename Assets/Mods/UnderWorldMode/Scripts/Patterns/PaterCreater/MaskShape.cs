//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace Underworld
//{
//    public class MaskShape
//    {
//        private readonly TermMode[,] shape;
//        private readonly Point[,] points;
//        private readonly Vector2Int[] directions = new Vector2Int[]
//        {
//            Vector2Int.right, Vector2Int.up,Vector2Int.zero,Vector2Int.one
//        };
//        private readonly Vector2Int bounds; 

//        private Vector2Int _curretPOsition;
//        private List<Point> _priviusShape = new List<Point>();

//        public delegate void OutToMap(MaskShape shape);
//        public event OutToMap OutTioMapAction;

//        public MaskShape(Point[,] points, Shape shape,TetrisMode mode)
//        {
//            this.mode = mode;
//            this.points = points;
//            this.shape = NormalizeShape(shape);
//            bounds = new Vector2Int(points.GetLength(0),
//            points.GetLength(1));
//            _curretPOsition = DefinePosition(this.shape, points);
//        }
//        public int Delth => _curretPOsition.y;
//        private Vector2Int DefinePosition(TermMode[,] shape, Point[,] points)
//        {
//            var position = Random.Range(0,points.GetLength(1) - shape.GetLength(1));

//            return Vector2Int.right * position;
//        }
//        private TermMode[,] NormalizeShape(Shape shape)
//        {
//            var index = Random.Range(0, 2);
//            TermMode[,] newShape = index == 1 ? Rotate(shape.ShapeMap) : shape.ShapeMap;
//            index = Random.Range(0, directions.Length);
//            newShape = Mirror(newShape, directions[index]);
//            return newShape;
//        }
//        private TermMode[,] Rotate(TermMode[,] shape)
//        {
//            var newShape = new TermMode[shape.GetLength(1), shape.GetLength(0)];
//            for (int i = 0; i < shape.GetLength(0); i++)
//            {
//                for (int j = 0; j < shape.GetLength(1); j++)
//                {
//                    newShape[j, i] = shape[i,j];
//                }
//            }
//            return newShape;
//        }
//        private TermMode[,] Mirror(TermMode[,] shape, Vector2Int direction)
//        {
//            shape = direction.x == 1 ? MirrorX(shape) : shape;
//            shape = direction.y == 1 ? MirrorY(shape) : shape;
//            return shape;
//        }
//        private TermMode[,] MirrorX(TermMode[,] shape)
//        {
//            var lostElement = shape.GetLength(1) - 1;
//            for (int i = 0; i < shape.GetLength(0); i++)
//            {
//                for (int j = 0; i < shape.GetLength(1) / 2; i++)
//                {
//                    var temp = shape[i, j];
//                    shape[i, j] = shape[i, lostElement - j];
//                    shape[i, lostElement - j] = temp;
//                }
//            }
//            return shape;
//        }
//        private TermMode[,] MirrorY(TermMode[,] shape)
//        {
//            var lostElement = shape.GetLength(0) - 1;
//            for (int i = 0; i < shape.GetLength(0); i++)
//            {
//                for (int j = 0; i < shape.GetLength(1) / 2; i++)
//                {
//                    var temp = shape[i, j];
//                    shape[i, j] = shape[lostElement - i, j];
//                    shape[lostElement - i, j] = temp;
//                }
//            }
//            return shape;
//        }
//        private List<Point> DrawShape(Vector2Int startPosition, int curretDethDeadLine)
//        {
//            var list = new List<Point>();
//            for (int i = 0; i < shape.GetLength(0); i++)
//            {
//                for (int j = 0; j < shape.GetLength(1); j++)
//                {
//                    if (shape[i, j] == TermMode.AttackMode)
//                    {
//                        var x = startPosition.y + i;
//                        var y = startPosition.x + j;
//                        if (x < bounds.x - curretDethDeadLine && y < bounds.y )
//                        {
//                            list.Add(points[x, y]);
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        public void MoveUpdate()
//        {
//            if (CheakOutToMap(_curretPOsition, mode.CurretDeadLineHeight))
//            {
//                mode.OffPoint(_priviusShape);
//                return;
//            }
//            var newDrawShape = DrawShape(_curretPOsition, mode.CurretDeadLineHeight);
//            foreach (var point in newDrawShape)
//            {
//                if (!_priviusShape.Remove(point))
//                {
//                    point.SetAtiveObject(true);
//                    point.Animation.Activate();
//                }
//            }
//            mode.OffPoint(_priviusShape);
//            _priviusShape = newDrawShape;
//            _curretPOsition += Vector2Int.up;
//        }
//        private bool CheakOutToMap(Vector2Int curretPosition,int curretDelth)
//        {
//            if (curretPosition.y - shape.GetLength(0) >= points.GetLength(0) - 1 - curretDelth)
//            {
//                if (OutTioMapAction != null)
//                    OutTioMapAction(this);
//                return true;
//            }
//            return false;
//        }
//    }
//}