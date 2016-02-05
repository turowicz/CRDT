using System;
using System.Linq;
using Crdt.Core.Counters;
using Machine.Specifications;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Crdt.Tests.Counters
{
    public class ConcurrentCounterTests
    {
        [Subject(typeof(ConcurrentCounter))]
        public abstract class BaseConcurrentCounterTest
        {
            Establish that = () =>
            {
                Subject = new ConcurrentCounter(0, 2);
                Other = new ConcurrentCounter(1, 2);
            };

            protected static ConcurrentCounter Other { get; set; }

            protected static ConcurrentCounter Subject { get; set; }

            protected const Int32 N = 100;

            protected const Int32 Nodes = 2;
        }

        public class When_incrementing_once : BaseConcurrentCounterTest
        {
            Because of = () => Subject.Increment();

            It should_return_1 = () => Subject.Value.ShouldEqual(1);
        }

        public class When_incrementing_N_times : BaseConcurrentCounterTest
        {
            Because of = () => Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());

            It should_return_N = () => Subject.Value.ShouldEqual(N);
        }

        public class When_merging_two_counters : BaseConcurrentCounterTest
        {
            Establish that = () =>
            {
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
                Enumerable.Range(0, N / 2).ToList().ForEach(x => Other.Increment());
            };

            Because of = () => Subject.Merge(Other);

            It should_return_sum_of_both = () => Subject.Value.ShouldEqual(N + N / 2);
        }

        public class When_comparing_two_merged_counters : BaseConcurrentCounterTest
        {
            static Int32 comparison;

            Establish that = () =>
            {
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
                Enumerable.Range(0, N).ToList().ForEach(x => Other.Increment());

                Subject.Merge(Other);
                Other.Merge(Subject);
            };

            Because of = () => comparison = Subject.CompareTo(Other);

            It should_return_0 = () => comparison.ShouldEqual(0);
        }

        public class When_comparing_two_umerged_counters : BaseConcurrentCounterTest
        {
            static Int32 comparison;

            Establish that = () =>
            {
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
                Enumerable.Range(0, N).ToList().ForEach(x => Other.Increment());
            };

            Because of = () => comparison = Subject.CompareTo(Other);

            It should_return_negative_1 = () => comparison.ShouldEqual(-1);
        }
    }
}
