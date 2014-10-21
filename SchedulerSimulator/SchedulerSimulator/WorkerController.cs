using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using SchedulerSimulator.Schedule;

namespace SchedulerSimulator {
	internal delegate JobScheduleState GetNextJob();

	class WorkerController {
		private int workerCount;
		private object syncCollect = new object();
		private Collection<Worker> workers;
		private ScheduleManager manager;

		private ConcurrentQueue<JobScheduleState> nextJobQueue = new ConcurrentQueue<JobScheduleState>();

		public WorkerController(ScheduleManager manager, int workerCount) {
			this.workerCount = workerCount;
			this.manager = manager;
		}

		public void StartWorkers() {
			this.workers = new Collection<Worker>();
			for (int i = 0; i < workerCount; ++i) {
				workers.Add(new Worker(new GetNextJob(NextJobHandler)));
			}
		}

		private JobScheduleState NextJobHandler() {
			if (nextJobQueue.Count == 0)
				CollectNextJobs();

			JobScheduleState jobState;
			if (nextJobQueue.TryDequeue(out jobState))
				return jobState;
			return null;
		}

		private void CollectNextJobs() {
			lock (this.syncCollect) {
				if (nextJobQueue.Count > 0)
					return;
				var jobStates = manager.GetNextJob(workerCount);
				foreach (var job in jobStates) {
					nextJobQueue.Enqueue(job);
				}
			}
		}
	}
}
