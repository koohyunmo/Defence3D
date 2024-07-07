using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;

public class ResourceManager
{
	Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

	public void Init()
	{
		
	}

	public T Load<T>(string key) where T : Object
	{
		if (_resources.TryGetValue(key, out Object resource))
			return resource as T;

		return null;
	}
	public void PrePooling(GameObject prefab, int count = 5)
	{
		List<GameObject> pool = new();
		for(int i =0; i < count; i++)
		{
			pool.Add(Instantiate(prefab, pooling:true));
		}
		for (int i = 0; i < count; i++)
		{
			Destroy(pool[i]);
		}
	}
	public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = Load<GameObject>($"{key}");
		if (prefab == null)
		{
			Debug.Log($"Failed to load prefab : {key}");
			return null;
		}

		// Pooling
		if (pooling)
			return Managers.Pool.Pop(prefab);

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;
		return go;
	}
	public GameObject Instantiate(GameObject pb, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = pb;
		if (prefab == null)
		{
			Debug.Log($"prefab is Null");
			return null;
		}

		// Pooling
		if (pooling)
			return Managers.Pool.Pop(prefab);

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;
		return go;
	}
	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		if (Managers.Pool.Push(go))
			return;

		Object.Destroy(go);
	}
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		// 이미 로드된 경우 콜백을 호출하고 리턴
		if (_resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		// Addressables에서 실제 로드할 키를 설정
		string loadKey = key;
		if (key.Contains(".sprite"))
			loadKey = $"{key}[{key.Replace(".sprite", "")}]";

		// Addressables를 통해 비동기적으로 리소스 로드
		var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
		asyncOperation.Completed += (op) =>
		{
			// 중복 체크 후 추가
			if (!_resources.ContainsKey(key))
				_resources.Add(key, op.Result);
			else
				Debug.LogWarning($"Key '{key}' is already in the resource dictionary.");

			callback?.Invoke(op.Result);
		};
	}

	public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
	{
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			foreach (var result in op.Result)
			{
				LoadAsync<T>(result.PrimaryKey, (obj) =>
				{
					loadCount++;
					callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
				});
			}
		};
	}
}
