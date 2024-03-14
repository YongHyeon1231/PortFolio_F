using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject talkPanel;
    public Image portraitImg;
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    public void Action(GameObject scanObj)
    {
        if (scanObj == null) return;

        scanObject = scanObj;
        // ObjectData objData = Util.GetOrAddComponent<ObjectData>(scanObject);
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        string talkData = Managers.Talk.GetTalk(id, talkIndex);

        if( talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }
        
        if (isNpc)
        {
            talkText.text = talkData.Split(':')[0];

            portraitImg.sprite = Managers.Talk.GetPortrait(id , int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        talkIndex++;
        isAction = true;
        return;
    }
}
