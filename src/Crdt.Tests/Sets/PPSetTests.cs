using System;
using System.Linq;
using Crdt.Abstract.Interfaces;
using Crdt.Core.Sets;
using Machine.Fakes;
using Machine.Specifications;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Crdt.Tests.Sets
{
    public class PPSetTests
    {
        [Subject(typeof(PPSet<Int32>))]
        public abstract class BasePPSetTest
        {
            protected const Int32 N = 100;

            protected static IPPSet<Int32> Subject { get; set; }

            Establish that = () => Subject = new PPSet<int>(new Set<int>(), new Set<int>());
        }

        public class When_adding_an_item : BasePPSetTest
        {
            Because of = () => Subject.Add(N);

            It should_contain_only_n = () => Subject.Single().ShouldEqual(N);
        }

        public class When_removing_an_item : BasePPSetTest
        {
            Establish that = () => Subject.Add(N);

            Because of = () => Subject.Remove(N);

            It should_be_empty = () => Subject.ShouldBeEmpty();
        }

        public class When_adding_N_items : BasePPSetTest
        {
            Because of = () => Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));

            It should_contain_N_elements = () => Subject.LongCount().ShouldEqual(N);
        }

        public class When_removing_all_items : BasePPSetTest
        {
            Establish that = () => Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));

            Because of = () => Subject.ToList().ForEach(x => Subject.Remove(x));

            It should_be_empty = () => Subject.ShouldBeEmpty();
        }

        public class When_merging_smaller_to_bigger_sets : BasePPSetTest
        {
            static IPPSet<Int32> _target;
            static IPPSet<Int32> _merged;

            Establish that = () =>
            {
                _target = new PPSet<int>(new Set<int>(), new Set<int>());
                Enumerable.Range(0, N / 2).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(N, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _merged = Subject.Merge(_target);

            It should_return_sum_of_both_as_value = () => _merged.LongCount().ShouldEqual(N + N / 2);
        }

        public class When_merging_bigger_to_smaller_sets : BasePPSetTest
        {
            static IPPSet<Int32> _target;
            static IPPSet<Int32> _merged;

            Establish that = () =>
            {
                _target = new PPSet<int>(new Set<int>(), new Set<int>());
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(N, N / 2).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _merged = Subject.Merge(_target);

            It should_return_sum_of_both_as_value = () => _merged.LongCount().ShouldEqual(N + N / 2);
        }

        public class When_merging_equal_to_equal_sets : BasePPSetTest
        {
            static IPPSet<Int32> _target;
            static IPPSet<Int32> _merged;

            Establish that = () =>
            {
                _target = new PPSet<int>(new Set<int>(), new Set<int>());
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(N, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _merged = Subject.Merge(_target);

            It should_return_sum_of_both_as_value = () => _merged.LongCount().ShouldEqual(N * 2);
        }

        public class When_comparing_smaller_to_bigger_sets : BasePPSetTest
        {
            static IPPSet<Int32> _target;
            static Int32 _comparison;

            Establish that = () =>
            {
                _target = new PPSet<int>(new Set<int>(), new Set<int>());
                Enumerable.Range(0, N / 2).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _comparison = Subject.CompareTo(_target);

            It should_not_return_0 = () => _comparison.ShouldNotEqual(0);
        }

        public class When_comparing_bigger_to_smaller_sets : BasePPSetTest
        {
            static IPPSet<Int32> _target;
            static Int32 _comparison;

            Establish that = () =>
            {
                _target = new PPSet<int>(new Set<int>(), new Set<int>());
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(0, N / 2).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _comparison = Subject.CompareTo(_target);

            It should_return_0 = () => _comparison.ShouldEqual(0);
        }

        public class When_comparing_equal_to_equal_sets : BasePPSetTest
        {
            static IPPSet<Int32> _target;
            static Int32 _comparison;

            Establish that = () =>
            {
                _target = new PPSet<int>(new Set<int>(), new Set<int>());
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _comparison = Subject.CompareTo(_target);

            It should_return_0 = () => _comparison.ShouldEqual(0);
        }
    }
}
