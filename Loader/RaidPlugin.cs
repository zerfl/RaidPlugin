using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;
using Il2CppSystem.Reflection;
using Il2CppSystem.Diagnostics;
using BepInEx;
using BepInEx.Logging;
using RaidPlugin;

namespace RaidPlugin
{
	[BepInPlugin(GUID, NAME, VERSION)]
	public class RaidPlugin : BepInEx.IL2CPP.BasePlugin, IPluginLoader
	{
		public const string NAME = "RaidPlugin";
		public const string VERSION = "1.0.0";
		public const string AUTHOR = "zerfl";
		public const string GUID = "com." + AUTHOR + "." + NAME;


		public static RaidPlugin Instance;
		public ManualLogSource LogSource => Log;
		public Harmony HarmonyInstance => s_harmony;
		private static readonly Harmony s_harmony = new Harmony(Bootstrapper.GUID);

		public Action<object> OnLogMessage => LogSource.LogMessage;
		public Action<object> OnLogWarning => LogSource.LogWarning;
		public Action<object> OnLogError   => LogSource.LogError;

		internal void Init()
		{
			Instance = this;
			Bootstrapper.Init(this);
		}

		public override void Load()
		{
			Init();
		}
	}
}
