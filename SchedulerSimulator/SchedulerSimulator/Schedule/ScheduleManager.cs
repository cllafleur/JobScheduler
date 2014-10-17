
using System;
using System.Collections.Generic;
using SchedulerSimulator.Schedule.Ruler;
namespace SchedulerSimulator.Schedule {
	class ScheduleManager {

		private DateTime startingDate;
		private JobCalendar calendar;
		private IScheduleComputer jobRuler;

		public ScheduleManager(IScheduleComputer jobRuler) {
			this.jobRuler = jobRuler;
		}

		public void Init() {
			this.startingDate = DateTime.Now + new TimeSpan(0, 0, 2);
			this.calendar = CalendarBuilder.GetCalendar(this.startingDate);

		}

		public JobScheduleState GetNextJob() {
			IEnumerable<JobScheduleState> jobs = calendar.GetPlannedJobBefore(DateTime.Now);

			JobScheduleState job = jobRuler.EvaluateNextJob(jobs);
			return job;
		}
	}
}
