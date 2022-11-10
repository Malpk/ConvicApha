using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleEnd : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoclip;
    private void Start()
    {
        StartCoroutine(EndEvent());
    }
    public IEnumerator EndEvent()
    {
        yield return new WaitForSeconds((float)_videoclip.length);
        Application.Quit();
        yield return null;
    }
}
