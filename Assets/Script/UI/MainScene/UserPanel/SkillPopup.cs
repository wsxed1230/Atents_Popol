
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class SkillPopup : MonoBehaviour
{
    
    public GameObject Content;
    public GameObject PlayerSkill;
    public GameObject DragImage;
    public GameObject SkillSlotDetail;
    GameObject[] AllSlots;
    public bool[] isMouseInSlot = new bool[4];
    Button[] SlotBtnList;

    private void Start() 
    {
        SlotUpdate();
        SortSlot();
        SlotBtnList = Content.GetComponentsInChildren<Button>();
        for(int i = 0; i < SlotBtnList.Length; i++)
        {
            int index = i;
            SlotBtnList[i].onClick.AddListener(() => PressedDetailBtn(index));
        }
        DisplaySlot();
           
    }
    void SlotUpdate()
    {
        AllSlots = Resources.LoadAll<GameObject>("Player/SkillEffect");
        for(int i = 0; i < AllSlots.Length; i++)
        {
            Instantiate(Resources.Load<GameObject>("UI/UserSkill/SlotBg"), Content.transform);
            Content.transform.GetChild(i).GetComponent<UserSkillSlot>().WeaponType
            = AllSlots[i].GetComponent<SkillManager>().WeaponType;
            Content.transform.GetChild(i).GetComponent<UserSkillSlot>().SkillLevel
            = AllSlots[i].GetComponent<SkillManager>().Level;
            Content.transform.GetChild(i).GetComponent<UserSkillSlot>().DetailDesc
            =AllSlots[i].GetComponent<SkillManager>().uiSkillStatus.uiSkillDetailDesc;
            Content.transform.GetChild(i).GetComponent<UserSkillSlot>().SkillEnergy
            =AllSlots[i].GetComponent<SkillManager>().EnergyGage;
            var go = Content.transform.GetChild(i).GetChild(0);
            go.Find("Image").GetComponent<Image>().sprite = 
            AllSlots[i].GetComponent<SkillManager>().uiSkillStatus.uiSkillSprite;
            go.Find("SkillName").GetChild(0).GetComponent<TMP_Text>().text = //Skill Name
            AllSlots[i].GetComponent<SkillManager>().uiSkillStatus.uiSkillName;
            go.Find("SkillType").GetChild(0).GetComponent<TMP_Text>().text = //SkillType
            ItemTypeIntToString.IntToStringUISkillType(AllSlots[i].GetComponent<SkillManager>().WeaponType);
            go.Find("SkillDesc").GetChild(0).GetComponent<TMP_Text>().text = //SkillType
            AllSlots[i].GetComponent<SkillManager>().uiSkillStatus.uiSkillDesc;
            go.Find("SkillLevel").GetChild(0).GetComponent<TMP_Text>().text = 
            AllSlots[i].GetComponent<SkillManager>().Level.ToString();
        }
    }
    public void SortSlot()
    {

        var inst = DataManager.instance.playerData;
        for(int i = 0; i < Content.transform.childCount - 1; i++)//skill level sort
        {
            for(int j = 0; j < Content.transform.childCount - 1; j++)
            {
                if(Content.transform.GetChild(j).GetComponent<UserSkillSlot>().SkillLevel > 
                Content.transform.GetChild(j + 1).GetComponent<UserSkillSlot>().SkillLevel)
                {
                    Content.transform.GetChild(j).SetSiblingIndex(j + 1);
                }
            }
        }
        for(int i = 0; i < PlayerSkill.transform.Find("GridLine").childCount; i++)//Road SkillSlot About RiggingWeapon
        {
            if(inst.SaveUISkillList[i + (10 * inst.Rigging_Weapon_Type)] != "")
            {
                //Debug.Log($"인덱스 체크  : {i + (10 * inst.WeaponType)} 배열 체크 :{inst.UiSkillList[i + (10 * inst.WeaponType)]} ");
                GameObject gameObject = Resources.Load<GameObject>($"Player/SkillEffect/{ItemTypeIntToString.IntToStringSkillFileName(inst.Rigging_Weapon_Type)}/{inst.SaveUISkillList[i + (10 * inst.Rigging_Weapon_Type)]}");
                if(gameObject != null)
                {
                    PlayerSkill.transform.Find("GridLine").GetChild(i).GetComponent<Image>().sprite =
                    gameObject.GetComponent<SkillManager>().uiSkillStatus.uiSkillSprite;
                }
                else
                {
                    PlayerSkill.transform.Find("GridLine").GetChild(i).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("UI/UserSkill/Grey");
                }
            }
            else
            {
                PlayerSkill.transform.Find("GridLine").GetChild(i).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("UI/UserSkill/Grey");
            }
        }
    }
    public void WeaponChange()
    {
        var inst = DataManager.instance.playerData;
        for(int i = 0; i < inst.InGameSkillList.Length; i++)
        {
            inst.InGameSkillList[i] = inst.SaveUISkillList[i + (10 * inst.Rigging_Weapon_Type)];
        }

    }
    public void DisplaySlot()
    {
        var inst = DataManager.instance.playerData;
        for(int i = 0; i < Content.transform.childCount; i++)
        {
            if(Content.transform.GetChild(i).GetComponent<UserSkillSlot>().WeaponType
                == inst.Rigging_Weapon_Type)
            {
                Content.transform.GetChild(i).gameObject.SetActive(true);
                if(Content.transform.GetChild(i).GetComponent<UserSkillSlot>().SkillLevel > inst.Character_CurrentLevel)
                {
                    Content.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    Content.transform.GetChild(i).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_Text>().text
                    =Content.transform.GetChild(i).GetComponent<UserSkillSlot>().SkillLevel.ToString() +"Lv";
                }
                
                else
                {
                    Content.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
            }
            else
            {
                Content.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < SkillSlotDetail.transform.GetChild(0).childCount; i++)
        {
            SkillSlotDetail.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = // Displayed WeaponType
        ItemTypeIntToString.IntToStringUIDesc(DataManager.instance.playerData.Rigging_Weapon_Type);
    }

    public void SkillChange(string SkillName, int index)
    {
        //Debug.Log("checkSkillChange");
        var inst = DataManager.instance.playerData;
        for(int i = 0; i < PlayerSkill.transform.Find("GridLine").childCount; i++)
        {
            var go = PlayerSkill.transform.Find("GridLine").GetChild(i);
            if(go.GetComponent<Image>().sprite.name == SkillName)
            {
                go.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UserSkill/Grey");
            }
        }
        PlayerSkill.transform.Find("GridLine").GetChild(index).GetComponent<Image>().sprite =
        DragImage.GetComponent<Image>().sprite;
        inst.InGameSkillList = new string[4];
        
        for(int i = 0; i < PlayerSkill.transform.Find("GridLine").childCount; i++)
        {
            var go = PlayerSkill.transform.Find("GridLine").GetChild(i);
            if(go.GetComponent<Image>().sprite.name != "Grey")
            {
                inst.InGameSkillList[i] = go.GetComponent<Image>().sprite.name;
                inst.SaveUISkillList[i + (10 * inst.Rigging_Weapon_Type)] =  go.GetComponent<Image>().sprite.name;
            }  
            else
            {
                inst.InGameSkillList[i] = null;
                inst.SaveUISkillList[i + (10 * inst.Rigging_Weapon_Type)] = null;
            }
        }
    }
    
    public void MouseInSlotCheck(int index, bool InCheck)
    {
        isMouseInSlot[index] = InCheck;
    }
    public void EndDrop(int index, Sprite image)
    {
        
        for(int i = 0; i < isMouseInSlot.Length; i++)
        {
            if(isMouseInSlot[i])
                return;
        }

        PlayerSkill.transform.Find("GridLine").GetChild(index).GetComponent<Image>().sprite
        = image;
    }
    void PressedDetailBtn(int index)
    {
        var SlotBgGo = Content.transform.GetChild(index).GetChild(0);
        var UserSkillGo = Content.transform.GetChild(index).GetComponent<UserSkillSlot>();
        for(int i = 0; i < SkillSlotDetail.transform.GetChild(0).childCount; i++)
        {
            SkillSlotDetail.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        }

        SkillSlotDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite =
        SlotBgGo.GetChild(0).GetComponent<Image>().sprite;
        SkillSlotDetail.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text =
        SlotBgGo.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text;
        SkillSlotDetail.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_Text>().text =
        UserSkillGo.SkillEnergy.ToString();
        SkillSlotDetail.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TMP_Text>().text =
        UserSkillGo.DetailDesc;
    }
}

