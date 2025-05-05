using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChronoECS.Core;

namespace ChronoECS.Tests
{
    [TestClass]
    public class ComponentStorageTests
    {
        private EntityManager _mgr;
        private ComponentStorage<int> _storage;

        [TestInitialize]
        public void Init()
        {
            _mgr = new EntityManager();
            _storage = new ComponentStorage<int>();
        }

        [TestMethod]
        public void Add_And_TryGet_Works()
        {
            var e = _mgr.Create();
            _storage.Add(e, 42);

            Assert.IsTrue(_storage.TryGet(e, out var value));
            Assert.AreEqual(42, value);
        }

        [TestMethod]
        public void Remove_Works()
        {
            var e = _mgr.Create();
            _storage.Add(e, 100);
            Assert.IsTrue(_storage.Remove(e));
            Assert.IsFalse(_storage.TryGet(e, out _));
        }

        [TestMethod]
        public void Replace_Component_DoesNotIncreaseCount()
        {
            var e = _mgr.Create();
            _storage.Add(e, 1);
            _storage.Add(e, 2);

            Assert.IsTrue(_storage.TryGet(e, out var value));
            Assert.AreEqual(2, value);
        }

        [TestMethod]
        public void All_Returns_AllPairs()
        {
            var e1 = _mgr.Create();
            var e2 = _mgr.Create();
            _storage.Add(e1, 11);
            _storage.Add(e2, 22);

            var list = new List<(Entity, int)>(_storage.All());
            Assert.AreEqual(2, list.Count);
            CollectionAssert.AreEquivalent(
                new[] { (e1, 11), (e2, 22) },
                list);
        }
    }
}