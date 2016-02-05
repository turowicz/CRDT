using System;
using System.Collections.Concurrent;
using Crdt.Abstract.Interfaces;

namespace Crdt.Core.Counters
{
    public class ConcurrentCounter : ICounter
    {
        readonly Int32 _id;
        readonly Int32 _nodes;
        readonly ConcurrentDictionary<Int32, Int64> _payload;

        public ConcurrentCounter(Int32 id, Int32 nodes)
        {
            _id = id;
            _nodes = nodes;
            _payload = new ConcurrentDictionary<Int32, Int64>();

            for (int i = 0; i < _nodes; i++)
            {
                _payload.AddOrUpdate(i, key => 0, (key, value) => 0);
            }
        }

        public void Increment()
        {
            _payload.AddOrUpdate(_id, key => 1, (key, value) => ++value);
        }

        public Int64 Value
        {
            get
            {
                var result = 0l;

                for (int i = 0; i < _nodes; i++)
                {
                    result += this[i];
                }

                return result;
            }
        }

        public ICounter Merge(ICounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }

            for (var i = 0; i < _nodes; i++)
            {
                _payload.AddOrUpdate(i, key => counter[i], (key, value) => Math.Max(value, counter[i]));
            }

            return this;
        }

        public Int32 CompareTo(object obj)
        {
            var counter = obj as ICounter;

            if (counter == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            for (var i = 0; i < _nodes; i++)
            {
                if (this[i] > counter[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        public Int64 this[Int32 i]
        {
            get
            {
                Int64 result;

                if (_payload.TryGetValue(i, out result))
                {
                    return result;
                }

                throw new InvalidOperationException("Getting value failed.");
            }
            set
            {
                throw new InvalidOperationException("Cannot set externally.");
            }
        }
    }
}
