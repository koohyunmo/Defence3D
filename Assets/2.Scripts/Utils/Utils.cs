using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;

public static class Utils
{
    public static float SetDelay(float delay)
    {
        return CURRENT_TIME + delay;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    // 베지어 곡선 계산 함수
    public static Vector3 GetBezierCurvePoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * p0
        p += 2 * u * t * p1; // 2 * (1-t) * t * p1
        p += tt * p2; // t^2 * p2

        return p;
    }

    // 베지어 곡선 계산 함수
    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 position = uuu * p0; // 첫 번째 항
        position += 3 * uu * t * p1; // 두 번째 항
        position += 3 * u * tt * p2; // 세 번째 항
        position += ttt * p3; // 네 번째 항

        return position;
    }

    public static string FormatMinutesTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public static string FormatSecondsTime(float timeInSeconds)
    {
        int seconds = Mathf.FloorToInt(timeInSeconds);
        int milliseconds = Mathf.FloorToInt((timeInSeconds - seconds) * 1000);

        return string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }

    public static float Normalize(float originalValue, float minValue, float maxValue)
    {
        return (originalValue - minValue) / (maxValue - minValue);
    }

}
