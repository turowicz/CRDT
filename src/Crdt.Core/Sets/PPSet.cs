using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crdt.Abstract.Interfaces;

namespace Crdt.Core.Sets
{
    public class PPSet<T> : IPPSet<T> where T : IComparable
    {
        public Abstract.Interfaces.ISet<T> AddSet { get; }

        public Abstract.Interfaces.ISet<T> RemoveSet { get; }

        public PPSet(Abstract.Interfaces.ISet<T> addSet, Abstract.Interfaces.ISet<T> removeSet)
        {
            AddSet = addSet;
            RemoveSet = removeSet;
        }

        public void Add(T element)
        {
            AddSet.Add(element);
        }

        public void Remove(T element)
        {
            RemoveSet.Add(element);
        }

        public IPPSet<T> Merge(IPPSet<T> set)
        {
            foreach (var element in set.AddSet)
            {
                Add(element);
            }

            foreach (var element in set.RemoveSet)
            {
                Remove(element);
            }

            return this;
        }

        public int CompareTo(object obj)
        {
            var set = obj as IPPSet<T>;

            if (set == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (AddSet.Any(element => !set.AddSet.Contains(element)))
            {
                return -1;
            }

            if (RemoveSet.Any(element => !set.RemoveSet.Contains(element)))
            {
                return -1;
            }

            return 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return AddSet.Where(element => !RemoveSet.Contains(element)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Abstract.Interfaces.ISet<T> Merge(Abstract.Interfaces.ISet<T> set)
        {
            throw new NotSupportedException("Can't merge instance of Abstract.Interfaces.ISet<T>");
        }
    }
}
