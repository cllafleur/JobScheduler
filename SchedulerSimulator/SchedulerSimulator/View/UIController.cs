using System;
using System.Collections.ObjectModel;
using System.Timers;
using SchedulerSimulator.Schedule;

namespace SchedulerSimulator.View {
	class UIController {
		private ScheduleManager manager;
		private Timer timer;

		private Collection<JobScheduleStateObserver> observers;
		private object syncConsole = new object();


		public UIController(ScheduleManager manager) {
			this.manager = manager;
			observers = new Collection<JobScheduleStateObserver>();
			int consolePositionRow = 2;
			foreach (var job in manager.GetJobs()) {
				observers.Add(new JobScheduleStateObserver(job, consolePositionRow, syncConsole));
				consolePositionRow++;
			}
			if (consolePositionRow > 20)
				Console.WindowHeight = consolePositionRow + 1;

			this.timer = new Timer(1000);
			this.timer.Elapsed += timer_Elapsed;
			this.timer.AutoReset = true;
			this.timer.Start();
			PrintCurrentTime();
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e) {
			PrintCurrentTime();
		}

		private void PrintCurrentTime() {
			lock (syncConsole) {
				Console.SetCursorPosition(0, 0);
				Console.Write("Current time : {0,8:hh\\:mm\\:ss}", DateTime.Now);
			}
		}
	}
}
