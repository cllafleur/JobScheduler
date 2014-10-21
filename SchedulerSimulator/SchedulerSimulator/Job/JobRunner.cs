using System;
using System.Threading;

namespace SchedulerSimulator.Job {
	class JobRunner {
		private JobDetail detail;

		public JobRunner(JobDetail detail) {
			this.detail = detail;
		}

		public void Execute() {
			Thread.Sleep(this.detail.RunningDuration);
		}
	}
}
