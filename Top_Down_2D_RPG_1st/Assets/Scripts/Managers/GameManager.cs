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
    public Text questTalk;
    public TypeEffect talk;
    public Sprite prevPortrait;
    public GameObject menuSet;
    public GameObject scanObject;
    public GameObject player;
    public Button save;
    public Button exit;
    public QuestManager questManager;
    
    public bool isAction;
    public int talkIndex;
    

    private void Start()
    {   
        UI_Popup ui = Managers.UI.ShowPopupUI<UI_Popup>("UI_TalkPanel");
        GameObject go = Util.FindChild<Image>(ui.gameObject, "TalkPanel", true).gameObject;
        talkPanel = go;
        talkText = Util.FindChild<Text>(go, "Text", true);
        talk = Util.GetOrAddComponent<TypeEffect>(talkText.gameObject);
        animTalkPanel = Util.GetOrAddComponent<Animator>(go);
        portraitImg = Util.FindChild<Image>(go, "Portrait", true);
        animPortrait = Util.GetOrAddComponent<Animator>(portraitImg.gameObject);
        ui = Managers.UI.ShowPopupUI<UI_Popup>("UI_Menu");
        menuSet = Util.FindChild<Image>(ui.gameObject, "Menu Set", true).gameObject;
        menuSet.SetActive(false);
        questTalk = Util.FindChild<Text>(menuSet, "Quest", true);
        questTalk.text = Managers.Quest.CheckQuest();
        save = Util.FindChild<Button>(menuSet, "Save", true);
        save.onClick.AddListener(() => GameSave());
        exit = Util.FindChild<Button>(menuSet, "Exit", true);
        exit.onClick.AddListener(() => GameExit());
        player = GameObject.Find("Player");

        GameLoad();
    }

    private void Update()
    {
        // Sub Menu
        if (Input.GetButtonDown("Cancel"))
        {
            if(menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }
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
            questTalk.text = questName;
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

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", Managers.Quest.questId);
        PlayerPrefs.SetInt("QuestActionIndex", Managers.Quest.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        Util.GetOrAddComponent<PlayerController>(player).CellPos = new Vector3Int((int)x - 1, (int)y, 0);
        player.transform.position = new Vector3(x, y, 0);
        Managers.Quest.questId = questId;
        Managers.Quest.questActionIndex = questActionIndex;
        Managers.Quest.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
