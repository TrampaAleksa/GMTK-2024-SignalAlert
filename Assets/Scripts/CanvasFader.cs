using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private bool _isFadeIn=true;
    [SerializeField] private float duration;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void FadeCanvas(bool isFadeIn)
    {
        Debug.Log(isFadeIn);
        Debug.Log(_isFadeIn);
        if (_isFadeIn.Equals(isFadeIn)) return;
        _isFadeIn = isFadeIn;
        StartCoroutine(FadeAnimation(isFadeIn));
    }
    private IEnumerator FadeAnimation(bool isFadeIn)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float val01 = time / duration;
            Debug.Log(val01);
            _canvasGroup.alpha = isFadeIn? val01 : 1-val01;
            yield return null;
        }
    }
}
