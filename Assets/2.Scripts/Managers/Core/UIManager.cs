using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    Dictionary<string, UI_Popup> _popupDictionary = new Dictionary<string, UI_Popup>();
    public UI_Scene SceneUI  {get; private set;}= null;

    public GameObject Root
    {
        get
        {
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };
            return root;
		}
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

	public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
		if (parent != null)
			go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

		return Utils.GetOrAddComponent<T>(go);
	}

	public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
		if (parent != null)
			go.transform.SetParent(parent);

		return Utils.GetOrAddComponent<T>(go);
	}

	public T ShowSceneUI<T>(string key = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(key))
			key = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"{key}");
		T sceneUI = Utils.GetOrAddComponent<T>(go);
        SceneUI = sceneUI;

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	public T ShowPopupUI<T>(string key = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(key))
            key = typeof(T).Name;

        
        if(_popupDictionary.ContainsKey(key))
        {
            _popupDictionary[key].gameObject.SetActive(true);
            _popupStack.Push(_popupDictionary[key]);
            return _popupDictionary[key] as T;
        }

        GameObject go = Managers.Resource.Instantiate($"{key}");
        T popup = Utils.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);
        _popupDictionary.Add(key, popup);

        go.transform.SetParent(Root.transform);

		return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
		if (_popupStack.Count == 0)
			return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        
        UI_Popup popup = _popupStack.Pop();
        //Managers.Resource.Destroy(popup.gameObject);
        popup.gameObject.SetActive(false);
        //_popupDictionary.Remove(popup.name);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        SceneUI = null;
    }
}
