using System.IO;
using System.Runtime.Serialization.Json;

namespace SchedulerSimulator.Schedule.Ruler {
	class ReplanLowPriorityJobRulesBuilder {

		public static Rule[] GetRules() {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Rule[]));
			using (FileStream stream = new FileStream("DataRules.json", FileMode.Open, FileAccess.Read)) {
				object o = serializer.ReadObject(stream);
				return (Rule[])o;
			}
		}
	}
}
