using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public enum EStoryBackground
{
    Road,
    Classroom,
    Library,
    Corridor,
    BackYard
}

public class MainStoryManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TalkText, TalkerText;
    [SerializeField] private Sprite[] BackgroundSprs;
    [SerializeField] private Image Background, Progress;
    [SerializeField] private GameObject Menu;

    [SerializeField] private Dialogue dialogue;
    private bool isTyping = false, readyNext = false, isSetting = false;
    private Tweener TypingTween;
    private SaveData SD;
    private AnimatorManager animatorManager;

    private void Start()
    {
        animatorManager = AnimatorManager.Instance;
        SD = GameManager.Instance.saveData;
        Progress.fillAmount = (float)SD.MainStoryProgress / dialogue.storyData.Length;
        StartCoroutine(StoryProgress());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        {
            TypingTween.timeScale = 15f;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            readyNext = true;
        }
    }

    void Typing(string str)
    {
        string ReplaceTalk = str.Replace("'���ΰ�'", SD.PlayerName);
        TalkText.text = "";
        isTyping = true;
        TypingTween = TalkText.DOText(ReplaceTalk, ReplaceTalk.Length * 0.07f).SetEase(Ease.Linear)
            .OnComplete(() => isTyping = false).SetEase(Ease.Linear).OnComplete(() => isTyping = false)
            .SetEase(Ease.Linear).OnComplete(() => isTyping = false);
    }

    void TalkerChange(ECharacterType type)
    {
        switch (type)
        {
            case ECharacterType.Yang:
                TalkerText.text = "�缼ȭ";
                break;
            case ECharacterType.Kang:
                TalkerText.text = "��ä��";
                break;
            case ECharacterType.Baek:
                TalkerText.text = "�鰡��";
                break;
            default:
                TalkerText.text = "���ΰ�";
                break;
        }
    }

    /// <summary>
    /// ���� ���丮 ����(��縦 ������ �ݺ�)�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator StoryProgress()
    {
        int StoryLength = dialogue.storyData.Length;

        for (int i = SD.MainStoryProgress; i < StoryLength; i++)
        {
            readyNext = false;

            animatorManager.CharacterChange(ECharacterType.Yang, (int)dialogue.storyData[SD.MainStoryProgress].Yang);
            animatorManager.CharacterChange(ECharacterType.Kang, (int)dialogue.storyData[SD.MainStoryProgress].Kang);
            animatorManager.CharacterChange(ECharacterType.Baek, (int)dialogue.storyData[SD.MainStoryProgress].Baek);

            TalkerChange((ECharacterType)dialogue.storyData[i].Talker);

            Typing(dialogue.storyData[i].Speech);

            while (!readyNext)
            {
                yield return null;
            }
            SD.MainStoryProgress++;
            Progress.fillAmount = (float)SD.MainStoryProgress / dialogue.storyData.Length;
        }
        SD.MainStoryProgress = 0;
        print("���� ��");
    }

    public void MenuClick()
    {
        if (!isSetting)
        {
            Menu.transform.DOLocalMoveX(Menu.transform.position.x - 361, 0.5f);
            isSetting = true;
        }
        else
        {
            Menu.transform.DOLocalMoveX(Menu.transform.position.x, 0.5f);
            isSetting = false;
        }
    }
}
