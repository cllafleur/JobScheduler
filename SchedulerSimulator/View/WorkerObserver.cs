using System;

namespace SchedulerSimulator.View {
	class WorkerObserver {
		private Worker worker;
		private int consolePositionRow;
		private object syncConsole;

		public WorkerObserver(Worker worker, int consolePositionRow, object syncConsole) {
			this.worker = worker;
			this.consolePositionRow = consolePositionRow;
			this.syncConsole = syncConsole;
			this.worker.PropertyChanged += worker_PropertyChanged;
			PrintWorker();
		}

		private void worker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			PrintWorker();
		}

		private void PrintWorker() {
			lock (this.syncConsole) {
				Console.SetCursorPosition(0, consolePositionRow);
				Console.Write("Worker: {0,2} Status: {1,7}", this.worker.Id, this.worker.Status);
			}
		}
	}
}
