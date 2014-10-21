using System;
using SchedulerSimulator.Schedule;
using SchedulerSimulator.Schedule.Ruler;
using SchedulerSimulator.View;

namespace SchedulerSimulator {
	class Program {
		static void Main(string[] args) {

			ScheduleManager manager = new ScheduleManager(new ReplanLowPriorityJob());
			manager.Init();
			WorkerController worker = new WorkerController(manager, 1);
			UIController uiController = new UIController(manager, worker);
			worker.StartWorkers();

			Console.ReadLine();
		}
	}
}
