using System;
using System.Linq;
using SchedulerSimulator.Schedule;
using SchedulerSimulator.Schedule.Ruler;
using SchedulerSimulator.View;

namespace SchedulerSimulator {
	class Program {
		static void Main(string[] args) {

			ScheduleManager manager = new ScheduleManager(new ReplanLowPriorityJob());
			manager.Init();
			WorkerController worker = new WorkerController(manager, 2);
			WorkerController worker2 = new WorkerController(manager, 2);
			UIController uiController = new UIController(manager, worker.GetWorkers().Union(worker2.GetWorkers()).ToArray());
			worker.StartWorkers();
			worker2.StartWorkers();

			Console.ReadLine();
		}
	}
}
