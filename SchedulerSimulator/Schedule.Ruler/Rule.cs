
using System;
using System.Runtime.Serialization;
namespace SchedulerSimulator {
	[DataContract]
	class Rule {
		[DataMember]
		public int PriorityValue { get; set; }

		[DataMember]
		public int RetryCountToEscalate { get; set; }

		[DataMember]
		public TimeSpan RescheduleDuration { get; set; }
	}
}
