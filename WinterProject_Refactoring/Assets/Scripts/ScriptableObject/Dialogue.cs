using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StoryData
{
    public string Speech;
    public int Talker;
    public EYangState Yang;
    public EKangState Kang;
    public EBaekState Baek;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Object/Dialogue Data")]
public class Dialogue : ScriptableObject
{
    public StoryData[] storyData;
}
