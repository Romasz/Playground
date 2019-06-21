using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace EfficientGuids
{
    [MemoryDiagnoser]
    public class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<Program>();

        private Guid _guid;

        [GlobalSetup]
        public void Setup() => _guid = Guid.NewGuid();

        //[Benchmark(Baseline = true)]
        //public string Base64EncodedGuidOriginal() => Convert.ToBase64String(_guid.ToByteArray())
        //                                                    .Replace("/", "-").Replace("+", "_").Replace("=", "");

        //[Benchmark]
        //public string Base64EncodedGuid() => _guid.EncodeBase64String();

        [Benchmark(Baseline = true)]
        public string Base64EncodedGuidMR() => _guid.EncodeBase64StringMR();

        [Benchmark]
        public string Base64EncodedGuidR() => _guid.EncodeBase64StringR();
    }
}
