using ChronoECS.Core;

namespace ChronoECS.Tests
{
    [TestClass]
    public class EntityManagerTests
    {
        [TestMethod]
        public void Create_TwoEntities_UniqueIndicesAndAlive()
        {
            var mgr = new EntityManager();
            var e1 = mgr.Create();
            var e2 = mgr.Create();

            Assert.AreNotEqual(e1.Index, e2.Index);
            Assert.IsTrue(mgr.IsAlive(e1));
            Assert.IsTrue(mgr.IsAlive(e2));
        }

        [TestMethod]
        public void Destroy_Entity_NotAlive()
        {
            var mgr = new EntityManager();
            var e = mgr.Create();
            Assert.IsTrue(mgr.IsAlive(e));

            mgr.Destroy(e);
            Assert.IsFalse(mgr.IsAlive(e));
        }

        [TestMethod]
        public void Recreate_AfterDestroy_SameIndexDifferentGeneration()
        {
            var mgr = new EntityManager();
            var e1 = mgr.Create();
            mgr.Destroy(e1);

            var e2 = mgr.Create();
            Assert.AreEqual(e1.Index, e2.Index);
            Assert.IsTrue(e2.Generation > e1.Generation);
            Assert.IsFalse(mgr.IsAlive(e1));
            Assert.IsTrue(mgr.IsAlive(e2));
        }
    }
}