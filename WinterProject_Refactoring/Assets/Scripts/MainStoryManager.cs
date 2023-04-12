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
        string ReplaceTalk = str.Replace("'주인공'", SD.PlayerName);
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
                TalkerText.text = "양세화";
                break;
            case ECharacterType.Kang:
                TalkerText.text = "강채아";
                break;
            case ECharacterType.Baek:
                TalkerText.text = "백가윤";
                break;
            default:
                TalkerText.text = "주인공";
                break;
        }
    }

    /// <summary>
    /// 메인 스토리 진행(대사를 끝까지 반복)하는 코루틴
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
        print("다음 씬");
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
