using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserIntaface.MainMenu
{
    public class RingList<T> : IEnumerable<T>
    {
        private Node<T> _head;
        private Node<T> _tail;
        private Node<T> _selectedNode;
        private List<Node<T>> _list = new List<Node<T>>();
        private int _count;

        public RingList(List<T> list)
        {
            foreach (var item in list)
                Add(item);

            Selected = _head;
        }
        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);
            _list.Add(node);
            if (_head == null)
            {
                _head = node;
                _tail = node;
                _tail.Next = _head;
                _tail.Previous = _tail;
            }
            else
            {
                node.Next = _head;
                node.Previous = _tail;
                _tail.Next = node;
                _tail = node;
                _head.Previous = _tail;
            }
            _count++;
        }

        public Node<T> this[int index]
        {
            get => _list[index];
        }
        public Node<T> Selected
        {
            get
            {
                if (_selectedNode != null)
                    return _selectedNode;
                else
                    return default(Node<T>);
            }
            private set
            {
                _selectedNode = value;
            }
        }

        public bool Remove(T data)
        {
            Node<T> current = _head;
            Node<T> previous = null;

            if (IsEmpty) return false;

            do
            {
                if (current.Data.Equals(data))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current == _tail)
                            _tail = previous;
                    }
                    else
                    {
                        if (_count == 1)
                        {
                            _head = _tail = null;
                        }
                        else
                        {
                            _head = current.Next;
                            _tail.Next = current.Next;
                        }
                    }
                    _count--;
                    return true;
                }
                previous = current;
                current = current.Next;

            } while (current != _head);
            return false;


        }
        public int Count { get { return _count; } }
        public bool IsEmpty { get { return _count == 0; } }

        public void RotateRight() =>
            Selected = Selected.Next;

        public void RotateLeft() =>
            Selected = Selected.Previous;


        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }
        public bool Contains(T data)
        {
            Node<T> current = _head;
            if (current == null) return false;
            do
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            while (current != _head);
            return false;
        }
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = _head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != _head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
    }
}
