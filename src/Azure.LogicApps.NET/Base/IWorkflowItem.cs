namespace Azure.LogicApps.NET.Base;

public interface IWorkflowItem<T>
{
	static abstract T FromWorkflowJsonString(string value);

	string ToWorkflowJsonString();
}
