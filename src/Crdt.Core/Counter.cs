using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Crdt.Core
{
    public class Counter : ICounter
    {
        readonly ConcurrentBag<UInt16> _payload = new ConcurrentBag<UInt16>();

        public void Increment()
        {
            _payload.Add(1);
        }

        public Int64 Value
        {
            get { return _payload.Sum(x => x); }
        }

        public ICounter Merge(ICounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }

            var result = new Counter();

            for (var i = 0; i < Value + counter.Value; i++)
            {
                result.Increment();
            }

            return result;
        }

        public Int32 CompareTo(object obj)
        {
            var counter = obj as ICounter;

            if (counter == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Value.CompareTo(counter.Value);
        }
    }
}
