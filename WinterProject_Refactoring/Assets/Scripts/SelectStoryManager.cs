using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Reflection;
using Unity.VisualScripting;

public enum EItemType
{
    TeddyBear,
    Flower,
    Candy,
    LoveLetter,
    Book,
    Chocolate,
    CoffeeMilk,
    Pencil,
    BlackTea
}

public class SelectStoryManager : MonoBehaviour
{
    [Header("Background Related")]
    [SerializeField] private Image Background;
    [SerializeField] private Sprite[] BackgroundSprs;
    [Header("Start Btns")]
    [SerializeField] private Button[] StartButtons;
    [Header("Select Scene Obj")]
    [SerializeField] private GameObject SelectScene;
    [Header("Yang Text Files")]
    [SerializeField] private Dialogue YangHappy;
    [SerializeField] private Dialogue YangNormal, YangBad, YangDialogue, YangGift;
    [Header("Kang Text Files")]
    [SerializeField] private Dialogue KangHappy;
    [SerializeField] private Dialogue KangNormal, KangBad, KangDialogue, KangGift;
    [Header("Baek Text Files")]
    [SerializeField] private Dialogue BaekHappy;
    [SerializeField] private Dialogue BaekNormal, BaekBad, BaekDialogue, BaekGift;
    [Header("Character Position")]
    [SerializeField] private Vector3 KangSelectPos;
    [SerializeField] private Vector3 YangSelectPos, BaekSelectPos;
    private Vector3 KangFirstPos, YangFirstPos, BaekFirstPos;
    [Header("Gift")]
    [SerializeField] private Image ItemImage;
    [SerializeField] private GameObject GiftPopup, ExplanPopup;
    [SerializeField] private Text NameText, ExplanText;
    [SerializeField] private Button GiveButton, ExitGiftButton;
    [SerializeField] private Button[] GiftButtons;
    [SerializeField] private Sprite[] GiftSprs;
    [Header("Select UI")]
    [SerializeField] private Slider LikingSlider; 
    [SerializeField] private ItemText itemText;

    [SerializeField] private TextMeshProUGUI MainText;

    private Tweener TypingTween;
    private SaveData SD;
    private AnimatorManager AM;
    private void Start()
    {
        AM = AnimatorManager.Instance;
        SD = GameManager.Instance.saveData;

        KangFirstPos = AM.KangAni.transform.position;
        YangFirstPos = AM.YangAni.transform.position;
        BaekFirstPos = AM.BaekAni.transform.position;

        for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
        {
            AM.CharacterChange((ECharacterType)i, 0);
        }

        AM.CharacterChange(ECharacterType.Yang, 0);
        AM.CharacterChange(ECharacterType.Kang, 0);
        AM.CharacterChange(ECharacterType.Baek, 0);

        StartButtons[(int)ECharacterType.Kang].onClick.AddListener(() =>
        {
            //배경전환
            Background.sprite = BackgroundSprs[(int)EStoryBackground.BackYard];
            //캐릭터 위치 이동
            AM.KangAni.gameObject.transform.position = KangSelectPos;
            //말하는 캐릭터 전환(다른 캐릭터 오퍼시티 0)
            for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
            {
                if (i == (int)ECharacterType.Kang)
                    AM.CharacterChange((ECharacterType)i, 0);
                else if (i == (int)ECharacterType.Yang)
                    AM.CharacterChange((ECharacterType)i, (int)KangDialogue.storyData[0].Yang);
                else if (i == (int)ECharacterType.Baek)
                    AM.CharacterChange((ECharacterType)i, (int)KangDialogue.storyData[0].Baek);

            }
            //대화하기 선물 나가기 버튼 열기(각자 다른 이벤트 Add하기 위한 함수)
            ShowButton(ECharacterType.Kang);
        });
        StartButtons[(int)ECharacterType.Yang].onClick.AddListener(() =>
        {
            //배경전환
            Background.sprite = BackgroundSprs[(int)EStoryBackground.Library];
            //캐릭터 위치 이동
            AM.YangAni.gameObject.transform.position = YangSelectPos;
            //말하는 캐릭터 전환(다른 캐릭터 오퍼시티 0)
            for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
            {
                if (i == (int)ECharacterType.Yang)
                    AM.CharacterChange((ECharacterType)i, 0);
                else if (i == (int)ECharacterType.Kang)
                    AM.CharacterChange((ECharacterType)i, (int)YangDialogue.storyData[0].Kang);
                else if (i == (int)ECharacterType.Baek)
                    AM.CharacterChange((ECharacterType)i, (int)YangDialogue.storyData[0].Baek);

            }
            //대화하기 선물 나가기 버튼 열기(각자 다른 이벤트 Add하기 위한 함수)
            ShowButton(ECharacterType.Yang);
        });
        StartButtons[(int)ECharacterType.Baek].onClick.AddListener(() =>
        {
            //배경전환
            Background.sprite = BackgroundSprs[(int)EStoryBackground.Classroom];
            //캐릭터 위치 이동
            AM.BaekAni.gameObject.transform.position = BaekSelectPos;
            //말하는 캐릭터 전환(다른 캐릭터 오퍼시티 0)
            for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
            {
                if (i == (int)ECharacterType.Baek)
                    AM.CharacterChange((ECharacterType)i, 0);
                else if (i == (int)ECharacterType.Kang)
                    AM.CharacterChange((ECharacterType)i, (int)BaekDialogue.storyData[0].Kang);
                else if (i == (int)ECharacterType.Yang)
                    AM.CharacterChange((ECharacterType)i, (int)BaekDialogue.storyData[0].Yang);

            }
            //대화하기 선물 나가기 버튼 열기(각자 다른 이벤트 Add하기 위한 함수)
            ShowButton(ECharacterType.Baek);
        });
    }

    private void ShowButton(ECharacterType type)
    {
        SelectScene.gameObject.SetActive(true);

        for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
        {
            StartButtons[i].gameObject.SetActive(false);
        }

        AddListenerByCharacterType(type);

        ExitGiftPopup();
    }

    private void AddListenerByCharacterType(ECharacterType type)
    {
        //캐릭터들에 따라 대화, 선물, 나가기 버튼 AddListener
        switch (type)
        {
            case ECharacterType.Yang:
                SelectScene.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                {

                });
                SelectScene.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    for (int i = 0; i < SD.RemainingItem.Count; i++)
                    {
                        GiftButtons[(int)SD.RemainingItem[i]].gameObject.SetActive(true);
                    }
                    GiftPopup.SetActive(true);
                    LikingSlider.value = SD.YangFavorabilityScore / 100;
                    for (int i = 0; i < Enum.GetValues(typeof(EItemType)).Length; i++)
                    {
                        //클로저 해결
                        int index = i;

                        GiftButtons[index].onClick.RemoveAllListeners();
                        GiftButtons[index].onClick.AddListener(() =>
                        {
                            ShowExplanInfo(index);

                            if (SD.YangGiftCount > 0)
                            {
                                GiveButton.onClick.RemoveAllListeners();
                                GiveButton.onClick.AddListener(() =>
                                {
                                    //특별한 상호작용 하는 아이템들을 줬을때 특별한 상호작용을 하게 하는 AddListener
                                    switch ((EItemType)index)
                                    {
                                        case EItemType.Flower:
                                            SD.YangFavorabilityScore -= 20;
                                            GiveItem(ECharacterType.Yang, (int)YangGift.storyData[1].Yang,
                                                YangGift.storyData[1].Speech, EItemType.Flower, SD.YangFavorabilityScore);
                                            break;
                                        case EItemType.LoveLetter:
                                            GiveItem(ECharacterType.Yang, (int)YangGift.storyData[2].Yang,
                                                YangGift.storyData[2].Speech, EItemType.LoveLetter, SD.YangFavorabilityScore);
                                            break;
                                        case EItemType.Book:
                                            SD.YangFavorabilityScore += 20;
                                            GiveItem(ECharacterType.Yang, (int)YangGift.storyData[0].Yang,
                                                YangGift.storyData[0].Speech, EItemType.Book, SD.YangFavorabilityScore);
                                            break;
                                        default:
                                            GiveItem(ECharacterType.Yang, (int)YangGift.storyData[3].Yang,
                                                YangGift.storyData[3].Speech, (EItemType)index, SD.YangFavorabilityScore);
                                            break;
                                    }
                                    SD.YangGiftCount--;
                                    HideExplanInfo();
                                });
                            }

                        });
                    }
                });
                SelectScene.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AM.YangAni.transform.position = YangFirstPos;
                    Background.sprite = BackgroundSprs[(int)EStoryBackground.Corridor];
                    SelectScene.SetActive(false);
                    for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
                    {
                        StartButtons[i].gameObject.SetActive(true);
                        AM.CharacterChange((ECharacterType)i, 0);
                    }
                });
                break;
            case ECharacterType.Kang:
                SelectScene.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                {

                });
                SelectScene.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    for (int i = 0; i < SD.RemainingItem.Count; i++)
                    {
                        GiftButtons[(int)SD.RemainingItem[i]].gameObject.SetActive(true);
                    }
                    GiftPopup.SetActive(true);
                    for (int i = 0; i < Enum.GetValues(typeof(EItemType)).Length; i++)
                    {
                        int index = i;
                        GiftButtons[index].onClick.RemoveAllListeners();
                        GiftButtons[index].onClick.AddListener(() =>
                        {
                            ShowExplanInfo(index);


                            if (SD.KangGiftCount > 0)
                            {
                                GiveButton.onClick.RemoveAllListeners();
                                GiveButton.onClick.AddListener(() =>
                                {
                                    //특별한 상호작용 하는 아이템들을 줬을때 특별한 상호작용을 하게 하는 AddListener
                                    switch ((EItemType)index)
                                    {
                                        case EItemType.TeddyBear:
                                            GiveItem(ECharacterType.Kang, (int)KangGift.storyData[0].Kang,
                                                KangGift.storyData[0].Speech, EItemType.TeddyBear, SD.KangFavorabilityScore);
                                            break;
                                        case EItemType.BlackTea:
                                            GiveItem(ECharacterType.Kang, (int)KangGift.storyData[1].Kang,
                                                KangGift.storyData[1].Speech, EItemType.BlackTea, SD.KangFavorabilityScore);
                                            break;
                                        case EItemType.LoveLetter:
                                            GiveItem(ECharacterType.Kang, (int)KangGift.storyData[2].Kang,
                                                KangGift.storyData[2].Speech, EItemType.LoveLetter, SD.KangFavorabilityScore);
                                            break;
                                        default:
                                            GiveItem(ECharacterType.Kang, (int)KangGift.storyData[3].Kang,
                                                KangGift.storyData[3].Speech, (EItemType)index, SD.KangFavorabilityScore);
                                            break;
                                    }
                                    SD.KangGiftCount--;
                                    HideExplanInfo();
                                });
                            }
                        });
                    }
                });
                SelectScene.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AM.KangAni.transform.position = KangFirstPos;
                    Background.sprite = BackgroundSprs[(int)EStoryBackground.Corridor];
                    SelectScene.SetActive(false);
                    for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
                    {
                        StartButtons[i].gameObject.SetActive(true);
                        AM.CharacterChange((ECharacterType)i, 0);
                    }
                });
                break;
            case ECharacterType.Baek:
                SelectScene.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                {

                });
                SelectScene.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    for (int i = 0; i < SD.RemainingItem.Count; i++)
                    {
                        GiftButtons[(int)SD.RemainingItem[i]].gameObject.SetActive(true);
                    }
                    GiftPopup.SetActive(true);

                    for (int i = 0; i < Enum.GetValues(typeof(EItemType)).Length; i++)
                    {
                        int index = i;

                        GiftButtons[index].onClick.RemoveAllListeners();
                        GiftButtons[index].onClick.AddListener(() =>
                        {
                            ShowExplanInfo(index);


                            if (SD.BaekGiftCount > 0)
                            {
                                GiveButton.onClick.RemoveAllListeners();
                                GiveButton.onClick.AddListener(() =>
                                {
                                    //특별한 상호작용 하는 아이템들을 줬을때 특별한 상호작용을 하게 하는 AddListener
                                    switch ((EItemType)index)
                                    {
                                        case EItemType.LoveLetter:
                                            GiveItem(ECharacterType.Baek, (int)BaekGift.storyData[1].Baek,
                                                BaekGift.storyData[1].Speech, EItemType.LoveLetter, SD.BaekFavorabilityScore);
                                            break;
                                        case EItemType.Chocolate:
                                            GiveItem(ECharacterType.Baek, (int)BaekGift.storyData[0].Baek,
                                                BaekGift.storyData[0].Speech, EItemType.Chocolate, SD.BaekFavorabilityScore);
                                            break;
                                        default:
                                            GiveItem(ECharacterType.Baek, (int)BaekGift.storyData[3].Baek,
                                                BaekGift.storyData[3].Speech, (EItemType)index, SD.BaekFavorabilityScore);
                                            break;
                                    }
                                    SD.BaekGiftCount--;
                                    HideExplanInfo();
                                });
                            }

                        });
                    }
                });
                SelectScene.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                SelectScene.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AM.BaekAni.transform.position = BaekFirstPos;
                    Background.sprite = BackgroundSprs[(int)EStoryBackground.Corridor];
                    SelectScene.SetActive(false);
                    for (int i = 0; i < Enum.GetValues(typeof(ECharacterType)).Length; i++)
                    {
                        StartButtons[i].gameObject.SetActive(true);
                        AM.CharacterChange((ECharacterType)i, 0);
                    }
                });
                break;
        }

    }


    private void FindAndRemove(EItemType type)
    {
        print(type);
        SD.RemainingItem.Remove(type);
        GiftButtons[(int)type].gameObject.SetActive(false);
    }

    private void ShowExplanInfo(int index)
    {
        ExplanPopup.transform.DOScale(1, 1);
        ExplanPopup.SetActive(true);
        ItemImage.sprite = GiftSprs[index];
        ExplanText.text = itemText.ItemExplan[index];
        NameText.text = itemText.ItemName[index];
    }

    private void HideExplanInfo()
    {
        ExplanPopup.SetActive(false);
        ExplanPopup.transform.localScale = Vector3.zero;
    }
    /// <summary>
    /// 아이템을 줬을때 실행되는 함수
    /// </summary>
    /// <param name="CharcterType">주는 캐릭터 타입</param>
    /// <param name="aniNum">줬을때 캐릭터가 하는 애니메이션</param>
    /// <param name="Speech">줬을때 캐릭터가 하는 말</param>
    /// <param name="ItemType">준 아이템 타입</param>
    /// <param name="LikingScore">호감도 점수</param>
    private void GiveItem(ECharacterType CharcterType, int aniNum, string Speech, EItemType ItemType, int LikingScore)
    {
        LikingSlider.value = LikingScore / 100;
        AM.CharacterChange(CharcterType, aniNum);
        Typing(Speech);
        FindAndRemove(ItemType);
    }

    private void ExitGiftPopup()
    {
        ExitGiftButton.onClick.RemoveAllListeners();
        ExitGiftButton.onClick.AddListener(() =>
        {
            ExplanPopup.SetActive(false);
            ExplanPopup.transform.localScale = Vector3.zero;

            GiftPopup.SetActive(false);
        });
    }

    private void Typing(string str)
    {
        string ReplaceTalk = str.Replace("'주인공'", SD.PlayerName);
        TypingTween.Kill();
        MainText.text = "";
        TypingTween = MainText.DOText(ReplaceTalk, ReplaceTalk.Length * 0.07f).SetEase(Ease.Linear);
    }
}
