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
        private readonly Dictionary<Type, object>    _storages       = new();
        private readonly Dictionary<Type, IStorage> _filterStorages = new();

        /// <summary> Register both generic storage and its IStorage view. </summary>
        public void RegisterStorage<T>() where T : struct
        {
            var t = typeof(T);
            if (_storages.ContainsKey(t)) return;

            var ss = new SparseSet<T>();
            _storages[t]        = ss;
            _filterStorages[t]  = ss;
        }

        /// <summary> Get generic storage for reading/writing component data. </summary>
        public SparseSet<T> GetStorage<T>() where T : struct
        {
            if (_storages.TryGetValue(typeof(T), out var inst))
                return (SparseSet<T>)inst;
            throw new InvalidOperationException($"No storage for {typeof(T)}");
        }
        
        /// <summary> Non‐generic access for filters. </summary>
        internal IStorage GetFilterStorage(Type t)
            => _filterStorages[t];
        
        /// <summary>
        /// Creates a query returning all entities that have both T1 and T2.
        /// </summary>
        public EntityQuery<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct
            => new EntityQuery<T1, T2>(this);
    }
}