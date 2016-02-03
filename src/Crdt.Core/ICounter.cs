using System;

namespace Crdt.Core
{
    public interface ICounter : IComparable
    {
        void Increment();

        long Value { get; }

        ICounter Merge(ICounter y);
    }
}
