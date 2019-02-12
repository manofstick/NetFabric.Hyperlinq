using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    public class SelectToArrayBenchmarks : BenchmarksBase
    {
        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Array() 
            => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(array, item => item));

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public int[] Linq_List() 
            => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(list, item => item));

        [BenchmarkCategory("Range")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Range() 
            => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(linqRange, item => item));

        [BenchmarkCategory("Enumerable")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Enumerable() 
            => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(enumerable, item => item));

        [BenchmarkCategory("Array")]
        [Benchmark]
        public int[] Hyperlinq_Array() 
            => array.Select(item => item).ToArray();

        [BenchmarkCategory("List")]
        [Benchmark]
        public int[] Hyperlinq_List() 
            => list.Select(item => item).ToArray();

        [BenchmarkCategory("Range")]
        [Benchmark]
        public int[] Hyperlinq_Range() 
            => hyperlinqRange.Select(item => item).ToArray();

        [BenchmarkCategory("Enumerable")]
        [Benchmark]
        public int[] Hyperlinq_Enumerable() 
            => enumerable.Select(item => item).ToArray();
    }
}