using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerSimulator.Schedule {
	class JobCalendar : Collection<JobScheduleState> {

		public IEnumerable<JobScheduleState> GetPlannedJobBefore(DateTime date) {
			return this.Where(i => i.CurrentPlannedDate <= date).ToList();
		}
	}
}
