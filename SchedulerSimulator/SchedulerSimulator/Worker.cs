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
		private JobScheduleState nextJob;
		private GetNextJob nextJobHandler;

		private System.Timers.Timer timer;

		public WorkerStatus Status { get; set; }

		public int Id { get; set; }

		public Worker(int id, GetNextJob nextJobHandler) {
			this.Id = id;
			this.timer = new System.Timers.Timer(500);
			this.timer.AutoReset = false;
			this.timer.Elapsed += timer_Elapsed;
			this.nextJobHandler = nextJobHandler;
			this.workerThread = new Thread(ExecuteJob);
			this.jobReadyForExecution = new AutoResetEvent(false);
			this.workerThread.Start();
		}

		public void Start() {
			this.timer.Start();
		}

		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
			try {
				UpdateNextJob();
				SetCurrentJob();
			}
			finally {
				this.timer.Start();
			}
		}

		private void ExecuteJob() {
			while (true) {
				jobReadyForExecution.WaitOne();

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
				if (nextJob != null) {
					currentJob = nextJob;
					nextJob = null;
					jobReadyForExecution.Set();
				}
			}
		}

		private void UpdateNextJob() {
			if (nextJob != null) {
				lock (nextJob.SyncState) {
					nextJob.Status = JobStatus.Planned;
					nextJob = null;
				}
			}
			nextJob = nextJobHandler();
			if (nextJob != null) {
				lock (nextJob.SyncState) {
					nextJob.Status = JobStatus.Pending;
				}
			}
		}

		#region IDisposable Members

		public void Dispose() {
			workerThread.Abort();
			jobReadyForExecution.Dispose();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
