using System;
using SchedulerSimulator.Schedule;
using SchedulerSimulator.Schedule.Ruler;
using SchedulerSimulator.View;

namespace SchedulerSimulator {
	class Program {
		static void Main(string[] args) {

			ScheduleManager manager = new ScheduleManager(new ReplanLowPriorityJob());
			manager.Init();
			UIController uiController = new UIController(manager);
			WorkerController worker = new WorkerController(manager, 4);
			worker.StartWorkers();
			//Worker worker = new Worker(manager);
			//Worker worker2 = new Worker(manager);

			Console.ReadLine();
		}
	}
}
