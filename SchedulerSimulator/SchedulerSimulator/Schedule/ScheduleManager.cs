
using System;
namespace SchedulerSimulator.Schedule {
	class ScheduleManager {

		private DateTime startingDate;
		private JobCalendar calendar;

		public void Init() {
			this.startingDate = DateTime.Now + new TimeSpan(0, 0, 2);
			this.calendar = CalendarBuilder.GetCalendar(this.startingDate);

		}

		internal JobScheduleState GetNextJob(JobScheduleState nextJob) {
			throw new NotImplementedException();
		}
	}
}
