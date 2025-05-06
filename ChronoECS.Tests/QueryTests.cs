using ChronoECS.Core;

namespace ChronoECS.Tests
{
    [TestClass]
    public class QueryTests
    {
        public struct C1 { public int X; }
        public struct C2 { public int Y; }

        [TestMethod]
        public void Query_TwoComponents_YieldsCorrectEntities()
        {
            var world = new World();
            world.RegisterStorage<C1>();
            world.RegisterStorage<C2>();

            var e1 = world.EntityMgr.Create();
            world.GetStorage<C1>().Add(e1.Index, new C1 { X = 5 });
            world.GetStorage<C2>().Add(e1.Index, new C2 { Y = 7 });

            var e2 = world.EntityMgr.Create();
            world.GetStorage<C1>().Add(e2.Index, new C1 { X = 1 });
            // C2 missing

            var results = world.Query<C1, C2>();
            var list = results.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(e1, list[0].entity);
            Assert.AreEqual(5, list[0].c1.X);
            Assert.AreEqual(7, list[0].c2.Y);
        }
    }
}
