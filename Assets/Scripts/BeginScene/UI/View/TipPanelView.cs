using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TipPanelView : BasePanel
{
    public Text TextContent;
    public Button BtnConfirm;
    public Button BtnCansel;

    public event System.Action OnConfirmClick;
    public event System.Action OnCancelClick;

    protected override void Init() { }

    public void InitView()
    {
        BtnConfirm.onClick.AddListener(()=> OnConfirmClick?.Invoke());
        BtnCansel.onClick.AddListener(()=> OnCancelClick?.Invoke());
    }

    public void UpdateContent(string content)
    {
        TextContent.text = content;
    }
}
