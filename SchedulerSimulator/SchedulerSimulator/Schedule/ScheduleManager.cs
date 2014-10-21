
using System;
using System.Collections.Generic;
using SchedulerSimulator.Schedule.Ruler;
namespace SchedulerSimulator.Schedule {
	class ScheduleManager {

		private DateTime startingDate;
		private JobCalendar calendar;
		private IScheduleComputer jobRuler;

		private int execCounter;

		public ScheduleManager(IScheduleComputer jobRuler) {
			this.jobRuler = jobRuler;
		}

		public void Init() {
			this.startingDate = DateTime.Now + new TimeSpan(0, 0, 2);
			this.calendar = CalendarBuilder.GetCalendar(this.startingDate);

			foreach (var job in calendar) {
				job.PropertyChanged += job_PropertyChanged;
			}
		}

		public JobScheduleState[] GetNextJob(int jobStateCount) {
			IEnumerable<JobScheduleState> jobs = calendar.GetPlannedJobBefore(DateTime.Now);

			JobScheduleState[] jobStates = jobRuler.EvaluateNextJobs(jobs, jobStateCount);
			return jobStates;
		}

		public IEnumerable<JobScheduleState> GetJobs() {
			return new List<JobScheduleState>(calendar);
		}
		private void job_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "Status" && ((JobScheduleState)sender).Status == JobStatus.Done) {
				((JobScheduleState)sender).ExecOrder = ++execCounter;
			}
		}

	}
}
