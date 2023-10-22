using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    // start 함수에서는 사용 불가하므로 Init으로 관리
    public override void Init()
    {
        GameManager.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        GameManager.UI.ClosePopupUI(this);
    }
}
