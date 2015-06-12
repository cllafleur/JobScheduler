using System.Collections.Generic;

namespace SchedulerSimulator.Schedule.Ruler {
	interface IScheduleComputer {
		JobScheduleState[] EvaluateNextJobs(IEnumerable<JobScheduleState> jobs, int jobStateCount);
	}
}
