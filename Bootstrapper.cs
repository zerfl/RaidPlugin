using System;
using HarmonyLib;
using UnityEngine;

namespace RaidPlugin
{
	public static class Bootstrapper
	{
		public const string NAME = "RaidPlugin";
		public const string VERSION = "1.0.0";
		public const string AUTHOR = "zerfl";
		public const string GUID = "com." + AUTHOR + "." + NAME;

		public static IPluginLoader Loader { get; private set; }

		public static void Init(IPluginLoader loader)
		{
			if (Loader != null)
			{
				LogWarning("Plugin is already loaded!");
				return;
			}
			Loader = loader;

			Log($"{NAME} {VERSION} initializing...");
			RuntimeProvider.Init();

			DataDumper.Setup();
		}

		public static void Log(object message) 
			=> Log(message, LogType.Log);

		public static void LogWarning(object message) 
			=> Log(message, LogType.Warning);

		public static void LogError(object message) 
			=> Log(message, LogType.Error);

		public static void LogUnity(object message, LogType logType)
		{
			Log($"[Unity] {message}", logType);
		}

		private static void Log(object message, LogType logType)
		{
			string log = message?.ToString() ?? "";

			switch (logType)
			{
				case LogType.Assert:
				case LogType.Log:
					Loader.OnLogMessage(log);
					break;

				case LogType.Warning:
					Loader.OnLogWarning(log);
					break;

				case LogType.Error:
				case LogType.Exception:
					Loader.OnLogError(log);
					break;
			}
		}

	}
}
