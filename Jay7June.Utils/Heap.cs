using System;
using System.Collections.Generic;

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
            UpdateHeap();
        }

        public void Remove(T item)
        {
            if (_changeIndex > -1)
            {
                DeleteChangedIndexItem();
            }
            int i = _elements.IndexOf(item);
            _changeIndex = i;
        }

        public T Pop()
        {
            if (_changeIndex > -1)
            {
                DeleteChangedIndexItem();
            }
            if (_elements.Count > 0)
            {
                T rst = _elements[0];
                _changeIndex = 0;
                return rst;
            }
            throw new IndexOutOfRangeException();
        }

        public T Peek()
        {
            if (_changeIndex == 0)
            {
                DeleteChangedIndexItem();
            }
            if (_elements.Count > 0)
            {
                return _elements[0];
            }
            throw new IndexOutOfRangeException();
        }

        public void Clear()
        {
            _elements.Clear();
            _changeIndex = -1;
        }

        private void DeleteChangedIndexItem()
        {
            _elements[_changeIndex] = _elements[_elements.Count - 1];
            _elements.RemoveAt(_elements.Count - 1);
            UpdateHeap();
        }

        private void UpdateHeap()
        {
            if (IsBubbleUp())
            {
                BubbleUp(_changeIndex);
            }
            else
            {
                BubbleDown(_changeIndex);
            }
            _changeIndex = -1;
        }

        private bool IsBubbleUp()
        {
            if (_changeIndex == 0)
            {
                return false;
            }
            var parentIndex = (_changeIndex + 1) / 2 - 1;
            var leftChildIndex = (_changeIndex * 2) + 1;
            if (
                leftChildIndex > Count - 1
                || HeapComparer.Compare(_elements[parentIndex], _elements[_changeIndex]) > 0
            )
            {
                return true;
            }
            return false;
        }

        private void BubbleUp(int startIndex)
        {
            while (startIndex > 0)
            {
                int parentPosition = (startIndex + 1) / 2 - 1;
                if (Swap(startIndex, parentPosition))
                {
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
                int bestChildIndex = GetBestChildIndex(leftChildIndex, rightChildIndex);
                if (Swap(bestChildIndex, rootIndex))
                {
                    rootIndex = bestChildIndex;
                    leftChildIndex = (rootIndex * 2) + 1;
                }
                else
                {
                    break;
                }
            }
            if (leftChildIndex < _elements.Count)
            {
                Swap(leftChildIndex, rootIndex);
            }
        }

        private bool Swap(int childIndex, int parentIndex)
        {
            if (
                parentIndex >= 0
                && childIndex < _elements.Count
                && HeapComparer.Compare(_elements[parentIndex], _elements[childIndex]) > 0
            )
            {
                (_elements[parentIndex], _elements[childIndex]) = (
                    _elements[childIndex],
                    _elements[parentIndex]
                );
                return true;
            }
            return false;
        }

        private int GetBestChildIndex(int leftChildIndex, int rightChildIndex)
        {
            return HeapComparer.Compare(_elements[leftChildIndex], _elements[rightChildIndex]) > 0
                ? rightChildIndex
                : leftChildIndex;
        }
    }
}
