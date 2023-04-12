using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Cannes;

    SaveData SD;

    private void Awake()
    {
        SD = GameManager.Instance.saveData;
    }

    private void OnEnable()
    {
        bool[] isActives = new bool[] {SD.KangNormalEnd, SD.YangNormalEnd, SD.BaekNormalEnd, SD.KangHappyEnd, SD.YangHappyEnd, SD.BaekHappyEnd };

        for (int i = 0; i < Cannes.Length; i++)
        {
            Cannes[i].SetActive(isActives[i]);
        }
    }
}
