using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanelView : BasePanel
{
    public Button BtnSatart;
    public Button BtnSettings;
    public Button BtnAbout;
    public Button BtnExit;

    public event System.Action OnStartClick;
    public event System.Action OnSettingsClick;
    public event System.Action OnAboutClick;
    public event System.Action OnExitClick;

    protected override void Init() { }

    public void InitializeView()
    {
        BtnSatart.onClick.AddListener(() => OnStartClick?.Invoke());
        BtnSettings.onClick.AddListener(() => OnSettingsClick?.Invoke());
        BtnAbout.onClick.AddListener(() => OnAboutClick?.Invoke());
        BtnExit.onClick.AddListener(() => OnExitClick?.Invoke());
    }
}
