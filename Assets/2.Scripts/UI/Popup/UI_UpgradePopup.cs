using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpgradePopup : UI_Popup
{
    enum ButtonIMG
    {
        BG,
        CloseIMG,
        Upgrade1,
        Upgrade2,
        Upgrade3,
        SpawnUpgrade
    }

    private void Start() 
    {
        Bind<Image>(typeof(ButtonIMG));

        Get<Image>((int)ButtonIMG.BG).gameObject.BindEvent(ClosePopup);
        Get<Image>((int)ButtonIMG.CloseIMG).gameObject.BindEvent(ClosePopup);
    }

    private void ClosePopup(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
}
