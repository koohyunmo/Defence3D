﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
	protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
	public abstract void Init();

	private void Awake()
	{
		Init();
	}

	protected void Bind<T>(Type type) where T : UnityEngine.Object
	{
		try
		{
			string[] names = Enum.GetNames(type);
			UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
			_objects.Add(typeof(T), objects);

			for (int i = 0; i < names.Length; i++)
			{
				if (typeof(T) == typeof(GameObject))
					objects[i] = Utils.FindChild(gameObject, names[i], true);
				else
					objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

				if (objects[i] == null)
					Debug.Log($"Failed to bind({names[i]})");
			}
		}
		catch (Exception ex)
		{
			Debug.Log($"{type} is error");
		}

	}

	protected T Get<T>(int idx) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects = null;
		if (_objects.TryGetValue(typeof(T), out objects) == false)
			return null;

		return objects[idx] as T;
	}

	protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
	protected Text GetText(int idx) { return Get<Text>(idx); }
	protected Button GetButton(int idx) { return Get<Button>(idx); }
	protected Image GetImage(int idx) { return Get<Image>(idx); }

	public static void BindEvent(GameObject go, Action<PointerEventData> action, MyEnums.UIEvent type = MyEnums.UIEvent.Click, bool isLeftClick = true)
	{
		UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case MyEnums.UIEvent.Click:
				if (isLeftClick)
				{
					evt.OnLeftClickHandler -= action;
					evt.OnLeftClickHandler += action;
				}
				else
				{
					evt.OnRightClickHandler -= action;
					evt.OnRightClickHandler += action;
				}
				break;
			case MyEnums.UIEvent.Drag:
				evt.OnDragHandler -= action;
				evt.OnDragHandler += action;
				break;
			case MyEnums.UIEvent.DoubleClick:  // 더블 클릭 케이스 추가
				evt.OnDoubleClickHandler -= action;
				evt.OnDoubleClickHandler += action;
				break;
			case MyEnums.UIEvent.PointUp:
				evt.OnPointerUpHandler -= action;
				evt.OnPointerUpHandler += action;
				break;
		}
	}
}
