using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;
    Sprite[] portraitArr;

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

    public void Init()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    private void GenerateData()
    {
        portraitArr = Resources.LoadAll<Sprite>("Sprite");

        //Talk Data
        talkData.Add(1000, new string[] { "안녕?:0",
                                          "이 곳에 처음 왔구나?:1" });
        talkData.Add(2000, new string[] { "여어.:1", 
                                          "이 호수는 정말 아름답지?:0", 
                                          "사실 이 호수에는 무언가의 비밀이 숨겨져있다고해.:1" });
        talkData.Add(100, new string[] { "평범한 나무상자다."});
        talkData.Add(200, new string[] { "누군가 사용했던 흔적이 있는 책상이다."});

        //Quest Talk
        talkData.Add(1000 + 10, new string[] { "어서 와.:0",
                                               "이 마을에 놀라운 전설이 있다는데:1",
                                               "왼쪽 집 쪽에 루도가 알려줄꺼야.:0"});
        talkData.Add(2000 + 11, new string[] { "어서 와.:0",
                                               "호수의 전설을 들으러 온거야?:1",
                                               "그럼 일 좀 하나 해주면 좋을텐데...:0",
                                               "우리 집 근처에 떨어진 동전좀 찾아줬으면 해.:1"});

        talkData.Add(1000 + 20, new string[] { "루도의 동전?:1",
                                               "돈을 흘리고 다니면 못쓰지!:3",
                                               "나중에 루도에게 한마디 해야겠어.:3"});
        talkData.Add(2000 + 20, new string[] { "찾으면 꼭 좀 가져다 줘.:1" });
        talkData.Add(5000 + 20, new string[] { "근처에서 동전을 찾았다." });

        talkData.Add(2000 + 21, new string[] { "엇, 찾아줘서 고마워.:2" });

        // TODO
        for (int i = 0; i < portraitArr.Length; i++)
        {
            if (i < 4)
                portraitData.Add(1000 + i, portraitArr[i]);
            else if (4 <= i)
                portraitData.Add(2000 + i - 4, portraitArr[i]);
        }

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkData.ContainsKey(id) == false)
        {
            if (talkData.ContainsKey(id - 10 % 10) == false)
            {
                // Get First Talk
                return GetTalk(id - id % 100, talkIndex);
            }
            else
            {
                // Get First Quest Talk
                return GetTalk(id - id % 10, talkIndex);
            }
        }

        if (talkIndex == talkData[id].Length)
            return null;
        
        return talkData[id][talkIndex];
    }
}

