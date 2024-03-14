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

        talkData.Add(1000, new string[] { "안녕?:0", "이 곳에 처음 왔구나?:1" });
        talkData.Add(2000, new string[] { "여어.:1", "이 호수는 정말 아름답지?:0", "사실 이 호수에는 무언가의 비밀이 숨겨져있다고해.:1" });
        talkData.Add(100, new string[] { "평범한 나무상자다."});
        talkData.Add(200, new string[] { "누군가 사용했던 흔적이 있는 책상이다."});

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
        if (talkIndex == talkData[id].Length)
            return null;
        
        return talkData[id][talkIndex];
    }
}

