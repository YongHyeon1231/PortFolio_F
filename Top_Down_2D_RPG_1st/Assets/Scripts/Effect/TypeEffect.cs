using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public float CharPerSeconds;
    public GameObject End_Cursor;
    public bool isAnim;

    private string targetMsg;
    Text msgText;
    AudioSource audioSource;
    int index;
    float interval;

    private void Awake()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }
    public void SetMsg(string msg)
    {
        if (isAnim) //Interrupt
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        End_Cursor.SetActive(false);

        //Start Animation
        interval = 1.0f / CharPerSeconds;

        isAnim = true;
        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];

        //Sound
        if (targetMsg[index] != ' ' && targetMsg[index] != '.')
            audioSource.Play();

        index++;

        //Recursive
        Invoke("Effecting", 1 / CharPerSeconds);
    }

    void EffectEnd()
    {
        isAnim = false;
        End_Cursor.SetActive(true);
    }
}

