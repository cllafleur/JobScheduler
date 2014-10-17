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
			this.timer.Elapsed += timer_Elapsed;
			this.scheduleManager = scheduleManager;
			this.workerThread = new Thread(ExecuteJob);
			this.jobReadyForExecution = new AutoResetEvent(false);
			this.workerThread.Start();
		}

		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
			UpdateNextJob();
			SetCurrentJob();
		}

		private void ExecuteJob() {
			while (true) {
				jobReadyForExecution.WaitOne();

				RunCurrentJob();
			}
		}

		private void RunCurrentJob() {
			JobRunner runner = new JobRunner(this.currentJob.Task.Details);
			this.currentJob.Status = JobStatus.Running;
			runner.Execute();
			this.currentJob.Status = JobStatus.Done;
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
			nextJob = scheduleManager.GetNextJob(nextJob);
		}

		#region IDisposable Members

		public void Dispose() {
			workerThread.Abort();
			jobReadyForExecution.Dispose();
		}

		#endregion
	}
}
