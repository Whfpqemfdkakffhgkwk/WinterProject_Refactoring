using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyAlbum : MonoBehaviour
{
    [SerializeField] private Sprite[] EndingSpr;

    private int CurEnding;

    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(EndingChange);
    }

    void EndingChange()
    {
        CurEnding++;
        if(CurEnding == EndingSpr.Length)
            CurEnding = 0;
        this.gameObject.GetComponent<Image>().sprite = EndingSpr[CurEnding];
    }
}
