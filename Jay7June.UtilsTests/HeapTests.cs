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
        public void UpdateCapacity()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            heap.Capacity = -1;
            Assert.AreEqual(10, heap.Capacity);
            heap.Pop();
            heap.Capacity = -1;
            Assert.AreEqual(10, heap.Capacity);
            heap.Peek();
            heap.Capacity = -1;
            Assert.AreEqual(9, heap.Capacity);
            heap.Capacity = 100;
            Assert.AreEqual(100, heap.Capacity);
        }

        [TestMethod()]
        public void AddTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            Assert.AreEqual(list.Count, heap.Count);
            heap.Add(-1);
            list.Add(-1);
            Assert.AreEqual(-1, heap.Peek());
            Assert.AreEqual(list.Count, heap.Count);
            heap.Add(8);
            list.Add(8);
            Assert.AreEqual(-1, heap.Peek());
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void AddTest_AfterRemove()
        {
            var comparer = Comparer<int>.Default;
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var list1 = new List<int>(list);
            //BubbleUp
            var heap = new Heap<int>(list1, comparer);
            Assert.AreEqual(list1.Count, heap.Count);
            heap.Remove(9);
            list1.Remove(9);
            Assert.AreEqual(list1.Count, heap.Count);
            Assert.AreEqual(1, heap.Peek());
            heap.Add(-1);
            list1.Add(-1);
            Assert.AreEqual(list1.Count, heap.Count);
            Assert.AreEqual(-1, heap.Peek());
            HeapCheck(heap, list1);

            //BubbleDown
            var list2 = new List<int>(list);
            var heap2 = new Heap<int>(list2, comparer);
            heap2.Remove(9);
            list2.Remove(9);
            heap2.Add(100);
            list2.Add(100);
            Assert.AreEqual(1, heap2.Peek());
            HeapCheck(heap2, list2);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            heap.Remove(1);
            list.Remove(1);
            Assert.AreEqual(list.Count, heap.Count);
            Assert.AreEqual(2, heap.Peek());
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void RemoveTest_AfterRemove()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            heap.Remove(1);
            list.Remove(1);
            Assert.AreEqual(list.Count, heap.Count);
            heap.Remove(13);
            list.Remove(13);
            Assert.AreEqual(2, heap.Peek());
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void PopTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            Assert.AreEqual(1, heap.Pop());
            list.Remove(1);
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void PopTest_AfterHeapChange()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            heap.Remove(9);
            list.Remove(9);
            Assert.AreEqual(1, heap.Pop());
            list.Remove(1);
            Assert.AreEqual(2, heap.Pop());
            list.Remove(2);
            heap.Add(-1);
            list.Add(-1);
            Assert.AreEqual(-1, heap.Pop());
            list.Remove(-1);
            HeapCheck(heap, list);
            heap.Add(100);
            Assert.AreEqual(100, heap.Pop());
        }

        [TestMethod()]
        public void PopTest_HeapIsEmpty_ThrowInvalidOperationException()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.ThrowsException<InvalidOperationException>(() => heap.Pop());
        }

        [TestMethod()]
        public void PeekTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list.Count, Comparer<int>.Default);
            var newList = new List<int>();
            list.ForEach(x =>
            {
                newList.Add(x);
                lock (heap)
                {
                    heap.Add(x);
                }
                var value = newList.Min();
                Assert.AreEqual(value, heap.Peek());
            });
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void PeekTest_AfterHeapChange()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            Assert.AreEqual(1, heap.Peek());
            heap.Pop();
            list.Remove(1);
            Assert.AreEqual(2, heap.Peek());
            heap.Remove(45);
            list.Remove(45);
            Assert.AreEqual(2, heap.Peek());
            heap.Add(100);
            list.Add(100);
            Assert.AreEqual(2, heap.Peek());
            heap.Remove(15);
            list.Remove(15);
            Assert.AreEqual(2, heap.Peek());
            heap.Add(-1);
            list.Add(-1);
            Assert.AreEqual(-1, heap.Peek());
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void PeekTest_HeapIsEmpty_ThrowInvalidOperationException()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.ThrowsException<InvalidOperationException>(() => heap.Peek());
        }

        [TestMethod()]
        public void ClearTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
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
            var list1 = new List<int>(list);
            var heap = new Heap<int>(list1.Count, comparer);
            list1.ForEach(x =>
            {
                lock (heap)
                {
                    heap.Add(x);
                }
            });
            Assert.AreEqual(99, heap.Peek());
            heap.Remove(99);
            list1.Remove(99);
            Assert.AreEqual(list1.Count, heap.Count);
            heap.Clear();
            Assert.AreEqual(0, heap.Count);

            var list2 = new List<int>(list);
            heap = new Heap<int>(list2, comparer);
            HeapCheck(heap, list2);

            var list3 = new List<int>(list);
            var heap1 = new Heap<int>(list3, comparer);
            heap1.Remove(41);
            list3.Remove(41);
            Assert.AreEqual(99, heap1.Peek());
            heap1.Add(100);
            list3.Add(100);
            Assert.AreEqual(100, heap1.Peek());
            HeapCheck(heap1, list3);

            var list4 = new List<int>(list);
            var heap2 = new Heap<int>(list4, comparer);
            heap2.Remove(41);
            list4.Remove(41);
            Assert.AreEqual(99, heap2.Peek());
            heap2.Add(-1);
            list4.Add(-1);
            Assert.AreEqual(99, heap2.Peek());
            heap2.Remove(45);
            list4.Remove(45);
            Assert.AreEqual(99, heap2.Peek());
            heap2.Add(100);
            list4.Add(100);
            Assert.AreEqual(100, heap2.Peek());
            HeapCheck(heap2, list4);
        }

        [TestMethod()]
        public void HeapTest_Init()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(Comparer<int>.Default);
            list.ForEach(x =>
            {
                heap.Add(x);
            });
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void HeapTest_InitWithList()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void HeapTest_InitWithCapacity()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list.Count, Comparer<int>.Default);
            Assert.AreEqual(list.Count, heap.Capacity);
            Assert.AreEqual(0, heap.Count);
            list.ForEach(x =>
            {
                heap.Add(x);
            });
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void TryPopTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            Assert.IsTrue(heap.TryPop(out int popItem));
            Assert.AreEqual(1, popItem);
            list.Remove(1);
            Assert.AreEqual(list.Count, heap.Count);
            HeapCheck(heap, list);
            heap.Add(100);
            Assert.IsTrue(heap.TryPop(out popItem));
            Assert.AreEqual(100, popItem);
        }

        [TestMethod()]
        public void TryPopTest_AfterHeapChange()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            heap.Remove(9);
            list.Remove(9);
            Assert.IsTrue(heap.TryPop(out int popItem));
            Assert.AreEqual(1, popItem);
            list.Remove(1);
            Assert.IsTrue(heap.TryPop(out popItem));
            Assert.AreEqual(2, popItem);
            list.Remove(2);
            heap.Add(-1);
            list.Add(-1);
            Assert.IsTrue(heap.TryPop(out popItem));
            Assert.AreEqual(-1, popItem);
            list.Remove(-1);
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void TryPopTest_HeapIsEmpty_ReturnFalse()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.IsFalse(heap.TryPop(out _));
            heap.Add(100);
            heap.Remove(100);
            Assert.IsFalse(heap.TryPop(out _));
        }

        [TestMethod()]
        public void TryPeekTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list.Count, Comparer<int>.Default);
            var newList = new List<int>();
            list.ForEach(x =>
            {
                newList.Add(x);
                lock (heap)
                {
                    heap.Add(x);
                }
                var value = newList.Min();
                Assert.IsTrue(heap.TryPeek(out int peekItem));
                Assert.AreEqual(value, peekItem);
            });
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void TryPeekTest_AfterHeapChange()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list, Comparer<int>.Default);
            Assert.IsTrue(heap.TryPeek(out int peekItem));
            Assert.AreEqual(1, peekItem);
            heap.Pop();
            list.Remove(1);
            Assert.IsTrue(heap.TryPeek(out peekItem));
            Assert.AreEqual(2, peekItem);
            heap.Remove(45);
            list.Remove(45);
            Assert.IsTrue(heap.TryPeek(out peekItem));
            Assert.AreEqual(2, peekItem);
            heap.Add(100);
            list.Add(100);
            Assert.IsTrue(heap.TryPeek(out peekItem));
            Assert.AreEqual(2, peekItem);
            heap.Remove(15);
            list.Remove(15);
            Assert.IsTrue(heap.TryPeek(out peekItem));
            Assert.AreEqual(2, peekItem);
            heap.Add(-1);
            list.Add(-1);
            Assert.IsTrue(heap.TryPeek(out peekItem));
            Assert.AreEqual(-1, peekItem);
            HeapCheck(heap, list);
        }

        [TestMethod()]
        public void TryPeekTest_HeapIsEmpty_ReturnFalse()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.IsFalse(heap.TryPeek(out _));
            heap.Add(100);
            heap.Remove(100);
            Assert.IsFalse(heap.TryPeek(out _));
        }

        #region Helper
        private static void HeapCheck<T>(Heap<T> heap, List<T> elements)
        {
            if (heap.HeapComparer.Equals(Comparer<T>.Default))
            {
                elements = elements.OrderBy(x => x).ToList();
            }
            else
            {
                elements = elements.OrderByDescending(x => x).ToList();
            }
            if (heap == null)
            {
                Assert.Fail("The heap is null");
            }
            foreach (var item in elements)
            {
                T popVal;
                lock (heap)
                {
                    popVal = heap.Pop();
                }
                Assert.AreEqual(item, popVal);
            }
        }
        #endregion
    }
}
