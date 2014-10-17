using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SchedulerSimulator.Schedule {
	class JobCalendar : Collection<JobScheduleState> {

		public IEnumerable<JobScheduleState> GetPlannedJobBefore(DateTime date) {
			return this.Where(i => {
				lock (i.SyncState) {
					return i.CurrentPlannedDate <= date && i.Status == JobStatus.Planned;
				}
			}).ToList();
		}
	}
}
