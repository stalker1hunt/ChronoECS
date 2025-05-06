using System;
using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// Holds EntityManager and component storages, provides access to them.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Manages creation and destruction of entities.
        /// </summary>
        public EntityManager EntityMgr { get; } = new();

        // map from component Type to its storage instance
        private readonly Dictionary<Type, object> _storages = new();

        /// <summary>
        /// Registers a storage for component type T.
        /// </summary>
        public void RegisterStorage<T>() where T : struct
        {
            _storages[typeof(T)] = new SparseSet<T>();
        }

        /// <summary>
        /// Retrieves the storage for component type T.
        /// Throws if not registered.
        /// </summary>
        public SparseSet<T> GetStorage<T>() where T : struct
        {
            if (_storages.TryGetValue(typeof(T), out var inst))
                return (SparseSet<T>)inst;
            throw new InvalidOperationException($"Storage for component {typeof(T)} is not registered.");
        }
        
        /// <summary>
        /// Creates a query returning all entities that have both T1 and T2.
        /// </summary>
        public EntityQuery<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct
            => new EntityQuery<T1, T2>(this);
    }
}