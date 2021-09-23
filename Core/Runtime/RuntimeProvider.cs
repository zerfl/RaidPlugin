using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RaidPlugin
{
	public abstract class RuntimeProvider
	{
		public static RuntimeProvider Instance;

		public RuntimeProvider()
		{
			Initialize();

			SetupEvents();
		}

		public static void Init() => Instance = new Il2CppProvider();

		public abstract void Initialize();

		public abstract void SetupEvents();

		public abstract void StartCoroutine(IEnumerator routine);

		public abstract void Update();

		public abstract T AddComponent<T>(GameObject obj, Type type) where T : Component;

		public abstract ScriptableObject CreateScriptable(Type type);

		public abstract UnityEngine.Object[] FindObjectsOfTypeAll(Type type);

		internal virtual void ProcessOnPostRender()
		{
		}

		internal virtual void ProcessFixedUpdate()
		{
		}
	}
}