using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    public class ToArrayBenchmarks : BenchmarksBase
    {
        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Array() => 
            System.Linq.Enumerable.ToArray(array);

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public int[] Linq_List() => 
            System.Linq.Enumerable.ToArray(list);

        [BenchmarkCategory("Range")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Range() => 
            System.Linq.Enumerable.ToArray(linqRange);

        [BenchmarkCategory("Enumerable")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Enumerable() => 
            System.Linq.Enumerable.ToArray(enumerable);

        [BenchmarkCategory("Array")]
        [Benchmark]
        public int[] Hyperlinq_Array() => 
            array.ToArray();

        [BenchmarkCategory("List")]
        [Benchmark]
        public int[] Hyperlinq_List() => 
            list.ToArray();

        [BenchmarkCategory("Range")]
        [Benchmark]
        public int[] Hyperlinq_Range() =>
            hyperlinqRange.ToArray();

        [BenchmarkCategory("Enumerable")]
        [Benchmark]
        public int[] Hyperlinq_Enumerable() =>
            enumerable.ToArray();
    }
}