using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChronoECS.Core;
using System.Linq;

namespace ChronoECS.Tests
{
    public struct A { public int X; }
    public struct B { public int Y; }
    public struct C { public int Z; }

    [TestClass]
    public class FilterBuilderTests
    {
        /// <summary>
        /// Populates the given world by registering storages and
        /// adding the specified components to their entities.
        /// </summary>
        private void Populate(World world, params (Entity e, object comp)[] entries)
        {
            world.RegisterStorage<A>();
            world.RegisterStorage<B>();
            world.RegisterStorage<C>();

            foreach (var (e, comp) in entries)
            {
                if (comp is A a) world.GetStorage<A>().Add(e.Index, a);
                if (comp is B b) world.GetStorage<B>().Add(e.Index, b);
                if (comp is C c) world.GetStorage<C>().Add(e.Index, c);
            }
        }

        [TestMethod]
        public void WithAndWithout_Works()
        {
            // Arrange
            var world = new World();

            // create entity
            var e1 = world.EntityMgr.Create();
            var e2 = world.EntityMgr.Create();
            var e3 = world.EntityMgr.Create();
            var e4 = world.EntityMgr.Create();
            var e5 = world.EntityMgr.Create();
            var e6 = world.EntityMgr.Create();

            // fill components for entity
            Populate(world,
                (e1, new A { X = 1 }),
                (e2, new B { Y = 2 }),
                (e3, new A { X = 3 }),
                (e4, new A { X = 4 }),
                (e5, new B { Y = 5 }),
                (e6, new C { Z = 6 })
            );

            // add two tests entity
            var e7 = world.EntityMgr.Create();
            world.GetStorage<A>().Add(e7.Index, new A { X = 9 });

            var e8 = world.EntityMgr.Create();
            world.GetStorage<A>().Add(e8.Index, new A { X = 9 });
            world.GetStorage<B>().Add(e8.Index, new B { Y = 9 });

            // Act
            var list = world
                .Filter()
                .With<A>()
                .Without<C>()
                .ToList();

            // Assert
         
            Assert.IsTrue(list.All(ent => 
                world.GetStorage<A>().TryGetValue(ent.Index, out _)
                && !world.GetStorage<C>().TryGetValue(ent.Index, out _)
            ));
        }

        [TestMethod]
        public void WithAny_Works()
        {
            // Arrange
            var world = new World();

            var e1 = world.EntityMgr.Create();
            var e2 = world.EntityMgr.Create();
            var e3 = world.EntityMgr.Create();

            Populate(world,
                (e1, new A { X = 1 }),
                (e2, new B { Y = 2 }),
                (e3, new C { Z = 3 })
            );

            // Act
            var list = world
                .Filter()
                .WithAny<A>()
                .WithAny<C>()
                .ToList();

            // Assert: 
            var expected = new[] { e1.Index, e3.Index };
            var actual   = list.Select(e => e.Index).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
