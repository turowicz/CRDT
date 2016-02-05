using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Crdt.Core.Sets
{
    public class Set<T> : Abstract.Interfaces.ISet<T>
    {
        readonly HashSet<T> _payload = new HashSet<T>();

        public void Add(T element)
        {
            _payload.Add(element);
        }

        public Abstract.Interfaces.ISet<T> Merge(Abstract.Interfaces.ISet<T> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            var result = new Set<T>();

            foreach (var element in this)
            {
                result.Add(element);
            }

            foreach (var element in set)
            {
                result.Add(element);
            }

            return result;
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
            return _payload.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
