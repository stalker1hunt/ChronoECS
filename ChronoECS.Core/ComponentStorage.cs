using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// Stores components of type T indexed by Entity.
    /// Wraps a SparseSet<int,T> for O(1) access.
    /// </summary>
    /// <typeparam name="T">Component type (must be struct).</typeparam>
    public class ComponentStorage<T> where T : struct
    {
        private readonly SparseSet<T> _set = new SparseSet<T>();

        /// <summary>Adds or replaces the component for the given entity.</summary>
        public void Add(Entity entity, T component)
        {
            _set.Add(entity.Index, component);
        }

        /// <summary>Removes the component for the given entity. Returns true if removed.</summary>
        public bool Remove(Entity entity)
        {
            return _set.Remove(entity.Index);
        }

        /// <summary>Tries to get the component for the given entity. Returns true if found.</summary>
        public bool TryGet(Entity entity, out T component)
        {
            return _set.TryGetValue(entity.Index, out component);
        }

        /// <summary>
        /// Enumerates all stored (Entity, Component) pairs.
        /// </summary>
        public IEnumerable<(Entity, T)> All()
        {
            foreach (var (idx, comp) in _set)
                yield return (new Entity(idx, /* dummy generation */ 0), comp);
        }
    }
}