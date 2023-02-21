namespace Azure.LogicApps.NET.Base;

public interface IWorkflowItem<T>
{
	static abstract T FromJsonString(string value);

	string ToJsonString();
}
