using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChronoECS.Core;
using System;

namespace ChronoECS.Tests
{
    public struct TestComponent
    {
        public int Value;
    }

    // Fake Awake-system
    public class FakeAwakeSystem : IAwakeSystem
    {
        public bool WasAwaken { get; private set; }

        public void Awake(World world)
        {
            WasAwaken = true;
        }
    }

    // Fake Update-system
    public class FakeUpdateSystem : IUpdateSystem
    {
        public int UpdateCount { get; private set; }

        public void Update(World world, float deltaTime)
        {
            UpdateCount++;
        }
    }

    [TestClass]
    public class SystemRunnerTests
    {
        [TestMethod]
        public void Init_CallsAllAwakeSystemsOnce()
        {
            // Arrange
            var world = new World();
            var awakeSys = new FakeAwakeSystem();
            var runner = SystemRunner.For(world)
                                     .Add((IAwakeSystem)awakeSys);

            // Act
            runner.Init();

            // Assert
            Assert.IsTrue(awakeSys.WasAwaken, "Awake should be called on the system during Init()");
        }

        [TestMethod]
        public void Update_CallsAllUpdateSystemsEachFrame()
        {
            // Arrange
            var world = new World();
            var updateSys = new FakeUpdateSystem();
            var runner = SystemRunner.For(world)
                                     .Add((IUpdateSystem)updateSys)
                                     .Init();

            // Act
            runner.Update(0.1f);
            runner.Update(0.2f);
            runner.Update(0.3f);

            // Assert
            Assert.AreEqual(3, updateSys.UpdateCount, "Update should be called once per frame on each IUpdateSystem");
        }
    }

    [TestClass]
    public class WorldTests
    {
        [TestMethod]
        public void RegisterStorage_GetStorage_ReturnsEmptySparseSet()
        {
            // Arrange
            var world = new World();

            // Act
            world.RegisterStorage<TestComponent>();
            var storage = world.GetStorage<TestComponent>();

            // Assert
            Assert.IsNotNull(storage);
            Assert.AreEqual(0, storage.Count, "Newly registered storage should be empty");
        }

        [TestMethod]
        public void AddAndGetComponent_WorldStorage_WorksCorrectly()
        {
            // Arrange
            var world = new World();
            world.RegisterStorage<TestComponent>();
            var storage = world.GetStorage<TestComponent>();
            var entity = world.EntityMgr.Create();

            // Act
            storage.Add(entity.Index, new TestComponent { Value = 42 });
            bool found = storage.TryGetValue(entity.Index, out var comp);

            // Assert
            Assert.IsTrue(found, "Component should be retrievable after Add()");
            Assert.AreEqual(42, comp.Value, "Retrieved component must match the one stored");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetStorage_WithoutRegister_ThrowsException()
        {
            // Arrange
            var world = new World();

            // Act
            _ = world.GetStorage<TestComponent>();

            // Assert is handled by ExpectedException
        }
    }
}
