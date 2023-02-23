using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Triggers;

public class RecurrenceTrigger : WorkflowTriggerBase
{
    public RecurrenceMethod Recurrence { get; set; }

    public class RecurrenceMethod
	{
		public string Frequency { get; set; }

		public int Interval { get; set; }
	}
}