using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;

namespace SchedulerSimulator.Schedule {
	class CalendarBuilder {

		public static JobCalendar GetCalendar(DateTime startDate) {
			var jobs = GetJobScheduleFromFile();
			//DebugJson(jobs);

			var calendar = new JobCalendar();
			foreach (var job in jobs) {
				JobScheduleState state = new JobScheduleState();
				state.Task = job;
				state.Status = JobStatus.Planned;
				state.CurrentPlannedDate = startDate + job.PlannedRunDate;
				calendar.Add(state);
			}
			return calendar;
		}

		private static JobSchedule[] GetJobScheduleFromFile() {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JobSchedule[]));
			using (FileStream stream = new FileStream("DataSchedule.json", FileMode.Open, FileAccess.Read)) {
				object o = serializer.ReadObject(stream);
				return (JobSchedule[])o;
			}
		}

		private static void DebugJson(JobSchedule[] jobs) {

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JobSchedule[]));
			using (MemoryStream stream = new MemoryStream()) {
				serializer.WriteObject(stream, jobs);

				stream.Position = 0;
				TextReader reader = new StreamReader(stream);
				Debug.Write(reader.ReadToEnd());

			}
			Debug.Write("toto");

		}
	}
}
