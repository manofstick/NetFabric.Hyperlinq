using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using JM.LinqFaster;
using System;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class AnyBenchmarks : BenchmarksBase
    {
        [BenchmarkCategory("Range")]
        [Benchmark(Baseline = true)]
        public bool Linq_Range() => 
            System.Linq.Enumerable.Any(linqRange, _ => true);

        [BenchmarkCategory("LinkedList")]
        [Benchmark(Baseline = true)]
        public bool Linq_LinkedList() => 
            System.Linq.Enumerable.Any(linkedList, _ => true);

        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public bool Linq_Array() => 
            System.Linq.Enumerable.Any(array, _ => true);

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public bool Linq_List() => 
            System.Linq.Enumerable.Any(list, _ => true);

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark(Baseline = true)]
        public bool Linq_Enumerable_Reference() => 
            System.Linq.Enumerable.Any(enumerableReference, _ => true);

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark(Baseline = true)]
        public bool Linq_Enumerable_Value() => 
            System.Linq.Enumerable.Any(enumerableValue, _ => true);

        [BenchmarkCategory("Array")]
        [Benchmark]
        public bool LinqFaster_Array() =>
            array.AnyF(_ => true);

        [BenchmarkCategory("List")]
        [Benchmark]
        public bool LinqFaster_List() =>
            list.AnyF(_ => true);

        [BenchmarkCategory("Range")]
        [Benchmark]
        public bool Hyperlinq_Range() =>
            hyperlinqRange.Any(_ => true);

        [BenchmarkCategory("LinkedList")]
        [Benchmark]
        public bool Hyperlinq_LinkedList() => 
            linkedList.Any(_ => true);

        [BenchmarkCategory("Array")]
        [Benchmark]
        public bool Hyperlinq_Array() =>
            array.Any(_ => true);

        [BenchmarkCategory("List")]
        [Benchmark]
        public bool Hyperlinq_List() => 
            list.Any(_ => true);

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark]
        public bool Hyperlinq_Enumerable_Reference() => 
            enumerableReference
            .AsValueEnumerable()
            .Any(_ => true);

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark]
        public bool Hyperlinq_Enumerable_Value() => 
            enumerableValue
            .AsValueEnumerable<TestEnumerable.Enumerable, TestEnumerable.Enumerable.Enumerator, int>(enumerable => enumerable.GetEnumerator())
            .Any(_ => true);
    }
}
