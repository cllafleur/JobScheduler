using System;
using System.Collections.ObjectModel;
using System.Timers;
using SchedulerSimulator.Schedule;

namespace SchedulerSimulator.View {
	class UIController {
		private ScheduleManager manager;
		private Worker[] workers;
		private Timer timer;

		private Collection<JobScheduleStateObserver> observers;
		private Collection<WorkerObserver> workerObservers;
		private object syncConsole = new object();


		public UIController(ScheduleManager manager, Worker[] workers) {
			this.manager = manager;
			this.workers = workers;
			observers = new Collection<JobScheduleStateObserver>();
			workerObservers = new Collection<WorkerObserver>();
			int consolePositionRow = 2;

			foreach (var worker in workers) {
				workerObservers.Add(new WorkerObserver(worker, consolePositionRow, syncConsole));
				consolePositionRow++;
			}
			consolePositionRow++;

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
