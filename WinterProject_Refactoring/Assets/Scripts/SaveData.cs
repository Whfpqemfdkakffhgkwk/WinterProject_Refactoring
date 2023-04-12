using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Header("Character")]
    public string PlayerName;

    [Header("AlbumOpen")]
    public bool KangNormalEnd;
    public bool KangHappyEnd;
    public bool YangNormalEnd;
    public bool YangHappyEnd;
    public bool BaekNormalEnd;
    public bool BaekHappyEnd;

    [Header("SaveDate")]
    public string[] SaveDates;

    [Header("Cur Progress")]
    public int MainStoryProgress;
    public int YangStoryProgress, KangStoryProgress, BaekStoryProgress;
    public int YangFavorabilityScore, KangFavorabilityScore, BaekFavorabilityScore;
    public int YangGiftCount, KangGiftCount, BaekGiftCount;
    public List<EItemType> RemainingItem;

}
