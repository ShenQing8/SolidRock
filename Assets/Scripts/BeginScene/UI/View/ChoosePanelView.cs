using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoosePanelView : BasePanel
{
    public Text TextMoney;
    public Text TextChName;
    public Text TextBuy;
    public Button BtnLeft;
    public Button BtnRight;
    public Button BtnStart;
    public Button BtnBack;
    public Button BtnBuy;
    
    private GameObject curRole;
    private Transform rolePos;
    
    public event System.Action OnLeftClick;
    public event System.Action OnRightClick;
    public event System.Action OnStartClick;
    public event System.Action OnBackClick;
    public event System.Action OnBuyClick;
    
    protected override void Init() { }
    public void InitializeView()
    {
        UpdateMoneyDisplay(DataManager.Instance.playerData.money);
        rolePos = GameObject.Find("RolePos").transform;
        
        BtnLeft.onClick.AddListener(() => OnLeftClick?.Invoke());
        BtnRight.onClick.AddListener(() => OnRightClick?.Invoke());
        BtnStart.onClick.AddListener(() => OnStartClick?.Invoke());
        BtnBack.onClick.AddListener(() => OnBackClick?.Invoke());
        BtnBuy.onClick.AddListener(() => OnBuyClick?.Invoke());
    }
    
    public void DisplayRole(RoleInfo roleInfo)
    {
        if(curRole != null) DestroyImmediate(curRole);
        
        curRole = Instantiate(Resources.Load<GameObject>(roleInfo.res), 
                             rolePos.position, rolePos.rotation);
        curRole.GetComponent<PlayerControl>().UpdateWeapon(roleInfo.defaultWeaponID);
        
        TextChName.text = roleInfo.name;
    }
    
    public void UpdateMoneyDisplay(int money)
    {
        TextMoney.text = money.ToString();
    }
    
    public void UpdateBuyButton(bool isOwned, int price)
    {
        if(isOwned)
        {
            BtnBuy.gameObject.SetActive(false);
            BtnStart.gameObject.SetActive(true);
        }
        else
        {
            BtnBuy.gameObject.SetActive(true);
            TextBuy.text = "￥" + price.ToString();
            BtnStart.gameObject.SetActive(false);
        }
    }
    
    public override void HideMe(UnityAction callback = null)
    {
        if(curRole != null)
            DestroyImmediate(curRole);
        base.HideMe(callback);
    }
}
