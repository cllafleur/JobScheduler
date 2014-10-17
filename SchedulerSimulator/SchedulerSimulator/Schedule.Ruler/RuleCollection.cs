using System;
using System.Collections.ObjectModel;

namespace SchedulerSimulator.Schedule.Ruler {
	[Serializable]
	class RuleCollection : KeyedCollection<int, Rule> {
		protected override int GetKeyForItem(Rule item) {
			return item.PriorityValue;
		}
	}
}
