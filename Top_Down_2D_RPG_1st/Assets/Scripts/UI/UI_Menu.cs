using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UI_Menu : UI_Popup
{
    enum Images
    {
        ButtonPanel,
        Save,
        Exit,
        QuestPanel
    }

    enum Texts
    {
        Text1,
        Text2,
        Text3,
        Quest
    }

    enum Buttons
    {
        Coninue,
        Save,
        Exit,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }
}

