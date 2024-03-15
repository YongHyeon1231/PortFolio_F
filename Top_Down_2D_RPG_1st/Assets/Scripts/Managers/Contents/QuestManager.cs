using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class QuestManager
{
    public int questId = 10;
    public int questActionIndex;
    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;

    public void Init()
    {
        questList = new Dictionary<int, QuestData>();
        questObject = new GameObject[1000];
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("마을 사람들과 대화하기"
                                        , new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("루도의 동전 찾아주기."
                                        , new int[] { 5000, 2000 }));
        questList.Add(30, new QuestData("퀘스트 올 클리어!"
                                        , new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public void MakeQuestObjects(string questName)
    {
        if (questName == "루도의 동전 찾아주기.")
        {
            if (questObject[0] == null)
            {
                questObject[0] = Managers.Resource.Instantiate("QuestObjects/Bronze Coin");
            }
        }
    }

    public string CheckQuest(int npcId)
    {
        // Next Talk Target
        if (npcId == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // Control Quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        // Quest Name
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        // Quest Name
        return questList[questId].questName;
    }

    private void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    private void ControlObject()
    {
        switch (questId)
        {
            case 10:
                break;
            case 20:
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }
    }
}

