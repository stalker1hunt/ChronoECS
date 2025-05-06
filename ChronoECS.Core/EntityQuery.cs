using System;
using System.Collections;
using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// Enumerates all entities having both T1 and T2 components.
    /// </summary>
    public class EntityQuery<T1, T2> : IEnumerable<(Entity entity, T1 c1, T2 c2)>
        where T1 : struct
        where T2 : struct
    {
        private readonly World _world;
        private readonly SparseSet<T1> _set1;
        private readonly SparseSet<T2> _set2;

        public EntityQuery(World world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _set1  = world.GetStorage<T1>();
            _set2  = world.GetStorage<T2>();
        }

        public IEnumerator<(Entity entity, T1 c1, T2 c2)> GetEnumerator()
        {
            bool iterateFirstSet = _set1.Count <= _set2.Count;

            if (iterateFirstSet)
            {
                foreach (var (idx, comp1) in _set1.GetAll())
                {
                    if (_set2.TryGetValue(idx, out var comp2))
                    {
                        var entity = new Entity(idx, _world.EntityMgr.GetGeneration(idx));
                        yield return (entity, comp1, comp2);
                    }
                }
            }
            else
            {
                foreach (var (idx, comp2) in _set2.GetAll())
                {
                    if (_set1.TryGetValue(idx, out var comp1))
                    {
                        var entity = new Entity(idx, _world.EntityMgr.GetGeneration(idx));
                        yield return (entity, comp1, comp2);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}