using System;

namespace Crdt.Core
{
    public interface ICounter : IComparable
    {
        void Increment();

        Int64 Value { get; }

        ICounter Merge(ICounter counter);
    }
}
