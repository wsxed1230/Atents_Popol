
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UserPanel : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject MainUi;
    public GameObject BossMonsterObj;
    public GameObject DragImage;
    public UnityEvent PlayerAbilityUpdate;
    public UnityEvent SkillSlotUpdate;
    public UnityEvent<int> BtnAct;
    public UnityEvent<int> CleanBtnAct;
    public UnityEvent<int> FreshBtnAct;
    public UnityEvent SaveInvenData;

    public GameObject UserPanelPopup;
    public void PressedBtn(int index)
    {
        
        if(index == 0)
        {
            if(!MainUi.gameObject.activeSelf)
            {
                for(int i = 2; i < Canvas.transform.childCount; i++)
                {
                    Canvas.transform.GetChild(i).gameObject.SetActive(false);
                }
                MainUi.gameObject.SetActive(true);
                BtnAct?.Invoke(0);
            }
            else
            {
                ShowPopup(index);
            }
            SaveInvenData?.Invoke();
            DataManager.instance.SaveData();
        }
        else 
        {
            ShowPopup(index);
            if(index == 1)
            {
                SkillSlotUpdate?.Invoke();
            }
            CleanBtnAct?.Invoke(-1);
            FreshBtnAct?.Invoke(0);
        }
        
    }
    void ShowPopup(int index)
    {
        for(int i = 0; i < UserPanelPopup.transform.childCount; i++)
        {
            UserPanelPopup.transform.GetChild(i).gameObject.SetActive(false);
        }
        UserPanelPopup.transform.gameObject.SetActive(true);
        BossMonsterObj.transform.gameObject.SetActive(false);
        PlayerAbilityUpdate?.Invoke();
        UserPanelPopup.transform.GetChild(index).gameObject.SetActive(true);
        DragImage.gameObject.SetActive(true);
    }
    public void ClosePopup()
    {
        UserPanelPopup.transform.gameObject.SetActive(false);
        BossMonsterObj.transform.gameObject.SetActive(true);
        SaveInvenData?.Invoke();
        DataManager.instance.SaveData();
    }
    

    public void GoToPopup(int index)
    {
        if(index == 0)//Yes
        {
            SceneLoading.SceneNum(0);
            DataManager.instance.DataClear();
            SceneManager.LoadScene(1);
        }
        if(index == 1)//No
        {
            UserPanelPopup.gameObject.SetActive(false);
        }
    }
}
