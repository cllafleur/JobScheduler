using System;
using System.ComponentModel;
using System.Threading;
using PropertyChanged;
using SchedulerSimulator.Job;
using SchedulerSimulator.Schedule;

namespace SchedulerSimulator {
	[ImplementPropertyChanged]
	class Worker : IDisposable, INotifyPropertyChanged {
		private Thread workerThread;
		private AutoResetEvent jobReadyForExecution;

		private JobScheduleState currentJob;
		private GetNextJob nextJobHandler;

		public WorkerStatus Status { get; set; }

		public int Id { get; set; }

		public Worker(int id, GetNextJob nextJobHandler, AutoResetEvent jobReady) {
			this.Id = id;
			this.nextJobHandler = nextJobHandler;
			this.workerThread = new Thread(ExecuteJob);
			this.jobReadyForExecution = jobReady;
			this.workerThread.Start();
		}

		private void ExecuteJob() {
			while (true) {
				jobReadyForExecution.WaitOne();

				SetCurrentJob();
				if (this.currentJob == null)
					continue;
				try {
					this.Status = WorkerStatus.Running;
					RunCurrentJob();
				}
				finally {
					this.Status = WorkerStatus.Idle;
				}
			}
		}

		private void RunCurrentJob() {
			JobRunner runner = new JobRunner(this.currentJob.Task.Details);
			lock (this.currentJob.SyncState) {
				this.currentJob.Status = JobStatus.Running;
			}
			runner.Execute();
			lock (this.currentJob.SyncState) {
				this.currentJob.Status = JobStatus.Done;
			}
		}

		private void SetCurrentJob() {
			if (currentJob == null || currentJob.Status == JobStatus.Done) {
				currentJob = nextJobHandler();
			}
		}

		#region IDisposable Members

		public void Dispose() {
			workerThread.Abort();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
