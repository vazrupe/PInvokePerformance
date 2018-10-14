using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;
using System.Windows;

namespace PInvokePerformance
{
    public partial class PerformanceTest
    {
        public int Repeat { get; }
        public ulong Iterations { get; }

        public PerformanceTest(int repeat, ulong iterations)
        {
            Repeat = repeat;
            Iterations = iterations;
        }

        public static double GetAverageRunningTime(int repeat, ulong count, Action<ulong> action)
        {
            if (repeat < 1)
                return 0;

            double sum = 0.0;
            for (int i = 0; i < repeat; i++)
                sum += GetRunningTime(count, action);

            return sum / repeat;
        }

        public static double GetRunningTime(ulong iterSize, Action<ulong> action)
        {
            return GetRunningTime(() => action?.Invoke(iterSize));
        }

        public static double GetRunningTime(Action action)
        {
            var sw = new Stopwatch();

            sw.Start();
            action?.Invoke();
            sw.Stop();

            return sw.Elapsed.TotalMilliseconds;
        }
    }

    public partial class PerformanceTest
    {
        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_Test1(ulong count);

        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_Test2(ulong count);

        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_Test3(ulong count);

        public double BenchmarkUnmanagedTest1()
        {
            return GetAverageRunningTime(Repeat, Iterations, TA_Test1);
        }
        public double BenchmarkUnmanagedTest2()
        {
            return GetAverageRunningTime(Repeat, Iterations, TA_Test2);
        }
        public double BenchmarkUnmanagedTest3()
        {
            return GetAverageRunningTime(Repeat, Iterations, TA_Test3);
        }
    }

    public partial class PerformanceTest
    {
        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_IncrementCounter();

        [DllImport("TraditionalAPI.dll")]
        private static extern double TA_CalculateSquareRoot(double value);

        [DllImport("TraditionalAPI.dll")]
        private static extern double TA_DotProduct(double[] threeTuple1, double[] threeTuple2);

        public double BenchmarkManagedInterfaceTest1()
        {
            ManagedInterface.ManagedInterface managedInterface = new ManagedInterface.ManagedInterface();

            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 1; i <= count; i++)
                    managedInterface.IncrementCounter();
            });
        }
        public double BenchmarkManagedInterfaceTest2()
        {
            ManagedInterface.ManagedInterface managedInterface = new ManagedInterface.ManagedInterface();

            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double d = managedInterface.CalculateSquareRoot((double)i);
                }
            });
        }
        public double BenchmarkManagedInterfaceTest3()
        {
            ManagedInterface.ManagedInterface managedInterface = new ManagedInterface.ManagedInterface();

            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double[] threeTuple1 = new double[] { i + 0.1, i + 0.2, i + 0.3 };
                    double[] threeTuple2 = new double[] { i - 0.7, i - 0.6, i - 0.5 };
                    double d = managedInterface.DotProduct(threeTuple1, threeTuple2);
                }
            });
        }

        public double BenchmarkPInvokeTest1()
        {
            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 1; i <= count; i++)
                    TA_IncrementCounter();
            });
        }
        public double BenchmarkPInvokeTest2()
        {
            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double d = TA_CalculateSquareRoot((double)i);
                }
            });
        }
        public double BenchmarkPInvokeTest3()
        {
            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double[] threeTuple1 = new double[] { i + 0.1, i + 0.2, i + 0.3 };
                    double[] threeTuple2 = new double[] { i - 0.7, i - 0.6, i - 0.5 };
                    double d = TA_DotProduct(threeTuple1, threeTuple2);
                }
            });
        }
    }
    
    public partial class PerformanceTest
    {
        private uint _counter = 0;

        private void CSharpSafeTest1(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
                _counter++;
        }

        private void CSharpSafeTest2(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                var tmp = Math.Sqrt((double)i);
            }
        }

        private void CSharpSafeTest3(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                double[] threeTuple1 = new double[] { i + 0.1, i + 0.2, i + 0.3 };
                double[] threeTuple2 = new double[] { i - 0.7, i - 0.6, i - 0.5 };
                var tmp = threeTuple1[0] * threeTuple2[0] + threeTuple1[1] * threeTuple2[1] + threeTuple1[2] * threeTuple2[2];
            }
        }
        
        private void CSharpUnsafeTest1(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                    _counter++;
            }
        }

        private void CSharpUnsafeTest2(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                {
                    var tmp = Math.Sqrt((double)i);
                }
            }
        }

        private void CSharpUnsafeTest3(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double[] threeTuple1 = new double[] { i + 0.1, i + 0.2, i + 0.3 };
                    double[] threeTuple2 = new double[] { i - 0.7, i - 0.6, i - 0.5 };
                    var tmp = threeTuple1[0] * threeTuple2[0] + threeTuple1[1] * threeTuple2[1] + threeTuple1[2] * threeTuple2[2];
                }
            }
        }

        public double BenchmarkCSharpSafeTest1()
        {
            return GetAverageRunningTime(Repeat, Iterations, CSharpSafeTest1);
        }
        public double BenchmarkCSharpSafeTest2()
        {
            return GetAverageRunningTime(Repeat, Iterations, CSharpSafeTest2);
        }
        public double BenchmarkCSharpSafeTest3()
        {
            return GetAverageRunningTime(Repeat, Iterations, CSharpSafeTest3);
        }

        public double BenchmarkCSharpUnsafeTest1()
        {
            return GetAverageRunningTime(Repeat, Iterations, CSharpUnsafeTest1);
        }
        public double BenchmarkCSharpUnsafeTest2()
        {
            return GetAverageRunningTime(Repeat, Iterations, CSharpUnsafeTest2);
        }
        public double BenchmarkCSharpUnsafeTest3()
        {
            return GetAverageRunningTime(Repeat, Iterations, CSharpUnsafeTest3);
        }
    }

    public partial class PerformanceTest
    {
        public double BenchmarkUnsafePInvokeTest1()
        {
            return GetAverageRunningTime(Repeat, Iterations, PerformanceTestUnsafePInvoke.UnmanagedPInvokeTest1);
        }
        public double BenchmarkUnsafePInvokeTest2()
        {
            return GetAverageRunningTime(Repeat, Iterations, PerformanceTestUnsafePInvoke.UnmanagedPInvokeTest2);
        }
        public double BenchmarkUnsafePInvokeTest3()
        {
            return GetAverageRunningTime(Repeat, Iterations, PerformanceTestUnsafePInvoke.UnmanagedPInvokeTest3);
        }

        public double BenchmarkUnsafePInvokeTest3CallOnly()
        {
            var sources = new List<Tuple<double[], double[]>>((int)Iterations);
            for (ulong i = 1; i <= Iterations; i++)
            {
                var arr = new double[] { i, i, i };
                sources.Add(Tuple.Create(arr, arr));
            }

            return GetAverageRunningTime(Repeat, Iterations, count =>
            {
                for (ulong i = 0; i < Iterations; i++)
                {
                    var source = sources[(int)i];
                    var temp = PerformanceTestUnsafePInvoke.UnmanagedPInvokeTest3CallOnly(source.Item1, source.Item2);
                }
            });
        }
    }

    public static class PerformanceTestUnsafePInvoke
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("TraditionalAPI.dll", EntryPoint = "TA_IncrementCounter")]
        private static extern void TA_IncrementCounter_unmanaged();

        [SuppressUnmanagedCodeSecurity]
        [DllImport("TraditionalAPI.dll", EntryPoint = "TA_CalculateSquareRoot")]
        private static extern double TA_CalculateSquareRoot_unmanaged(double value);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("TraditionalAPI.dll", EntryPoint = "TA_DotProduct")]
        private static extern double TA_DotProduct_unmanaged(double[] threeTuple1, double[] threeTuple2);
        
        public static void UnmanagedPInvokeTest1(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
                TA_IncrementCounter_unmanaged();
        }

        public static void UnmanagedPInvokeTest2(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                double d = TA_CalculateSquareRoot_unmanaged((double)i);
            }
        }

        public static void UnmanagedPInvokeTest3(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                double[] threeTuple1 = new double[] { i + 0.1, i + 0.2, i + 0.3 };
                double[] threeTuple2 = new double[] { i - 0.7, i - 0.6, i - 0.5 };
                double tmp = TA_DotProduct_unmanaged(threeTuple1, threeTuple2);
            }
        }
        
        public static double UnmanagedPInvokeTest3CallOnly(double[] threeTuple1, double[] threeTuple2)
        {
            return TA_DotProduct_unmanaged(threeTuple1, threeTuple2);
        }
    }

    public partial class PerformanceTest
    {
        public void AnotherTest()
        {
            var arr2 = Enumerable.Range(0, 100000000).Select(n => (double)n).ToArray();
            var timeCpp1 = GetRunningTime(() => ComplexCpp(arr2, arr2, arr2.Length));
            var timeCppUnman1 = GetRunningTime(() => ComplexCppUnman(arr2.Length));
            var timeCSharp1 = GetRunningTime(() => ComplexCsharp(arr2, arr2, arr2.Length));

            var targetNum = 1000000000;
            var timeCpp2 = GetRunningTime(() => ComplexCpp2(targetNum));
            var timeCppUnman2 = GetRunningTime(() => ComplexCpp2Unman(targetNum));
            var timeCSharp2 = GetRunningTime(() => ComplexCsharp2(targetNum));


#if TRACE
            MessageBox.Show(
                string.Join("\n", new[] {
                    $"C extension: {timeCpp1}ms, {timeCppUnman1}ms / C#: {timeCSharp1}ms",
                    $"C extension: {timeCpp2}ms, {timeCppUnman2}ms / C#: {timeCSharp2}ms"}));
#endif
        }


        [DllImport("TraditionalAPI.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCpp(double[] arr1, double[] arr2, int length);
        [DllImport("TraditionalAPI.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCpp2(int num);

        [DllImport("TraditionalAPI.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCppUnman(int length);
        [DllImport("TraditionalAPI.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCpp2Unman(int num);

        public double ComplexCsharp(double[] arr1, double[] arr2, int length)
        {
            double r = 0.0;
            for (int i = 0; i < length; i++)
            {
                r = r + arr1[i] * arr2[i];
                r = r - arr1[i] * arr2[length - i - 1];
                r = Math.Sqrt(r);
            }
            return r;
        }

        public double ComplexCsharp2(int num)
        {
            double n = num;

            while (n > 1.0)
            {
                n = n / 1.00000001;
            }
            return n;
        }
    }
}
