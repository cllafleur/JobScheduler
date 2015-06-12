using System.Collections.Generic;
using System.Linq;

namespace SchedulerSimulator.Schedule.Ruler {
	class ReplanLowPriorityJob : IScheduleComputer {
		private Rule[] rules;

		public ReplanLowPriorityJob() {
			this.rules = ReplanLowPriorityJobRulesBuilder.GetRules();
		}

		public JobScheduleState[] EvaluateNextJobs(IEnumerable<JobScheduleState> jobs, int jobStateCount) {
			IEnumerable<JobScheduleState> nextJobs = GetHighestPriorityJob(jobs, jobStateCount);
			RescheduleJobs(jobs.Except(nextJobs));
			return nextJobs.ToArray();
		}

		private Rule GetRule(int ruleId) {
			return rules.FirstOrDefault(r => r.PriorityValue == ruleId);
		}

		private IEnumerable<JobScheduleState> GetHighestPriorityJob(IEnumerable<JobScheduleState> jobs, int jobStateCount) {
			SortedList<string, JobScheduleState> selectedJobs = new SortedList<string, JobScheduleState>();

			int counter = 0;
			foreach (var job in jobs) {
				counter++;
				selectedJobs.Add(GetSortKey(job, counter), job);
			}
			return selectedJobs.Take(jobStateCount).Select(p => p.Value);
		}

		private string GetSortKey(JobScheduleState jobState, int index) {
			return string.Format("{0:0} {1:0000} {2:yyyyMMddhhmmss} {3:000}", jobState.CurrentPriority, 9999 - jobState.Retry, jobState.InitialPlannedDate, index);
		}

		private void RescheduleJobs(IEnumerable<JobScheduleState> jobs) {
			foreach (var job in jobs) {
				Rule jobRule = GetRule(job.Task.Priority);
				if (job.Retry++ == jobRule.RetryCountToEscalate && job.CurrentPriority > 0) {
					job.CurrentPriority--;
					job.Retry = 0;
				}
				job.CurrentPlannedDate += jobRule.RescheduleDuration;
			}
		}
	}
}
