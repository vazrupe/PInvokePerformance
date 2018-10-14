using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace PInvokePerformance
{
    public abstract class RunTestViewModelBase : BindableBase
    {
        private bool _isReady = true;
        private int _jobStep;
        private int _jobCount;

        public bool IsReady
        {
            get { return _isReady; }
            private set { SetProperty(ref _isReady, value); }
        }

        public int JobStep
        {
            get { return _jobStep; }
            private set { SetProperty(ref _jobStep, value); }
        }

        public int JobCount
        {
            get { return _jobCount; }
            private set { SetProperty(ref _jobCount, value); }
        }

        public DelegateCommand RunTestsCommand { get; }

        protected RunTestViewModelBase()
        {
            RunTestsCommand = new DelegateCommand(RunTests).ObservesCanExecute(() => IsReady);
        }

        protected abstract IEnumerable<Action> GenerateJobs();

        private void RunTests()
        {
            IsReady = false;

            Task.Factory.StartNew(() =>
            {
                var jobList = GenerateJobs().ToList();

                JobStep = 0;
                JobCount = jobList.Count;

                foreach (var job in jobList)
                {
                    job();
                    JobStep++;
                }
            }).ContinueWith(t => IsReady = true);
        }
    }
}
