using System.Linq;

namespace SchedulerSimulator.Schedule.Ruler {
	class ReplanLowPriorityJob : IScheduleComputer {
		private Rule[] rules;

		public ReplanLowPriorityJob() {
			this.rules = ReplanLowPriorityJobRulesBuilder.GetRules();
		}

		public JobScheduleState EvaluateNextJob(System.Collections.Generic.IEnumerable<JobScheduleState> jobs) {
			return null;
		}

		private Rule GetRule(int ruleId) {
			return rules.FirstOrDefault(r => r.PriorityValue == ruleId);
		}
	}
}
