using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChronoECS.Core;

namespace ChronoECS.Tests
{
    [TestClass]
    public class MovementSystemTests
    {
        [TestMethod]
        public void MovementSystem_MovesPositionByVelocity()
        {
            // Arrange
            var world = new World();
            var system = new MovementSystem();

            var e = world.EntityMgr.Create();
            world.RegisterStorage<Position>();
            world.RegisterStorage<Velocity>();

            world.GetStorage<Position>().Add(e.Index, new Position { X = 0, Y = 0 });
            world.GetStorage<Velocity>().Add(e.Index, new Velocity { VX = 1, VY = 2 });

            system.Awake(world);

            system.Update(world, 0.5f);  // 0.5 * (1,2) = (0.5,1)
            system.Update(world, 1.0f);  // (0.5+1, 1+2) = (1.5,3)

            // Assert
            var moved = world.GetStorage<Position>().TryGetValue(e.Index, out var pos);
            Assert.IsTrue(moved);
            Assert.AreEqual(1.5f, pos.X, 1e-5);
            Assert.AreEqual(3.0f, pos.Y, 1e-5);
        }
    }
}