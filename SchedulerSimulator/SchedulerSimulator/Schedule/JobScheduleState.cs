using System;
using PropertyChanged;
using PropertyChanging;

namespace SchedulerSimulator.Schedule {
	[ImplementPropertyChanged]
	[ImplementPropertyChanging]
	class JobScheduleState {

		private object syncState = new object();
		public object SyncState { get { return syncState; } }

		public JobSchedule Task { get; set; }

		public JobStatus Status { get; set; }

		public int Retry { get; set; }

		public DateTime CurrentPlannedDate { get; set; }

	}
}
