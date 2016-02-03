using System;
using System.Collections.Generic;

namespace Crdt.Core
{
    public interface ISet<T> : IComparable, IEnumerable<T>
    {
        void Add(T element);

        ISet<T> Merge(ISet<T> set);
    }
}