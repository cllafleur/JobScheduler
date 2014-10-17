using System;
using System.Threading;
using SchedulerSimulator.Job;
using SchedulerSimulator.Schedule;

namespace SchedulerSimulator {
	class Worker : IDisposable {
		private Thread workerThread;
		private AutoResetEvent jobReadyForExecution;

		private JobScheduleState currentJob;
		private JobScheduleState nextJob;
		private ScheduleManager scheduleManager;

		private System.Timers.Timer timer;

		public Worker(ScheduleManager scheduleManager) {
			this.timer = new System.Timers.Timer(500);
			this.timer.AutoReset = false;
			this.timer.Elapsed += timer_Elapsed;
			this.scheduleManager = scheduleManager;
			this.workerThread = new Thread(ExecuteJob);
			this.jobReadyForExecution = new AutoResetEvent(false);
			this.workerThread.Start();
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

				RunCurrentJob();
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
			lock (nextJob.SyncState) {
				nextJob.Status = JobStatus.Planned;
				nextJob = null;
			}
			nextJob = scheduleManager.GetNextJob();
			lock (nextJob.SyncState) {
				nextJob.Status = JobStatus.Pending;
			}
		}

		#region IDisposable Members

		public void Dispose() {
			workerThread.Abort();
			jobReadyForExecution.Dispose();
		}

		#endregion
	}
}
