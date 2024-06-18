using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public GameObject hpFill;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetHpBar(float hp, float maxHp)
    {
        float fillAmount = hp / maxHp;
        hpFill.transform.localScale = new Vector3(fillAmount, 1, 1);
    }

    public void ResetHpBar()
    {
        hpFill.transform.localScale = Vector3.one;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = transform.position + mainCamera.transform.rotation * Vector3.forward;
        targetPosition.y = transform.position.y; // y축 고정

        transform.LookAt(targetPosition);
    }
}
