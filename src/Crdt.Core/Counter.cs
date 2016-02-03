using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Crdt.Core
{
    [Serializable]
    public class Counter : ICounter
    {
        readonly ConcurrentBag<UInt16> _payload = new ConcurrentBag<UInt16>();

        public void Increment()
        {
            _payload.Add(1);
        }

        public long Value
        {
            get { return _payload.Sum(x => x); }
        }

        public ICounter Merge(ICounter y)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            var result = new Counter();

            for (var i = 0; i < Value + y.Value; i++)
            {
                result.Increment();
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            var y = obj as ICounter;

            if (y == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Value.CompareTo(y.Value);
        }
    }
}
