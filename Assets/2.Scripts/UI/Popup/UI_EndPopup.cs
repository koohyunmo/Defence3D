using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EndPopup : UI_Popup
{
    enum TMPs
    {
        Title_TMP,
        Info_TMP
    }

    enum Buttons
    {
        RestartButton,
        EndButton
    }

    public override void Init() 
    {
        Bind<TextMeshProUGUI>(typeof(TMPs));
        Bind<Button>(typeof(Buttons));

        Get<Button>((int)Buttons.EndButton).gameObject.BindEvent(ClickGameLeaveButton);
        Get<Button>((int)Buttons.RestartButton).gameObject.BindEvent(ClickGameRestartButton);
    }

    private void ClickGameLeaveButton(PointerEventData data)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    private void ClickGameRestartButton(PointerEventData data)
    {
        Managers.Stage.Resert();
    }

    public void ShowGameOver()
    {
        UpdateUI("GameOver", "GameOver");
    }

    public void ShowGameClear()
    {
        UpdateUI("GameClear","GameClear");
    }

    private void UpdateUI(string title,string info)
    {
        Get<TextMeshProUGUI>((int)TMPs.Title_TMP).text = title;
        Get<TextMeshProUGUI>((int)TMPs.Info_TMP).text = info;
    }
}
