using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jay7June.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jay7June.Utils.Tests
{
    [TestClass()]
    public class HeapTests
    {
        [TestMethod()]
        public void AddTest()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            Assert.AreEqual(list.Count, heap.Count);
            heap.Add(-1);
            Assert.AreEqual(-1, heap.Peek());
            Assert.AreEqual(list.Count + 1, heap.Count);
            heap.Add(8);
            Assert.AreEqual(-1, heap.Peek());
            Assert.AreEqual(list.Count + 2, heap.Count);
            HeapCheck(heap, comparer);
        }

        [TestMethod()]
        public void AddTest_AfterRemove()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            //BubbleUp
            var heap = new Heap<int>(list, comparer);
            Assert.AreEqual(list.Count, heap.Count);
            heap.Remove(9);
            Assert.AreEqual(list.Count, heap.Count + 1);
            Assert.AreEqual(1, heap.Peek());
            heap.Add(-1);
            Assert.AreEqual(list.Count, heap.Count);
            Assert.AreEqual(-1, heap.Peek());
            HeapCheck(heap, comparer);

            //BubbleDown
            var heap2 = new Heap<int>(list, comparer);
            heap2.Remove(9);
            heap2.Add(100);
            Assert.AreEqual(1, heap2.Peek());
            HeapCheck(heap2, comparer);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            heap.Remove(1);
            Assert.AreEqual(list.Count - 1, heap.Count);
            Assert.AreEqual(2, heap.Peek());
            HeapCheck(heap, comparer);
        }

        [TestMethod()]
        public void RemoveTest_AfterRemove()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            heap.Remove(1);
            Assert.AreEqual(list.Count - 1, heap.Count);
            heap.Remove(13);
            Assert.AreEqual(2, heap.Peek());
            HeapCheck(heap, comparer);
        }

        [TestMethod()]
        public void PopTest()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            Assert.AreEqual(1, heap.Pop());
            HeapCheck(heap, comparer);
        }

        [TestMethod()]
        public void PopTest_AfterHeapChange()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            heap.Remove(2);
            Assert.AreEqual(1, heap.Pop());
            Assert.AreEqual(2, heap.Pop());
            heap.Add(0);
            Assert.AreEqual(0, heap.Pop());
            HeapCheck(heap, comparer);
            Assert.ThrowsException<IndexOutOfRangeException>(() => heap.Pop());
        }

        [TestMethod()]
        public void PeekTest()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list.Count, comparer);
            var newList = new List<int>();
            list.ForEach(x =>
            {
                newList.Add(x);
                lock (heap)
                {
                    heap.Add(x);
                }
                var value =
                    comparer.Compare(list.Max(), list.Min()) > 0 ? newList.Min() : newList.Max();
                Assert.AreEqual(value, heap.Peek());
            });
        }

        [TestMethod()]
        public void PeekTest_AfterHeapChange()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            Assert.AreEqual(1, heap.Peek());
            heap.Pop();
            Assert.AreEqual(2, heap.Peek());
            heap.Remove(89);
            Assert.AreEqual(2, heap.Peek());
            HeapCheck(heap, comparer);
            Assert.ThrowsException<IndexOutOfRangeException>(() => heap.Peek());
        }

        [TestMethod()]
        public void ClearTest()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            Assert.AreEqual(list.Count, heap.Count);
            heap.Clear();
            Assert.AreEqual(0, heap.Count);
        }

        [TestMethod()]
        public void HeapTest_Descending()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return y - x;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list.Count, comparer);
            list.ForEach(x =>
            {
                lock (heap)
                {
                    heap.Add(x);
                }
            });
            int peekVal = heap.Peek();
            Assert.AreEqual(99, peekVal);
            heap.Remove(99);
            Assert.AreEqual(list.Count, heap.Count + 1);
            heap.Clear();
            Assert.AreEqual(0, heap.Count);
            heap = new Heap<int>(list, comparer);
            HeapCheck(heap, comparer);

            var heap1 = new Heap<int>(list, comparer);
            heap1.Remove(41);
            Assert.AreEqual(99, heap1.Peek());
            heap1.Add(100);
            Assert.AreEqual(100, heap1.Peek());
            HeapCheck(heap1, comparer);


            var heap2 = new Heap<int>(list, comparer);
            heap2.Remove(41);
            Assert.AreEqual(99, heap2.Peek());
            heap2.Add(-1);
            Assert.AreEqual(99, heap2.Peek());
            HeapCheck(heap2, comparer);

        }

        [TestMethod()]
        public void HeapTest_Init()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(comparer);
            list.ForEach(x =>
            {
                heap.Add(x);
            });
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, comparer);
        }

        [TestMethod()]
        public void HeapTest_InitWithList()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, comparer);
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, comparer);
        }

        [TestMethod()]
        public void HeapTest_InitWithCapacity()
        {
            var comparer = Comparer<int>.Create(
                (x, y) =>
                {
                    return x - y;
                }
            );
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list.Count, comparer);
            list.ForEach(x =>
            {
                heap.Add(x);
            });
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, comparer);
        }

        #region Helper
        private void HeapCheck<T>(Heap<T> heap, Comparer<T> comparer)
        {
            if (heap == null)
            {
                Assert.Fail("The heap is null");
            }
            var pre = heap.Peek();
            var popVal = pre;
            while (heap.Count > 0)
            {
                lock (heap)
                {
                    popVal = heap.Pop();
                }
                Assert.IsTrue(comparer.Compare(pre, popVal) <= 0);
                pre = popVal;
            }
        }
        #endregion
    }
}
