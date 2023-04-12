using Live2D.Cubism.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ECharacterType
{
    Yang,
    Kang,
    Baek
}

public enum EYangState
{
    Normal,
    Contempt,
    Worry,
    OpenMouth,
    Tear,
    Shy,
    Smile,
    Flushing,
    FlushingSmile,
    Angry,
    None
}
public enum EKangState
{
    Normal,
    Worry,
    Contempt,
    OpenMouth,
    Amazing,
    Shy,
    Sad,
    Smile,
    FlushingOpenMouth,
    Flushing,
    FlushingSmile,
    Angry,
    None
}
public enum EBaekState
{
    Normal,
    Worry,
    Contempt,
    OpenMouth,
    Amazing,
    Shy,
    Absurd,
    Smile1,
    Smile2,
    FlushingSmile,
    Angry,
    None
}

public class AnimatorManager : Singleton<AnimatorManager>
{
    public int YangState, KangState, BaekState;


    public Animator YangAni, KangAni, BaekAni;

    private CubismRenderController YangRender, KangRender, BaekRender;


    private new void Awake()
    {
        YangRender = YangAni.GetComponent<CubismRenderController>();
        KangRender = KangAni.GetComponent<CubismRenderController>();
        BaekRender = BaekAni.GetComponent<CubismRenderController>();
        YangRender.Opacity = 0;
        KangRender.Opacity = 0;
        BaekRender.Opacity = 0;
    }

    /// <summary>
    /// 캐릭터 Live2D 상태 변환
    /// </summary>
    /// <param name="type"></param>
    /// <param name="num"></param>
    public void CharacterChange(ECharacterType type, int num)
    {
        switch (type)
        {
            case ECharacterType.Yang:
                YangRender.Opacity = 1;
                YangState = num;
                //애니메이션이 None일 경우 투명화 시키기
                if (YangState == (int)EYangState.None)
                {
                    YangRender.Opacity = 0;
                    return;
                }

                //아닐경우 애니메이션 전환
                YangAni.SetInteger("num", YangState);
                break;
            case ECharacterType.Kang:
                KangRender.Opacity = 1;
                KangState = num;
                //애니메이션이 None일 경우 투명화 시키기
                if (KangState == (int)EKangState.None)
                {
                    KangRender.Opacity = 0;
                    return;
                }
                //아닐경우 애니메이션 전환
                KangAni.SetInteger("num", KangState);
                break;
            case ECharacterType.Baek:
                BaekRender.Opacity = 1;
                BaekState = num;
                //애니메이션이 None일 경우 투명화 시키기
                if (BaekState == (int)EBaekState.None)
                {
                    BaekRender.Opacity = 0;
                    return;
                }

                //아닐경우 애니메이션 전환
                BaekAni.SetInteger("num", BaekState);
                break;
        }
    }

}
