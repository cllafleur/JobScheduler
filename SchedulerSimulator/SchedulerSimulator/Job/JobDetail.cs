
using System;
using System.Runtime.Serialization;
namespace SchedulerSimulator.Job {

	[DataContract]
	class JobDetail {
		[DataMember]
		public TimeSpan RunningDuration { get; set; }
	}
}
