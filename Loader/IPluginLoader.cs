using System;

namespace RaidPlugin
{
	public interface IPluginLoader
	{
		Action<object> OnLogMessage { get; }
		Action<object> OnLogWarning { get; }
		Action<object> OnLogError { get; }
	}
}