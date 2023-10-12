using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Jay7June.Utils
{
    public class Heap<T>
    {
        readonly List<T> _elements;
        private int _dirty = -1;
        public IComparer<T> Comparer { get; }

        public int Count
        {
            get { return _dirty < 0 ? _elements.Count : _elements.Count - 1; }
        }

        public int Capacity
        {
            get { return _elements.Capacity; }
            set
            {
                if (_dirty >= 0)
                {
                    _elements[_dirty] = _elements[_elements.Count - 1];
                    _elements.RemoveAt(_elements.Count - 1);
                    BubbleUp(_dirty);
                    BubbleDown(_dirty);
                    _dirty = -1;
                }
                _elements.Capacity = value;
            }
        }

        public Heap(IComparer<T>? comparer = null)
        {
            _elements = new List<T>();
            Comparer = comparer ?? Comparer<T>.Default;
        }

        public Heap(int capacity, IComparer<T>? comparer = null)
        {
            _elements = new List<T>(capacity);
            Comparer = comparer ?? Comparer<T>.Default;
        }

        public Heap(IEnumerable<T> elements, IComparer<T>? comparer = null)
        {
            _elements = new List<T>(elements);
            Comparer = comparer ?? Comparer<T>.Default;
            for (int i = 1; i < _elements.Count; i++)
            {
                BubbleUp(i);
            }
        }

        public void Add(T item)
        {
            if (_dirty >= 0)
            {
                _elements[_dirty] = item;
            }
            else
            {
                _dirty = _elements.Count;
                _elements.Add(item);
            }
            BubbleUp(_dirty);
            BubbleDown(_dirty);
            _dirty = -1;
        }

        public bool Remove(T item)
        {
            if (_dirty >= 0)
            {
                _elements[_dirty] = _elements[_elements.Count - 1];
                _elements.RemoveAt(_elements.Count - 1);
                BubbleUp(_dirty);
                BubbleDown(_dirty);
                _dirty = -1;
            }
            //int i = _elements.IndexOf(item);
            int i = FindItemIndex(item);
            _dirty = i;
            return i >= 0;
        }

        public T Pop()
        {
            if (_elements.Count > 1)
            {
                if (_dirty >= 0)
                {
                    _elements[_dirty] = _elements[_elements.Count - 1];
                    _elements.RemoveAt(_elements.Count - 1);
                    BubbleUp(_dirty);
                    BubbleDown(_dirty);
                }
                T rst = _elements[0];
                _dirty = 0;
                return rst;
            }
            else if (_elements.Count == 1 && _dirty < 0)
            {
                _dirty = 0;
                return _elements[0];
            }
            throw new InvalidOperationException();
        }

        public T[] PopAll()
        {
            var arr = new T[Count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Pop();
            }
            return arr;
        }

        public bool TryPop([MaybeNullWhen(false)] out T item)
        {
            if (_elements.Count > 1)
            {
                if (_dirty >= 0)
                {
                    _elements[_dirty] = _elements[_elements.Count - 1];
                    _elements.RemoveAt(_elements.Count - 1);
                    BubbleUp(_dirty);
                    BubbleDown(_dirty);
                }
                item = _elements[0];
                _dirty = 0;
                return true;
            }
            else if (_elements.Count == 1 && _dirty < 0)
            {
                _dirty = 0;
                item = _elements[0];
                return true;
            }
            item = default;
            return false;
        }

        public T Peek()
        {
            if (_dirty != 0 && _elements.Count > 0)
            {
                return _elements[0];
            }
            else if (_dirty == 0 && _elements.Count > 1)
            {
                _elements[_dirty] = _elements[_elements.Count - 1];
                _elements.RemoveAt(_elements.Count - 1);
                BubbleUp(_dirty);
                BubbleDown(_dirty);
                _dirty = -1;
                return _elements[0];
            }
            throw new InvalidOperationException();
        }

        public bool TryPeek([MaybeNullWhen(false)] out T item)
        {
            if (_dirty != 0 && _elements.Count > 0)
            {
                item = _elements[0];
                return true;
            }
            else if (_dirty == 0 && _elements.Count > 1)
            {
                _elements[_dirty] = _elements[_elements.Count - 1];
                _elements.RemoveAt(_elements.Count - 1);
                BubbleUp(_dirty);
                BubbleDown(_dirty);
                _dirty = -1;
                item = _elements[0];
                return true;
            }
            item = default;
            return false;
        }

        public void Clear()
        {
            _elements.Clear();
            _dirty = -1;
        }

        private void BubbleUp(int startIndex)
        {
            while (startIndex > 0)
            {
                int parentPosition = (startIndex + 1) / 2 - 1;
                if (parentPosition >= 0 && startIndex < _elements.Count && Comparer.Compare(_elements[parentPosition], _elements[startIndex]) > 0)
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
                int bestChildIndex = Comparer.Compare(_elements[leftChildIndex], _elements[rightChildIndex]) > 0 ? rightChildIndex : leftChildIndex;
                if (rootIndex >= 0 && bestChildIndex < _elements.Count && Comparer.Compare(_elements[rootIndex], _elements[bestChildIndex]) > 0)
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
            if (leftChildIndex < _elements.Count && Comparer.Compare(_elements[rootIndex], _elements[leftChildIndex]) > 0)
            {
                var temp = _elements[rootIndex];
                _elements[rootIndex] = _elements[leftChildIndex];
                _elements[leftChildIndex] = temp;
            }
        }

        private int FindItemIndex(T item)
        {
            var queue = new Queue<int>();
            queue.Enqueue(0);
            var totalLayer = (int)Math.Log(_elements.Count, 2);
            while (queue.Count > 0)
            {
                var endLayer = totalLayer;
                var rootIndex = queue.Dequeue();
                #region 比较左链第一个
                if (Comparer.Compare(_elements[rootIndex], item) == 0)
                {
                    return rootIndex;
                }
                else if (Comparer.Compare(_elements[rootIndex], item) > 0)
                {
                    continue;
                }
                else
                {
                    int rightNode = 2 * (rootIndex + 1);
                    if (rightNode < Count)
                    {
                        queue.Enqueue(rightNode);
                    }
                }
                #endregion
                var startLayer = (int)Math.Log(rootIndex + 1, 2);
                var endIndex = (int)Math.Pow(2, endLayer - startLayer) * (rootIndex + 1) - 1;
                if (endIndex >= Count)
                {
                    endLayer--;
                    endIndex = (int)Math.Pow(2, endLayer - startLayer) * (rootIndex + 1) - 1;
                }
                #region 比较左链最后一个
                if (Comparer.Compare(_elements[endIndex], item) == 0)
                {
                    return endIndex;
                }
                else if (Comparer.Compare(_elements[endIndex], item) < 0)
                {
                    while (endLayer > startLayer + 1)
                    {
                        var rightNode = (int)Math.Pow(2, endLayer - startLayer) * (rootIndex + 1);
                        if (rightNode < Count)
                        {
                            queue.Enqueue(rightNode);
                        }
                        endLayer--;
                    }
                    continue;
                }
                #endregion
                int medianLayer;
                //二分比较
                while (endLayer > startLayer + 1)
                {
                    medianLayer = (endLayer + startLayer) / 2;
                    var medianIndex = (int)Math.Pow(2, medianLayer - startLayer) * (rootIndex + 1) - 1;
                    if (Comparer.Compare(_elements[medianIndex], item) == 0)
                    {
                        return medianIndex;
                    }
                    else if (Comparer.Compare(_elements[medianIndex], item) < 0)
                    {
                        int end = medianLayer + 1;
                        while (end > startLayer + 1)
                        {
                            //右子树加入队列
                            var rightNode = (int)Math.Pow(2, end - startLayer) * (rootIndex + 1);
                            if (rightNode < Count)
                            {
                                queue.Enqueue(rightNode);
                            }
                            end--;
                        }
                        rootIndex = medianIndex;
                        startLayer = medianLayer;
                    }
                    else
                    {
                        endLayer = medianLayer;
                    }
                }
            }
            return -1;
        }
    }
}
