using ChronoECS.Core;

namespace ChronoECS.Tests
{
    [TestClass]
    public class SparseSetTests
    {
        [TestMethod]
        public void Add_And_TryGetValue_Works()
        {
            var set = new SparseSet<float>();
            set.Add(5, 3.14f);
            Assert.IsTrue(set.TryGetValue(5, out var value));
            Assert.AreEqual(3.14f, value);
            Assert.AreEqual(1, set.Count);
        }
    } 
}
