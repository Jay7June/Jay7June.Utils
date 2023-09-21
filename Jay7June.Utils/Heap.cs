using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Jay7June.Utils
{
    public class Heap<T>
    {
        readonly List<T> _elements;
        private int _changeIndex = -1;
        public readonly IComparer<T> HeapComparer;

        public int Count
        {
            get { return _changeIndex == -1 ? _elements.Count : _elements.Count - 1; }
        }

        public int Capacity
        {
            set { _elements.Capacity = _elements.Count >= value ? _elements.Count : value; }
            get { return _elements.Capacity; }
        }

        /// <summary>
        ///Initialize heap
        /// </summary>
        /// <param name="comparer">x is child,y is parent</param>
        public Heap(IComparer<T> comparer)
        {
            _elements = new List<T>();
            HeapComparer = comparer;
        }

        /// <summary>
        ///Initialize heap with capacity
        /// </summary>
        /// <param name="capacity">heap capacity</param>
        /// <param name="comparer">x is child,y is parent</param>
        public Heap(int capacity, IComparer<T> comparer)
        {
            _elements = new List<T>(capacity);
            HeapComparer = comparer;
        }

        /// <summary>
        ///Initialize heap with list
        /// </summary>
        /// <param name="elements">heap elements</param>
        /// <param name="comparer">x is  child,y is parent</param>
        public Heap(List<T> elements, IComparer<T> comparer)
        {
            _elements = new List<T>(elements.Count);
            HeapComparer = comparer;
            for (int i = 0; i < elements.Count; i++)
            {
                _elements.Add(elements[i]);
                BubbleUp(i);
            }
        }

        public void Add(T item)
        {
            if (_changeIndex > -1)
            {
                _elements[_changeIndex] = item;
            }
            else
            {
                _changeIndex = _elements.Count;
                _elements.Add(item);
            }
            BubbleUp(_changeIndex);
            BubbleDown(_changeIndex);
            _changeIndex = -1;
        }

        public void Remove(T item)
        {
            if (_changeIndex > -1)
            {
                _elements[_changeIndex] = _elements[_elements.Count - 1];
                _elements.RemoveAt(_elements.Count - 1);
                BubbleUp(_changeIndex);
                BubbleDown(_changeIndex);
                _changeIndex = -1;
            }
            int i = _elements.IndexOf(item);
            _changeIndex = i;
        }

        public T Pop()
        {
            if (_elements.Count > 1)
            {
                if (_changeIndex > -1)
                {
                    _elements[_changeIndex] = _elements[_elements.Count - 1];
                    _elements.RemoveAt(_elements.Count - 1);
                    BubbleUp(_changeIndex);
                    BubbleDown(_changeIndex);
                }
                T rst = _elements[0];
                _changeIndex = 0;
                return rst;
            }
            if (_elements.Count == 1 && _changeIndex == -1)
            {
                _changeIndex = 0;
                return _elements[0];
            }
            throw new InvalidOperationException();
        }

        public bool TryPop([MaybeNullWhen(false)] out T item)
        {
            if (_elements.Count > 1)
            {
                if (_changeIndex > -1)
                {
                    _elements[_changeIndex] = _elements[_elements.Count - 1];
                    _elements.RemoveAt(_elements.Count - 1);
                    BubbleUp(_changeIndex);
                    BubbleDown(_changeIndex);
                }
                item = _elements[0];
                _changeIndex = 0;
                return true;
            }
            if (_elements.Count == 1 && _changeIndex == -1)
            {
                _changeIndex = 0;
                item = _elements[0];
                return true;
            }
            item = default;
            return false;
        }

        public T Peek()
        {
            if (_changeIndex != 0 && _elements.Count > 0)
            {
                return _elements[0];
            }
            if (_changeIndex == 0 && _elements.Count > 1)
            {
                _elements[_changeIndex] = _elements[_elements.Count - 1];
                _elements.RemoveAt(_elements.Count - 1);
                BubbleUp(_changeIndex);
                BubbleDown(_changeIndex);
                _changeIndex = -1;
                return _elements[0];
            }
            throw new InvalidOperationException();
        }

        public bool TryPeek([MaybeNullWhen(false)] out T item)
        {
            if (_changeIndex != 0 && _elements.Count > 0)
            {
                item = _elements[0];
                return true;
            }
            if (_changeIndex == 0 && _elements.Count > 1)
            {
                _elements[_changeIndex] = _elements[_elements.Count - 1];
                _elements.RemoveAt(_elements.Count - 1);
                BubbleUp(_changeIndex);
                BubbleDown(_changeIndex);
                _changeIndex = -1;
                item = _elements[0];
                return true;
            }
            item = default;
            return false;
        }

        public void Clear()
        {
            _elements.Clear();
            _changeIndex = -1;
        }

        private void BubbleUp(int startIndex)
        {
            while (startIndex > 0)
            {
                int parentPosition = (startIndex + 1) / 2 - 1;
                if (parentPosition >= 0 && startIndex < _elements.Count && HeapComparer.Compare(_elements[parentPosition], _elements[startIndex]) > 0)
                {
                    var temp = _elements[parentPosition];
                    _elements[parentPosition] = _elements[startIndex];
                    _elements[startIndex] = temp;
                    startIndex = parentPosition;
                }
                else
                {
                    break;
                }
            }
        }

        private void BubbleDown(int rootIndex)
        {
            int leftChildIndex = (rootIndex * 2) + 1;
            while (leftChildIndex <= _elements.Count - 2)
            {
                int rightChildIndex = leftChildIndex + 1;
                int bestChildIndex =
                    HeapComparer.Compare(_elements[leftChildIndex], _elements[rightChildIndex]) > 0 ? rightChildIndex : leftChildIndex;
                if (rootIndex >= 0 && bestChildIndex < _elements.Count && HeapComparer.Compare(_elements[rootIndex], _elements[bestChildIndex]) > 0)
                {
                    var temp = _elements[rootIndex];
                    _elements[rootIndex] = _elements[bestChildIndex];
                    _elements[bestChildIndex] = temp;
                    rootIndex = bestChildIndex;
                    leftChildIndex = (rootIndex * 2) + 1;
                }
                else
                {
                    break;
                }
            }
            if (leftChildIndex < _elements.Count && HeapComparer.Compare(_elements[rootIndex], _elements[leftChildIndex]) > 0)
            {
                var temp = _elements[rootIndex];
                _elements[rootIndex] = _elements[leftChildIndex];
                _elements[leftChildIndex] = temp;
            }
        }
    }
}
