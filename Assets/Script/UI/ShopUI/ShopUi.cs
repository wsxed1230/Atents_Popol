using System.Collections;

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopUi : MonoBehaviour
{
    public GameObject Contents;
    public GameObject MoneyPopup;
    public GameObject BuyPopup;
    public GameObject DescPopup;
    public GameObject GridLine;
    public UnityEvent<int> AddItem;
    string ItemRiggingStr = "";
    int InstanceCount = 0;
    int ItemPrice;
    int SelectItem;
    Coroutine CorMoneyPopup;
    Coroutine CorSlotHL;
    // Start is called before the first frame update
    void Start()
    {
        ItemDataManager.GetInstance().InvenItemLoadDatas();
        BuyPopup.gameObject.SetActive(false);
        MoneyPopup.gameObject.SetActive(false);
        foreach(KeyValuePair<int, ItemData> item in ItemDataManager.GetInstance().dicItemDatas)
        {   
            bool Found = ItemDataManager.GetInstance().dicResouseTable[item.Key].ImageResourceName.Contains("Begginer");
            if(!Found)
            {
                Instantiate(Resources.Load("UI/ShopUi/ItemSlot"),Contents.transform);
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").GetComponent<UIItem>().Init(item.Key);
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").Find("ItemName").GetComponent<TMP_Text>().text =
                "이름 :\n"+
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").GetComponent<UIItem>().ItemName;
                if(item.Value.Inven_riggingType == 0)
                    ItemRiggingStr = "공격력 : ";
                if(item.Value.Inven_riggingType == 1)
                    ItemRiggingStr = "체력 : ";
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").Find("ItemValue").GetComponent<TMP_Text>().text =
                ItemRiggingStr +
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").GetComponent<UIItem>().ItemValue.ToString();
            
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").Find("ItemType").GetComponent<TMP_Text>().text =
                "종류 :" +
                WeaponTypeToString(Contents.transform.GetChild(InstanceCount).Find("ItemDetail").GetComponent<UIItem>().WeaponType);
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").Find("Gold").Find("Text (TMP)").GetComponent<TMP_Text>().text =
                Contents.transform.GetChild(InstanceCount).Find("ItemDetail").GetComponent<UIItem>().ItemPrice.ToString();
                InstanceCount++;
            }
            Button[] BuyBtnList = Contents.GetComponentsInChildren<Button>();
            for(int i = 0; i < BuyBtnList.Length; i++)
            {
                int index = i;
                BuyBtnList[i].onClick.AddListener(() => PressedBuyBtn(index));
            }    
        }
    }

    public void PressedBuyBtn(int index)
    {
        ItemPrice = int.Parse(Contents.transform.GetChild(index).Find("ItemDetail").Find("Gold").Find("Text (TMP)").GetComponent<TMP_Text>().text);
        if(DataManager.instance.playerData.PlayerGold >= ItemPrice)
        {
            BuyPopup.gameObject.SetActive(true);
            SelectItem = index;
        }
        else
        {
            MoneyPopup.gameObject.SetActive(true);
            CorMoneyPopup = StartCoroutine(OnCorMoneyPopup());
        }
    }
    public void SortRiggingType(int index)
    {
        for(int i = 0; i <  Contents.transform.childCount; i++)
        {
                Contents.transform.GetChild(i).gameObject.SetActive(false);
        }
        if(index == 0)
        {
            for(int i = 0; i <  Contents.transform.childCount; i++)
            {
                Contents.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if(index == 1)
        {
            for(int i = 0; i <  Contents.transform.childCount; i++)
            {
                if(Contents.transform.GetChild(i).Find("ItemDetail").GetComponent<UIItem>().ItemRigging == 0)
                    Contents.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if(index == 2)
        {
            for(int i = 0; i <  Contents.transform.childCount; i++)
            {
                if(Contents.transform.GetChild(i).Find("ItemDetail").GetComponent<UIItem>().ItemRigging == 1)
                    Contents.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    IEnumerator OnCorMoneyPopup()
    {
        yield return new WaitForSeconds(0.8f);
        MoneyPopup.gameObject.SetActive(false);
    }
    public void BuyPopupYesOrNo(int index)
    {
        if(index == 0)
        {
            DataManager.instance.playerData.PlayerGold -= ItemPrice;
            AddItem?.Invoke(Contents.transform.GetChild(SelectItem).Find("ItemDetail").GetComponent<UIItem>().id);
        }
        
        BuyPopup.gameObject.SetActive(false);
    }

    public string WeaponTypeToString(int index)
    {
        string Rtstring = "";
        //0 : 한손 검, 1: 양손 검, 2 : 한손 둔기, 3 : 양손 둔기, 4 : 창, 5 : 단검, 6 : 투창용 창, 10 : 가죽, 11 : 경갑, 12 : 판금
        if(index == 0)
            Rtstring = "한손 검";
        if(index == 1)
            Rtstring = "양손 검";
        if(index == 2)
            Rtstring = "한손 둔기";
        if(index == 3)
            Rtstring = "양손 둔기";
        if(index == 4)
            Rtstring = "창";
        if(index == 5)
            Rtstring = "단검";
        if(index == 6)
            Rtstring = "투창용 창";
        if(index == 10)
            Rtstring = "가죽";
        if(index == 11)
            Rtstring = "경갑";
        if(index == 12)
            Rtstring = "판금";
        return Rtstring;
    }

    public string RiggingTypeToString(int index)
    {
        string Rtstring = "";
        //0 : 무기 1 : 방어구
        if(index == 0)
            Rtstring = "공격력 : ";
        if(index == 1)
            Rtstring = "체력 : ";
       
        return Rtstring;
    }
    public void OnInvenHighLite(int index, bool OnCheck)
    {
        if(GridLine.transform.GetChild(index).GetComponent<UIItem>().id > 0)
        {
            GridLine.transform.GetChild(index).Find("Button").gameObject.SetActive(OnCheck);
            if(OnCheck)
            {
                CorSlotHL = StartCoroutine(OnCorInvenSlotHL(0,index));
            }  
            else
            {
                if(CorSlotHL != null)
                {
                    StopCoroutine(CorSlotHL);
                    CorSlotHL = null;
                }
                    
                DescPopup.transform.gameObject.SetActive(false);
                
            }
        }
    }
    public void OnRiggingHighLite(int index, bool OnCheck)
    {
        GridLine.transform.GetChild(index).Find("Button").gameObject.SetActive(OnCheck);
        if(OnCheck)
        {
            CorSlotHL = StartCoroutine(OnCorInvenSlotHL(1,index));
        }  
        else
        {
            if(CorSlotHL != null)
            {
                StopCoroutine(CorSlotHL);
                CorSlotHL = null;
            }
            DescPopup.transform.gameObject.SetActive(false);
         }
        
    }

    IEnumerator OnCorInvenSlotHL(int type,int index)//0 : grid 1: rigging
    {
        UIItem go = gameObject.AddComponent<UIItem>();
        if(type == 0)
        {
            go = GridLine.transform.GetChild(index).GetComponent<UIItem>();
            if(GridLine.transform.GetChild(index).GetComponent<RectTransform>().anchoredPosition.x <= 300)
            {
                transform.Find("Main_Panel").Find("DescPopup").GetComponent<RectTransform>().anchoredPosition =
                GridLine.transform.GetChild(index).GetComponent<RectTransform>().anchoredPosition + new Vector2(290 , -145);
            }
            else
            {
                transform.Find("Main_Panel").Find("DescPopup").GetComponent<RectTransform>().anchoredPosition =
                GridLine.transform.GetChild(index).GetComponent<RectTransform>().anchoredPosition + new Vector2(-310 , -145);
            }
        } 
        if(type == 1)
        {
            
        }
        yield return new WaitForSeconds(0.8f);
        DescPopup.transform.gameObject.SetActive(true);
        transform.Find("Main_Panel").Find("DescPopup").gameObject.SetActive(true);
        transform.Find("Main_Panel").Find("DescPopup").Find("Paper").Find("ImgBg").Find("ItemImage").GetComponent<Image>().sprite
        = go.icon.sprite;
        transform.Find("Main_Panel").Find("DescPopup").Find("Paper").Find("ItemName").GetComponent<TMP_Text>().text
        = go.ItemName;
        transform.Find("Main_Panel").Find("DescPopup").Find("Paper").Find("ItemType").GetComponent<TMP_Text>().text
        = WeaponTypeToString(go.WeaponType);
        transform.Find("Main_Panel").Find("DescPopup").Find("Paper").Find("ItemValue").GetComponent<TMP_Text>().text
        = RiggingTypeToString(go.ItemRigging) + go.ItemValue.ToString();
        transform.Find("Main_Panel").Find("DescPopup").Find("Paper").Find("ItemDesc").GetComponent<TMP_Text>().text
        = go.ItemDesc;
        
        
        
        
    }
    

}
