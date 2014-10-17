using System.Collections.Generic;

namespace SchedulerSimulator.Schedule.Ruler {
	interface IScheduleComputer {
		JobScheduleState EvaluateNextJob(IEnumerable<JobScheduleState> jobs);
	}
}
