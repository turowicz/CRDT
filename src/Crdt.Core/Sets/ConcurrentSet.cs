using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Crdt.Core.Sets
{
    public class ConcurrentSet<T> : Abstract.Interfaces.ISet<T>
    {
        readonly ConcurrentDictionary<T, T> _payload = new ConcurrentDictionary<T, T>();

        public void Add(T element)
        {
            _payload.AddOrUpdate(element, key => element, (key, value) => value);
        }

        public Abstract.Interfaces.ISet<T> Merge(Abstract.Interfaces.ISet<T> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            foreach (var element in set)
            {
                Add(element);
            }

            return this;
        }

        public Int32 CompareTo(object obj)
        {
            var set = obj as Abstract.Interfaces.ISet<T>;

            if (set == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (this.Any(element => !set.Contains(element)))
            {
                return -1;
            }

            return 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _payload.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
