using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using SchedulerSimulator.Schedule;

namespace SchedulerSimulator {
	internal delegate JobScheduleState GetNextJob();

	class WorkerController {
		private int workerCount;
		private object syncCollect = new object();
		private Collection<Worker> workers;
		private ScheduleManager manager;
		private System.Timers.Timer timer;
		private AutoResetEvent jobReadyForExecution;

		private ConcurrentQueue<JobScheduleState> nextJobQueue = new ConcurrentQueue<JobScheduleState>();
		private DateTime lastCollectedJobs;

		public WorkerController(ScheduleManager manager, int workerCount) {
			this.workerCount = workerCount;
			this.manager = manager;
			this.jobReadyForExecution = new AutoResetEvent(false);
			this.workers = new Collection<Worker>();
			for (int i = 0; i < workerCount; ++i) {
				workers.Add(new Worker(i, new GetNextJob(NextJobHandler), jobReadyForExecution));
			}
			this.timer = new System.Timers.Timer(1000);
			this.timer.AutoReset = false;
			this.timer.Elapsed += timer_Elapsed;
		}

		public void StartWorkers() {
			this.timer.Start();
		}

		public Worker[] GetWorkers() {
			return workers.ToArray();
		}

		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
			try {
				ClearJobQueue();
				CollectNextJobs();
				if (nextJobQueue.Count > 0)
					jobReadyForExecution.Set();
			}
			finally {
				this.timer.Start();
			}
		}

		private JobScheduleState NextJobHandler() {
			try {
				JobScheduleState jobState;
				if (nextJobQueue.TryDequeue(out jobState))
					return jobState;
				return null;
			}
			finally {
				if (nextJobQueue.Count > 0)
					jobReadyForExecution.Set();
			}
		}

		private void ClearJobQueue() {
			lock (this.syncCollect) {
				while (nextJobQueue.Count > 0) {
					JobScheduleState jobState;
					if (nextJobQueue.TryDequeue(out jobState))
						lock (jobState.SyncState) {
							jobState.Status = JobStatus.Planned;
						}
				}
			}
		}

		private void CollectNextJobs() {
			lastCollectedJobs = DateTime.Now;
			lock (this.syncCollect) {
				if (nextJobQueue.Count > 0)
					return;
				var jobStates = manager.GetNextJob(workerCount);
				foreach (var job in jobStates) {
					lock (job.SyncState) {
						job.Status = JobStatus.Pending;
					}
					nextJobQueue.Enqueue(job);
				}
			}
		}
	}
}
