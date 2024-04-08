using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    int nowWave = 1;
    bool iscine = false;
    public int TotalWave;

    public GameObject WaveFrame;
    public GameObject WaveTxt;
    public GameObject WaveNum;

    public Vector2 txtPos;
    public Vector2 txtEndPos;

    public float txtScale;
    public float txtEndScale;

    public float moveSpeed;

    public GameObject BrodCastCam;
    public GameObject BossOpening;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) && !iscine)
        {
            nextWave();
        }
    }

    public void nextWave()
    {
        if (nowWave! <= 5)
        {
            state = 0;
            StartCoroutine(WaveChange());
        }
    }

    int state = 0;
    IEnumerator WaveChange()
    {
        iscine = true;
        state = 0;
        while (state == 0)      //Move to Center
        {
            txtPos.y = Mathf.Lerp(txtPos.y, txtEndPos.y, Time.deltaTime * moveSpeed);
            txtScale = Mathf.Lerp(txtScale, txtEndScale, Time.deltaTime * moveSpeed);
            UIMove();
            if (Mathf.Floor(Vector2.Distance(txtPos, txtEndPos)) == 0)
            {
                if(nowWave >= 5)
                {
                    state = 10;
                    break;
                }
                ++state;
                Debug.Log("0end");
            }
            yield return null;
        }
        float alph = 1.0f;
        ++nowWave;
        while (state == 1)      //Change num
        {
            alph -= Time.deltaTime*3;
            WaveNum.GetComponent<TMP_Text>().alpha = Mathf.Abs(alph);
            if (Mathf.Floor(alph*10) <= 0)
            {
                WaveNum.GetComponent<TMP_Text>().text = nowWave.ToString();
            }
            if(alph <= -1)
            {
                txtEndPos.y = 1025;
                txtEndScale = 1;
                alph = 1.0f;
                ++state;
                Debug.Log("1end");
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }

        while (state == 10)     //Boss
        {
            alph -= Time.deltaTime * 3;
            WaveTxt.GetComponent<TMP_Text>().alpha = Mathf.Abs(alph);
            WaveNum.GetComponent<TMP_Text>().alpha = Mathf.Abs(alph);
            if (Mathf.Floor(alph * 10) <= 0)
            {
                WaveNum.GetComponent<TMP_Text>().text = "";
                WaveTxt.GetComponent<TMP_Text>().text = "   Boss";
            }
            if (alph <= -1)
            {
                txtEndPos.y = 1025;
                txtEndScale = 1;
                alph = 1.0f;
                state = 2;
                yield return new WaitForSeconds(1f);
                MonsterFactory a = new MonsterFactory(); a.CreateMonster(30000);
                Instantiate(BrodCastCam);
                Instantiate(BossOpening);
            }
            yield return null;
        }

        while (state == 2)      //Move back
        {
            txtPos.y = Mathf.Lerp(txtPos.y, txtEndPos.y, Time.deltaTime * moveSpeed);
            txtScale = Mathf.Lerp(txtScale, txtEndScale, Time.deltaTime * moveSpeed);
            UIMove();
            if (Mathf.Floor(Vector2.Distance(txtPos, txtEndPos)) == 0)
            {
                txtEndPos.y = 540;
                txtEndScale = 2f;
                ++state;
                iscine = false;
                
                Debug.Log("2end");
            }
            yield return null;
        }
    }

    private void UIMove()
    {
        WaveFrame.GetComponent<RectTransform>().position = txtPos;
        WaveFrame.GetComponent<RectTransform>().localScale = new Vector3(txtScale, txtScale, 1);
    }
}
