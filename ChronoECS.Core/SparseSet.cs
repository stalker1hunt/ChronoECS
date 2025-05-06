// src/ChronoECS.Core/SparseSet.cs
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// A sparse set implementation for fast lookup and iteration
    /// of components of type T by entity index.
    /// </summary>
    public class SparseSet<T> : IEnumerable<(int Entity, T Component)>
        where T : struct
    {
        private int[] _sparse = Array.Empty<int>();
        private int[] _dense = Array.Empty<int>();
        private T[]   _data  = Array.Empty<T>();
        private int   _count;

        /// <summary>Gets the number of stored components.</summary>
        public int Count => _count;

        /// <summary>
        /// Adds or replaces the component for the given entity.
        /// </summary>
        public void Add(int entity, T component)
        {
            // Забезпечуємо, що масиви достатньої довжини
            if (entity >= _sparse.Length)
            {
                int newSize = Math.Max(entity + 1, _sparse.Length * 2);
                Array.Resize(ref _sparse, newSize);
            }

            // Якщо існує, оновлюємо
            int idx = _sparse[entity];
            if (idx < _count && _dense[idx] == entity)
            {
                _data[idx] = component;
                return;
            }

            // Інакше вставляємо новий
            if (_count == _dense.Length)
            {
                int newSize = Math.Max(4, _count * 2);
                Array.Resize(ref _dense,  newSize);
                Array.Resize(ref _data,   newSize);
            }

            idx = _count++;
            _sparse[entity] = idx;
            _dense[idx] = entity;
            _data[idx]  = component;
        }


        /// <summary>
        /// Removes the component for the given entity, if present.
        /// Returns true if removed.
        /// </summary>
        public bool Remove(int entity)
        {
            if (entity < 0 || entity >= _sparse.Length)
                return false;

            int idx = _sparse[entity];
            if (idx >= _count || _dense[idx] != entity)
                return false;

            // swap last element into idx
            int lastEntity      = _dense[_count - 1];
            _dense[idx]         = lastEntity;
            _data[idx]          = _data[_count - 1];
            _sparse[lastEntity] = idx;

            _count--;
            return true;
        }

        /// <summary>
        /// Tries to get the component for the given entity.
        /// Returns true if found.
        /// </summary>
        public bool TryGetValue(int entity, out T component)
        {
            component = default;
            if (entity < 0 || entity >= _sparse.Length) 
                return false;
            int idx = _sparse[entity];
            if (idx < _count && _dense[idx] == entity)
            {
                component = _data[idx];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ensures internal arrays can hold at least (entity + 1) entries.
        /// </summary>
        private void EnsureCapacity(int entity)
        {
            if (entity < _sparse.Length) return;

            int newSize = Math.Max(entity + 1, _sparse.Length * 2);
            Array.Resize(ref _sparse, newSize);
            Array.Resize(ref _dense,  newSize);
            Array.Resize(ref _data,   newSize);
        }

        /// <summary>
        /// Returns an enumerator over all stored (entity, component) pairs.
        /// </summary>
        public IEnumerator<(int Entity, T Component)> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return (_dense[i], _data[i]);
        }
        
        /// <summary>
        /// Enumerates all (entityIndex, component) pairs currently stored.
        /// </summary>
        public IEnumerable<(int entity, T component)> GetAll()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return (_dense[i], _data[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
