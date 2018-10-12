using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;
using System.ComponentModel;

namespace PInvokePerformance
{
    public class MainViewModel : SimpleViewModel
    {
        public MainViewModel()
        {
            //  Set up the background worker.
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            //  Create the view model command.
            runTestsCommand = new ViewModelCommand(DoRunTests, true);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //  Set the view model properties.
            PerformanceTest test = e.Result as PerformanceTest;

            Unmanaged_Test1_Result = test.Unmanaged_Test1_Time;
            Unmanaged_Test2_Result = test.Unmanaged_Test2_Time;
            Unmanaged_Test3_Result = test.Unmanaged_Test3_Time;
            ManagedInteface_Test1_Result = test.ManagedInterface_Test1_Time;
            ManagedInteface_Test2_Result = test.ManagedInterface_Test2_Time;
            ManagedInteface_Test3_Result = test.ManagedInterface_Test3_Time;
            PInvoke_Test1_Result = test.PInvoke_Test1_Time;
            PInvoke_Test2_Result = test.PInvoke_Test2_Time;
            PInvoke_Test3_Result = test.PInvoke_Test3_Time;
            CSharp_Safe_Test1_Result = test.CSharp_Safe_Test1_Time;
            CSharp_Safe_Test2_Result = test.CSharp_Safe_Test2_Time;
            CSharp_Safe_Test3_Result = test.CSharp_Safe_Test3_Time;
            CSharp_Unsafe_Test1_Result = test.CSharp_Unsafe_Test1_Time;
            CSharp_Unsafe_Test2_Result = test.CSharp_Unsafe_Test2_Time;
            CSharp_Unsafe_Test3_Result = test.CSharp_Unsafe_Test3_Time;
            Unsafe_PInvoke_Test1_Result = test.Unsafe_PInvoke_Test1_Time;
            Unsafe_PInvoke_Test2_Result = test.Unsafe_PInvoke_Test2_Time;
            Unsafe_PInvoke_Test3_Result = test.Unsafe_PInvoke_Test3_Time;
            Unsafe_PInvoke_Test3_Only_Call_Result = test.Unsafe_PInvoke_Test3_Time_Only_Call;

            //  We can now run the command again.
            runTestsCommand.CanExecute = true;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //  Create a performance test, set the iterations.
            PerformanceTest test = new PerformanceTest();
            test.TestCount = Iterations;

            //  Run the performance test.
            test.RunTests();

            //  Set the result of the worker.
            e.Result = test;
        }

        private void DoRunTests()
        {
            //  The test cannot run again till we finish.
            runTestsCommand.CanExecute = false;

            //  Run the background worker.
            backgroundWorker.RunWorkerAsync();
        }

        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        
        public ulong Iterations { get; set; } = 1000000UL;

        private double _unmanagedTest1Result;
        private double _unmanagedTest2Result;
        private double _unmanagedTest3Result;
        public double Unmanaged_Test1_Result
        {
            get { return _unmanagedTest1Result; }
            set { Set(ref _unmanagedTest1Result, value); }
        }
        public double Unmanaged_Test2_Result
        {
            get { return _unmanagedTest2Result; }
            set { Set(ref _unmanagedTest2Result, value); }
        }
        public double Unmanaged_Test3_Result
        {
            get { return _unmanagedTest3Result; }
            set { Set(ref _unmanagedTest3Result, value); }
        }

        private double _managedIntefaceTest1Result;
        private double _managedIntefaceTest2Result;
        private double _managedIntefaceTest3Result;
        public double ManagedInteface_Test1_Result
        {
            get { return _managedIntefaceTest1Result; }
            set { Set(ref _managedIntefaceTest1Result, value); }
        }
        public double ManagedInteface_Test2_Result
        {
            get { return _managedIntefaceTest2Result; }
            set { Set(ref _managedIntefaceTest2Result, value); }
        }
        public double ManagedInteface_Test3_Result
        {
            get { return _managedIntefaceTest3Result; }
            set { Set(ref _managedIntefaceTest3Result, value); }
        }

        private double _pInvokeTest1Result;
        private double _pInvokeTest2Result;
        private double _pInvokeTest3Result;
        public double PInvoke_Test1_Result
        {
            get { return _pInvokeTest1Result; }
            set { Set(ref _pInvokeTest1Result, value); }
        }
        public double PInvoke_Test2_Result
        {
            get { return _pInvokeTest2Result; }
            set { Set(ref _pInvokeTest2Result, value); }
        }
        public double PInvoke_Test3_Result
        {
            get { return _pInvokeTest3Result; }
            set { Set(ref _pInvokeTest3Result, value); }
        }

        private double _cSharpSafeTest1Result;
        private double _cSharpSafeTest2Result;
        private double _cSharpSafeTest3Result;
        public double CSharp_Safe_Test1_Result
        {
            get { return _cSharpSafeTest1Result; }
            set { Set(ref _cSharpSafeTest1Result, value); }
        }
        public double CSharp_Safe_Test2_Result
        {
            get { return _cSharpSafeTest2Result; }
            set { Set(ref _cSharpSafeTest2Result, value); }
        }
        public double CSharp_Safe_Test3_Result
        {
            get { return _cSharpSafeTest3Result; }
            set { Set(ref _cSharpSafeTest3Result, value); }
        }

        private double _cSharpUnsafeTest1Result;
        private double _cSharpUnsafeTest2Result;
        private double _cSharpUnsafeTest3Result;
        public double CSharp_Unsafe_Test1_Result
        {
            get { return _cSharpUnsafeTest1Result; }
            set { Set(ref _cSharpUnsafeTest1Result, value); }
        }
        public double CSharp_Unsafe_Test2_Result
        {
            get { return _cSharpUnsafeTest2Result; }
            set { Set(ref _cSharpUnsafeTest2Result, value); }
        }
        public double CSharp_Unsafe_Test3_Result
        {
            get { return _cSharpUnsafeTest3Result; }
            set { Set(ref _cSharpUnsafeTest3Result, value); }
        }

        private double _unsafePInvokeTest1Result;
        private double _unsafePInvokeTest2Result;
        private double _unsafePInvokeTest3Result;
        public double Unsafe_PInvoke_Test1_Result
        {
            get { return _unsafePInvokeTest1Result; }
            set { Set(ref _unsafePInvokeTest1Result, value); }
        }
        public double Unsafe_PInvoke_Test2_Result
        {
            get { return _unsafePInvokeTest2Result; }
            set { Set(ref _unsafePInvokeTest2Result, value); }
        }
        public double Unsafe_PInvoke_Test3_Result
        {
            get { return _unsafePInvokeTest3Result; }
            set { Set(ref _unsafePInvokeTest3Result, value); }
        }


        private double _unsafePInvokeTest3OnlyCallResult;
        public double Unsafe_PInvoke_Test3_Only_Call_Result
        {
            get { return _unsafePInvokeTest3OnlyCallResult; }
            set { Set(ref _unsafePInvokeTest3OnlyCallResult, value); }
        }

        private ViewModelCommand runTestsCommand;

        public ViewModelCommand RunTestsCommand
        {
            get { return runTestsCommand; }
        }
    }
}
