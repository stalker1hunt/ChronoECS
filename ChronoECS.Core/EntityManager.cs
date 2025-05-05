using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// Manages creation, destruction, and liveness checks for entities.
    /// </summary>
    public class EntityManager
    {
        // Tracks the current generation for each index.
        private readonly List<int> _generations = new();

        // Queue of freed indices available for reuse.
        private readonly Queue<int> _freeIndices = new();

        /// <summary>
        /// Creates a new entity or reuses a freed index with its generation.
        /// </summary>
        /// <returns>A fresh Entity handle.</returns>
        public Entity Create()
        {
            int idx;

            if (_freeIndices.Count > 0)
            {
                // Reuse a freed index.
                idx = _freeIndices.Dequeue();
            }
            else
            {
                // Allocate a brand-new index.
                idx = _generations.Count;
                _generations.Add(0);
            }

            return new Entity(idx, _generations[idx]);
        }

        /// <summary>
        /// Returns true if the given entity handle is still valid (has not been destroyed).
        /// </summary>
        public bool IsAlive(Entity e)
            => e.Index >= 0
               && e.Index < _generations.Count
               && _generations[e.Index] == e.Generation;

        /// <summary>
        /// Destroys an entity: increments its generation and queues its index for reuse.
        /// </summary>
        public void Destroy(Entity e)
        {
            if (!IsAlive(e)) return;

            // Bump generation so old handles become invalid.
            _generations[e.Index]++;
            _freeIndices.Enqueue(e.Index);
        }
    }
}