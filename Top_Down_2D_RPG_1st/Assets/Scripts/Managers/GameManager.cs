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
    public Animator animTalkPanel;
    public Animator animPortrait;
    public Image portraitImg;
    public Text talkText;
    public TypeEffect talk;
    public Sprite prevPortrait;
    public GameObject scanObject;
    
    public bool isAction;
    public int talkIndex;
    

    private void Start()
    {
        Debug.Log(Managers.Quest.CheckQuest());

        UI_Popup ui = Managers.UI.ShowPopupUI<UI_Popup>("UI_TalkPanel");
        GameObject go = Util.FindChild<Image>(ui.gameObject, "TalkPanel", true).gameObject;
        talkPanel = go;
        talkText = Util.FindChild<Text>(go, "Text", true);
        talk = Util.GetOrAddComponent<TypeEffect>(talkText.gameObject);
        //animTalkPanel = go.gameObject.GetComponent<Animator>();
        animTalkPanel = Util.GetOrAddComponent<Animator>(go);
        portraitImg = Util.FindChild<Image>(go, "Portrait", true);
        //animPortrait = portraitImg.gameObject.GetComponent<Animator>();
        animPortrait = Util.GetOrAddComponent<Animator>(portraitImg.gameObject);
    }

    public void Action(GameObject scanObj)
    {
        if (scanObj == null) return;

        scanObject = scanObj;
        // ObjectData objData = Util.GetOrAddComponent<ObjectData>(scanObject);
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        animTalkPanel.Play("TalkShow");
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = 0;
        string talkData = "";
        //Set Talk Data
        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = Managers.Quest.GetQuestTalkIndex(id);
            talkData = Managers.Talk.GetTalk(id + questTalkIndex, talkIndex);
        }

        //End Talk
        if (talkData == null)
        {
            animTalkPanel.Play("TalkHide");
            isAction = false;
            talkIndex = 0;
            string questName = Managers.Quest.CheckQuest(id);
            Managers.Quest.MakeQuestObjects(questName);
            Debug.Log(questName);
            return;
        }
        
        //Continue Talk
        if (isNpc)
        {
            talk.SetMsg(talkData.Split(':')[0]);

            // Show Portrait
            portraitImg.sprite = Managers.Talk.GetPortrait(id , int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);

            // Animation portrait
            if(prevPortrait != portraitImg.sprite)
            {
                animPortrait.Play("PortraitEffect", -1, 0f);
                prevPortrait = portraitImg.sprite;
            }
        }
        else
        {
            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        talkIndex++;
        isAction = true;
        return;
    }
}
