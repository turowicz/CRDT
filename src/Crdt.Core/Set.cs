using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Crdt.Core
{
    public class Set<T> : ISet<T>
    {
        readonly ConcurrentBag<T> _payload = new ConcurrentBag<T>();

        public void Add(T element)
        {
            _payload.Add(element);
        }

        public ISet<T> Merge(ISet<T> set)
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
            var set = obj as ISet<T>;

            if (set == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (this.Any(element => !set.Contains(element)))
            {
                return -1;
            }

            if (set.Any(element => !this.Contains(element)))
            {
                return 1;
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
