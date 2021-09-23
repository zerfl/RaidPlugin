using System;
using System.Collections.Generic;
using System.Reflection;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RaidPlugin
{
    public class Il2CppProvider : RuntimeProvider
    {
        public override void Initialize()
        {
        }

        public override void SetupEvents()
        {
            try
            {
                // Application.add_logMessageReceived(new Action<string, string, LogType>(Application_logMessageReceived));
            }
            catch (Exception ex)
            {
                Bootstrapper.LogWarning("Exception setting up Unity log listener, make sure Unity libraries have been unstripped!");
                Bootstrapper.Log(ex);
            }
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
	        Bootstrapper.LogUnity(condition, type);
        }

        public override void Update()
        {
            Il2CppCoroutine.Process();
        }

        internal override void ProcessOnPostRender()
        {
            Il2CppCoroutine.ProcessWaitForEndOfFrame();
        }

        internal override void ProcessFixedUpdate()
        {
            Il2CppCoroutine.ProcessWaitForFixedUpdate();
        }

        public override void StartCoroutine(IEnumerator routine)
        {
            Il2CppCoroutine.Start(routine);
        }

        public override T AddComponent<T>(GameObject obj, Type type)
        {
            return obj.AddComponent(Il2CppType.From(type)).TryCast<T>();
        }

        public override ScriptableObject CreateScriptable(Type type)
        {
            return ScriptableObject.CreateInstance(Il2CppType.From(type));
        }

        internal delegate IntPtr d_FindObjectsOfTypeAll(IntPtr type);

        public override UnityEngine.Object[] FindObjectsOfTypeAll(Type type)
        {
	        var iCall = ICallManager.GetICallUnreliable<d_FindObjectsOfTypeAll>(new[]
	        {
		        "UnityEngine.Resources::FindObjectsOfTypeAll",
		        "UnityEngine.ResourcesAPIInternal::FindObjectsOfTypeAll" // Unity 2020+ updated to this
	        });

	        return new Il2CppReferenceArray<UnityEngine.Object>(iCall.Invoke(Il2CppType.From(type).Pointer));
        }

    }
}

public static class Il2CppExtensions
{
    public static void AddListener(this UnityEvent action, Action listener)
    {
        action.AddListener(listener);
    }

    public static void AddListener<T>(this UnityEvent<T> action, Action<T> listener)
    {
        action.AddListener(listener);
    }

    public static void RemoveListener(this UnityEvent action, Action listener)
    {
        action.RemoveListener(listener);
    }

    public static void RemoveListener<T>(this UnityEvent<T> action, Action<T> listener)
    {
        action.RemoveListener(listener);
    }

    public static void SetChildControlHeight(this HorizontalOrVerticalLayoutGroup group, bool value) => group.childControlHeight = value;
    public static void SetChildControlWidth(this HorizontalOrVerticalLayoutGroup group, bool value) => group.childControlWidth = value;
}
