namespace SchedulerSimulator.Schedule {
	using System;
	using System.Runtime.Serialization;
	using SchedulerSimulator.Job;

	[DataContract]
	class JobSchedule {
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public TimeSpan PlannedRunDate { get; set; }

		[DataMember]
		public int Priority { get; set; }

		[DataMember]
		public JobDetail Details { get; set; }
	}
}
