using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;
using System.Security.Policy;
using System.Windows;

namespace PInvokePerformance
{
    public partial class PerformanceTest
    {
        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_IncrementCounter();

        [DllImport("TraditionalAPI.dll")]
        private static extern double TA_CalculateSquareRoot(double value);

        [DllImport("TraditionalAPI.dll")]
        private static extern double TA_DotProduct(double[] threeTuple1, double[] threeTuple2);

        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_Test1(ulong count);

        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_Test2(ulong count);

        [DllImport("TraditionalAPI.dll")]
        private static extern void TA_Test3(ulong count);

        public void RunTests()
        {
            //  Run the unmanged tests.
            Unmanaged_Test1_Time = GetRunningTime(TestCount, TA_Test1);
            Unmanaged_Test2_Time = GetRunningTime(TestCount, TA_Test2);
            Unmanaged_Test3_Time = GetRunningTime(TestCount, TA_Test3);

            //  Create the managed interface.
            ManagedInterface.ManagedInterface managedInterface = new ManagedInterface.ManagedInterface();

            //  Run the tests through the interface.
            ManagedInterface_Test1_Time = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 1; i <= count; i++)
                    managedInterface.IncrementCounter();
            });
            ManagedInterface_Test2_Time = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double d = managedInterface.CalculateSquareRoot((double)i);
                }
            });
            ManagedInterface_Test3_Time = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double[] threeTuple1 = new double[] { i, i, i };
                    double[] threeTuple2 = new double[] { i, i, i };
                    double d = managedInterface.DotProduct(threeTuple1, threeTuple2);
                }
            });

            //  Run the tests through the pinvoke.
            PInvoke_Test1_Time = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 1; i <= count; i++)
                    TA_IncrementCounter();
            });
            PInvoke_Test2_Time = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double d = TA_CalculateSquareRoot((double)i);
                }
            });
            PInvoke_Test3_Time = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double[] threeTuple1 = new double[] { i, i, i };
                    double[] threeTuple2 = new double[] { i, i, i };
                    double d = TA_DotProduct(threeTuple1, threeTuple2);
                }
            });

            // Run C# safe code
            CSharp_Safe_Test1_Time = GetRunningTime(TestCount, CSharp_Safe_Test1);
            CSharp_Safe_Test2_Time = GetRunningTime(TestCount, CSharp_Safe_Test2);
            CSharp_Safe_Test3_Time = GetRunningTime(TestCount, CSharp_Safe_Test3);

            // Run C# unsafe code
            CSharp_Unsafe_Test1_Time = GetRunningTime(TestCount, CSharp_Unsafe_Test1);
            CSharp_Unsafe_Test2_Time = GetRunningTime(TestCount, CSharp_Unsafe_Test2);
            CSharp_Unsafe_Test3_Time = GetRunningTime(TestCount, CSharp_Unsafe_Test3);

            //  Run the tests through the pinvoke.
            Unsafe_PInvoke_Test1_Time = GetRunningTime(TestCount, PerformanceTest_UnsafePInvoke.Unsafe_PInvoke_Test1);
            Unsafe_PInvoke_Test2_Time = GetRunningTime(TestCount, PerformanceTest_UnsafePInvoke.Unsafe_PInvoke_Test2);
            Unsafe_PInvoke_Test3_Time = GetRunningTime(TestCount, PerformanceTest_UnsafePInvoke.Unsafe_PInvoke_Test3);

            var sources = new List<Tuple<double[], double[]>>((int)TestCount);
            for (ulong i = 1; i <= TestCount; i++)
            {
                var arr = new double[] {i, i, i};
                sources.Add(Tuple.Create(arr, arr));
            }
            Unsafe_PInvoke_Test3_Time_Only_Call = GetRunningTime(TestCount, count =>
            {
                for (ulong i = 0; i < TestCount; i++)
                {
                    var source = sources[(int) i];
                    var temp = PerformanceTest_UnsafePInvoke.Unsafe_PInvoke_Test3_call(source.Item1, source.Item2);
                }
            });

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
                string.Join("\n", new [] {
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

        public ulong TestCount { get; set; } = 1000000;

        public static double GetRunningTime(ulong count, Action<ulong> action)
        {
            return GetRunningTime(() => action?.Invoke(count));
        }

        public static double GetRunningTime(Action action)
        {
            var sw = new Stopwatch();

            sw.Start();
            action?.Invoke();
            sw.Stop();

            return sw.Elapsed.TotalMilliseconds;
        }

        private Stopwatch stopwatch = new Stopwatch();

        public double Unmanaged_Test1_Time { get; set; }
        public double Unmanaged_Test2_Time { get; set; }
        public double Unmanaged_Test3_Time { get; set; }

        public double ManagedInterface_Test1_Time { get; set; }
        public double ManagedInterface_Test2_Time { get; set; }
        public double ManagedInterface_Test3_Time { get; set; }

        public double PInvoke_Test1_Time { get; set; }
        public double PInvoke_Test2_Time { get; set; }
        public double PInvoke_Test3_Time { get; set; }
    }

    public partial class PerformanceTest
    {
        private uint _counter = 0;

        private void CSharp_Safe_Test1(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
                _counter++;
        }

        private void CSharp_Safe_Test2(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                var tmp = Math.Sqrt((double) i);
            }
        }

        private void CSharp_Safe_Test3(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                var arr1 = new double[] {i, i, i};
                var arr2 = new double[] {i, i, i};
                var tmp = arr1[0] * arr2[0] + arr1[1] * arr2[1] + arr1[2] * arr2[2];
            }
        }

        private void CSharp_Unsafe_Test1(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                    _counter++;
            }
        }

        private void CSharp_Unsafe_Test2(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                {
                    var tmp = Math.Sqrt((double)i);
                }
            }
        }

        private void CSharp_Unsafe_Test3(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                {
                    var arr1 = new double[] { i, i, i };
                    var arr2 = new double[] { i, i, i };
                    var tmp = arr1[0] * arr2[0] + arr1[1] * arr2[1] + arr1[2] * arr2[2];
                }
            }
        }

        public double CSharp_Safe_Test1_Time { get; set; }
        public double CSharp_Safe_Test2_Time { get; set; }
        public double CSharp_Safe_Test3_Time { get; set; }

        public double CSharp_Unsafe_Test1_Time { get; set; }
        public double CSharp_Unsafe_Test2_Time { get; set; }
        public double CSharp_Unsafe_Test3_Time { get; set; }

        public double Unsafe_PInvoke_Test1_Time { get; set; }
        public double Unsafe_PInvoke_Test2_Time { get; set; }
        public double Unsafe_PInvoke_Test3_Time { get; set; }
        
        public double Unsafe_PInvoke_Test3_Time_Only_Call { get; set; }
    }

    [SuppressUnmanagedCodeSecurity]
    public static class PerformanceTest_UnsafePInvoke
    {
        [DllImport("TraditionalAPI.dll", EntryPoint = "TA_IncrementCounter")]
        private static extern unsafe void TA_IncrementCounter_unsafe();
        
        [DllImport("TraditionalAPI.dll", EntryPoint = "TA_CalculateSquareRoot")]
        private static extern unsafe double TA_CalculateSquareRoot_unsafe(double value);
        
        [DllImport("TraditionalAPI.dll", EntryPoint = "TA_DotProduct")]
        private static extern unsafe double TA_DotProduct_unsafe(double* threeTuple1, double* threeTuple2);

        public static void Unsafe_PInvoke_Test1(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                    TA_IncrementCounter_unsafe();
            }
        }

        public static void Unsafe_PInvoke_Test2(ulong count)
        {
            unsafe
            {
                for (ulong i = 1; i <= count; i++)
                {
                    double d = TA_CalculateSquareRoot_unsafe((double)i);
                }
            }
        }

        public static unsafe void Unsafe_PInvoke_Test3(ulong count)
        {
            for (ulong i = 1; i <= count; i++)
            {
                double[] threeTuple1 = new double[] { i, i, i };
                double[] threeTuple2 = new double[] { i, i, i };
                fixed (double* tuple1 = threeTuple1, tuple2 = threeTuple2)
                {
                    double d = TA_DotProduct_unsafe(tuple1, tuple2);
                }
            }
        }
        
        public static unsafe double Unsafe_PInvoke_Test3_call(double[] threeTuple1, double[] threeTuple2)
        {
            fixed (double* tuple1 = threeTuple1, tuple2 = threeTuple2)
            {
                return TA_DotProduct_unsafe(tuple1, tuple2);
            }
        }
    }
}
