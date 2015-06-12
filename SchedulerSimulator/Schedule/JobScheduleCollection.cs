using System;
using System.Collections.ObjectModel;

namespace SchedulerSimulator.Schedule {

	[Serializable]
	class JobScheduleCollection : KeyedCollection<int, JobSchedule> {
		protected override int GetKeyForItem(JobSchedule item) {
			return item.Id;
		}
	}
}
