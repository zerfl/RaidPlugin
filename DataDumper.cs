using System;
using System.Collections;
using System.Reflection;
using Il2CppSystem.IO;
using Plarium.Common.Serialization;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RaidPlugin
{
	public class DataDumper : MonoBehaviour
	{
		internal static DataDumper Instance { get; private set; }
		private static bool _sceneIsInitialized;

		internal static void Setup()
		{
            ClassInjector.RegisterTypeInIl2Cpp<DataDumper>();

			var obj = new GameObject("DataDumper");
			GameObject.DontDestroyOnLoad(obj);
			obj.hideFlags |= HideFlags.HideAndDontSave;
			Instance = obj.AddComponent<DataDumper>();
		}

        public DataDumper(IntPtr ptr) : base(ptr) { }

        internal void Awake()
        {
        }

        private IEnumerator WaitForInit()
        {
	        //Bootstrapper.LogWarning("Waiting...");
	        while (SceneManager.GetActiveScene().name != "Village")
	        {
		        yield return null;
	        }

			_sceneIsInitialized = true;
	        ExtractStaticData();
        }

        private void ExtractStaticData()
        {
	        try
	        {
		        var data = SharedModel.SharedModelManager.StaticData;
		        var outFile = Path.Combine(AppContext.BaseDirectory, "static_data.json");

		        File.WriteAllText(outFile, JsonMain.ToJsonStr(data, true));
	        }
	        catch (Exception ex)
	        {
		        Bootstrapper.LogWarning($"Data extraction failed: {ex}");
	        }
        }


        internal void Update()
        {
	        try
	        {
		        if (!_sceneIsInitialized)
		        {
			        RuntimeProvider.Instance.StartCoroutine(WaitForInit());
		        }
		        
	        }
	        catch (Exception ex)
	        {
		        Bootstrapper.LogWarning($"Failed: {ex}");
	        }
        }

        internal void FixedUpdate()
        {
        }

        internal void LateUpdate()
        {
        }

	}
}
