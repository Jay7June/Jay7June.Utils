using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jay7June.Utils.Tests
{
    [TestClass()]
    public class HeapTests
    {
        [TestMethod()]
        public void CtorTest()
        {
            Assert.AreEqual(0, new Heap<int>().Count);
            Assert.AreEqual(5, new Heap<int>(new[] { 15, 2, 13, 5, 89 }).Count);
            Heap<int> heap = new Heap<int>(30);
            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(30, heap.Capacity);
        }

        [TestMethod()]
        public void CtorTest_WithComparer()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45 };
            var heap = new Heap<int>(list, Comparer<int>.Create((x, y) => y - x));
            AssertHeapSquence(heap, list.OrderByDescending(i => i));
        }

        [TestMethod()]
        public void CapacityTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50 };
            var heap = new Heap<int>(list);
            Assert.AreEqual(10, heap.Count);
            Assert.AreEqual(10, heap.Capacity);
            heap.Capacity = 100;
            Assert.AreEqual(100, heap.Capacity);
            Assert.AreEqual(10, heap.Count);
            heap.Capacity = 20;
            Assert.AreEqual(20, heap.Capacity);
        }

        [TestMethod()]
        public void CapacityTest_SetAfterRemove()
        {
            var list = new List<int> { 1, 2 };
            var heap = new Heap<int>(list);
            heap.Pop();
            heap.Capacity = 1;
            Assert.AreEqual(1, heap.Capacity);
        }

        [TestMethod()]
        public void AddTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97 };
            var heap = new Heap<int>(list);
            heap.Add(-1);
            Assert.AreEqual(-1, heap.Peek());
            heap.Add(8);
            Assert.AreEqual(-1, heap.Peek());
            AssertHeapSquence(heap, list.Concat(new[] { -1, 8 }).OrderBy(i => i));
        }

        [TestMethod()]
        public void AddTest_AfterRemove()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            //BubbleUp
            var heap = new Heap<int>(list);
            heap.Remove(9);
            heap.Add(-1);
            AssertHeapSquence(heap, list.Append(-1).Exclude(9).OrderBy(i => i));

            //BubbleDown
            heap = new Heap<int>(list);
            heap.Remove(9);
            heap.Add(100);
            AssertHeapSquence(heap, list.Append(100).Exclude(9).OrderBy(i => i));
        }

        [TestMethod()]
        public void PopTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>();
            list.ForEach(x =>
            {
                heap.Add(x);
            });
            heap.Remove(9);
            Assert.AreEqual(1, heap.Pop());
            Assert.AreEqual(2, heap.Pop());
            heap.Add(-1);
            Assert.AreEqual(-1, heap.Pop());
            AssertHeapSquence(heap, list.Append(-1).Exclude(9).OrderBy(i => i).Skip(3));
        }

        [TestMethod()]
        public void PopTest_ThrowIfEmpty()
        {
            var heap = new Heap<int>();
            Assert.ThrowsException<InvalidOperationException>(() => heap.Pop());
        }

        [TestMethod()]
        public void TryPopTest()
        {
            var heap = new Heap<int>(new[] { 15 });
            Assert.IsTrue(heap.TryPop(out int popItem));
            Assert.AreEqual(15, popItem);
            Assert.IsFalse(heap.TryPop(out _));
            AssertHeapSquence(heap, Enumerable.Empty<int>());
        }

        [TestMethod()]
        public void PeekTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9 };
            var heap = new Heap<int>(list);
            Assert.AreEqual(1, heap.Peek());
            heap.Pop();
            Assert.AreEqual(2, heap.Peek());
            heap.Remove(5);
            Assert.AreEqual(2, heap.Peek());
            heap.Add(100);
            Assert.AreEqual(2, heap.Peek());
            heap.Remove(15);
            Assert.AreEqual(2, heap.Peek());
            heap.Add(-1);
            Assert.AreEqual(-1, heap.Peek());
        }

        [TestMethod()]
        public void PeekTest_ThrowIfEmpty()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.ThrowsException<InvalidOperationException>(() => heap.Peek());
        }

        [TestMethod()]
        public void TryPeekTest()
        {
            var heap = new Heap<int>();
            Assert.IsFalse(heap.TryPeek(out _));
            heap.Add(15);
            Assert.IsTrue(heap.TryPeek(out int item));
            Assert.AreEqual(15, item);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89 };
            var heap = new Heap<int>(list);
            Assert.IsTrue(heap.Remove(23));
            Assert.AreEqual(list.Count - 1, heap.Count);
            Assert.IsFalse(heap.Remove(24));
            Assert.IsTrue(heap.Remove(13));
            AssertHeapSquence(heap, list.Exclude(23).Exclude(13).OrderBy(i => i));
        }

        [TestMethod()]
        public void ClearTest()
        {
            var list = new List<int> { 15, 2, 13, 5, 89, 2, 1, 9, 23, 50, 41, 42, 43, 44, 45, 99, 97, 89, 39, 29, 79, 91, 90, 93, 95, 99, 99, 99, 99, 99, 99 };
            var heap = new Heap<int>(list);
            Assert.AreEqual(list.Count, heap.Count);
            heap.Clear();
            Assert.AreEqual(0, heap.Count);
            Assert.IsFalse(heap.TryPeek(out _));
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(9)]
        [DataRow(16)]
        [DataRow(30)]
        public void Remove_Test(int item)
        {
            var list = new List<int> { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 99 };
            var heap = new Heap<int>(list);
            Assert.IsTrue(heap.Remove(item));
            Assert.IsFalse(heap.Remove(item));
            Assert.AreEqual(list.Count - 1, heap.Count);
            Assert.IsTrue(heap.Remove(99));
            Assert.IsTrue(heap.Remove(-1));
            AssertHeapSquence(heap, list.Exclude(item).Exclude(99).Exclude(-1).OrderBy(i => i));
        }

        #region Helper
        private static void AssertHeapSquence<T>(Heap<T> heap, IEnumerable<T> sorted)
        {
            var list = heap.PopAll();
            if (!list.SequenceEqual(sorted))
            {
                Assert.Fail($"The heap sequence is not expected. \r\nExpect: {string.Join(",", sorted)} \r\nBut:    {string.Join(",", list)}");
            }
        }
        #endregion
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> values, T element)
            where T : IEquatable<T>
        {
            bool excluded = false;
            foreach (var item in values)
            {
                if (!excluded && item.Equals(element))
                {
                    excluded = true;
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}
