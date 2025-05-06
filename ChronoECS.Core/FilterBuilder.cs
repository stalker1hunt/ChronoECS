using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChronoECS.Core
{
    /// <summary>
    /// Builder for queries: With, Without, WithAny.
    /// </summary>
    public class FilterBuilder : IEnumerable<Entity>
    {
        private readonly World _world;
        private readonly List<Type> _all  = new();
        private readonly List<Type> _none = new();
        private readonly List<Type> _any  = new();

        public FilterBuilder(World world)
            => _world = world ?? throw new ArgumentNullException(nameof(world));

        public FilterBuilder With<T>() where T : struct
        {
            _all.Add(typeof(T));
            return this;
        }

        public FilterBuilder Without<T>() where T : struct
        {
            _none.Add(typeof(T));
            return this;
        }

        public FilterBuilder WithAny<T>() where T : struct
        {
            _any.Add(typeof(T));
            return this;
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            IEnumerable<int> candidates;

            // 1) start
            if (_all.Count > 0)
            {
                var first = _all
                    .OrderBy(t => _world.GetFilterStorage(t).Count)
                    .First();
                candidates = _world.GetFilterStorage(first).EntityIndices;
            }
            else
            {
                candidates = Enumerable
                    .Range(0, _world.EntityMgr.Count)
                    .Where(i => _world.EntityMgr.IsAlive(
                        new Entity(i, _world.EntityMgr.GetGeneration(i))));
            }

            // 2) filter
            foreach (var idx in candidates)
            {
                // all
                if (_all.Any(t => !_world.GetFilterStorage(t).Has(idx)))
                    continue;
                // none
                if (_none.Any(t => _world.GetFilterStorage(t).Has(idx)))
                    continue;
                // any
                if (_any.Count > 0 && !_any.Any(t => _world.GetFilterStorage(t).Has(idx)))
                    continue;

                yield return new Entity(idx, _world.EntityMgr.GetGeneration(idx));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class WorldExtensions
    {
        /// <summary> Shortcut: world.Filter().With<A>().Without<B>().WithAny<C>(). </summary>
        public static FilterBuilder Filter(this World world)
            => new FilterBuilder(world);
    }
}
