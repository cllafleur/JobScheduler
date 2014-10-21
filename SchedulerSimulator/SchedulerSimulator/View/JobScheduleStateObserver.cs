using System;
using System.Collections.ObjectModel;

namespace SchedulerSimulator.View {
	class JobScheduleStateObserver {

		private static readonly Collection<ConsoleColor[]> priorityColors = new Collection<ConsoleColor[]>() { 
			new [] {ConsoleColor.DarkBlue, ConsoleColor.Magenta},
			new[] {ConsoleColor.DarkBlue, ConsoleColor.Red},
			new[] {ConsoleColor.DarkBlue, ConsoleColor.Yellow}
		};

		private static readonly ConsoleColor[] statusColors = new[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.DarkGreen, ConsoleColor.Green };

		private Schedule.JobScheduleState job;
		private int consolePositionRow;
		private object syncConsole;

		public JobScheduleStateObserver(Schedule.JobScheduleState job, int consolePositionRow, object syncConsole) {
			this.job = job;
			this.consolePositionRow = consolePositionRow;
			this.syncConsole = syncConsole;
			this.job.PropertyChanged += job_PropertyChanged;
			PrintJob();
		}

		void job_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			PrintJob();
		}

		private void PrintJob() {
			lock (syncConsole) {
				Console.SetCursorPosition(0, consolePositionRow);

				Console.Write("Id: {0,3} Priority: ", job.Task.Id);

				ConsoleColor[] currentPriorityColors = priorityColors[job.Task.Priority];
				Console.ForegroundColor = currentPriorityColors[0];
				Console.BackgroundColor = currentPriorityColors[1];
				Console.Write("{0}", job.Task.Priority);
				Console.ResetColor();

				Console.Write("(");

				currentPriorityColors = priorityColors[job.CurrentPriority];
				Console.ForegroundColor = currentPriorityColors[0];
				Console.BackgroundColor = currentPriorityColors[1];
				Console.Write("{0}", job.CurrentPriority);
				Console.ResetColor();

				Console.Write(") Planned @{0,8:hh\\:mm\\:ss} Retry: {1,3} Status: ", job.InitialPlannedDate, job.Retry);

				//Console.Write(string.Format("Id: {0,3} Priority: {1,2} Planned @{2,8:hh\\:mm\\:ss} Retry:{3,3} Status: ", job.Task.Id, job.CurrentPriority, job.CurrentPlannedDate, job.Retry));

				Console.BackgroundColor = statusColors[(int)job.Status];
				Console.Write("{0,7}", job.Status);
				Console.ResetColor();

				Console.Write(" {0}", job.ExecOrder);
			}
		}
	}
}
