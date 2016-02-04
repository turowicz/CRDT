using System;

namespace Crdt.Core
{
    public interface ICounter : IComparable
    {
        void Increment();

        Int64 Value { get; }

        void Merge(ICounter counter);

        Int64 this[Int32 i] { get; set; }
    }
}
