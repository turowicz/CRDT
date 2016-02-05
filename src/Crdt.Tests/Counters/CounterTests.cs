using System;
using System.Linq;
using Crdt.Core.Counters;
using Machine.Specifications;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Crdt.Tests.Counters
{
    public class CounterTests
    {
        [Subject(typeof(Counter))]
        public abstract class BaseCounterTest
        {
            Establish that = () =>
            {
                Subject = new Counter(0, 2);
                Other = new Counter(1, 2);
            };

            protected static Counter Other { get; set; }

            protected static Counter Subject { get; set; }

            protected const Int32 N = 100;

            protected const Int32 Nodes = 2;
        }

        public class When_incrementing_once : BaseCounterTest
        {
            Because of = () => Subject.Increment();

            It should_return_1 = () => Subject.Value.ShouldEqual(1);
        }

        public class When_incrementing_N_times : BaseCounterTest
        {
            Because of = () => Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());

            It should_return_N = () => Subject.Value.ShouldEqual(N);
        }

        public class When_merging_two_counters : BaseCounterTest
        {
            Establish that = () =>
            {
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
                Enumerable.Range(0, N / 2).ToList().ForEach(x => Other.Increment());
            };

            Because of = () => Subject.Merge(Other);

            It should_return_sum_of_both = () => Subject.Value.ShouldEqual(N + N / 2);
        }

        public class When_comparing_two_merged_counters : BaseCounterTest
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

        public class When_comparing_two_umerged_counters : BaseCounterTest
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
