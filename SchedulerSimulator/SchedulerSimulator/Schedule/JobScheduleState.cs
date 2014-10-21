using System;
using System.ComponentModel;
using PropertyChanged;
using PropertyChanging;

namespace SchedulerSimulator.Schedule {
	[ImplementPropertyChanged]
	[ImplementPropertyChanging]
	class JobScheduleState : INotifyPropertyChanged {

		private object syncState = new object();
		public object SyncState { get { return syncState; } }

		public JobSchedule Task { get; set; }

		public JobStatus Status { get; set; }

		public int CurrentPriority { get; set; }

		public int Retry { get; set; }

		public DateTime CurrentPlannedDate { get; set; }

		public DateTime InitialPlannedDate { get; set; }

		public int ExecOrder { get; set; }


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
