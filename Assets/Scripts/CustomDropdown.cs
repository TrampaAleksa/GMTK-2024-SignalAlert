using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class CustomDropdown : MonoBehaviour
{
    private bool _isAnim = false;
    private bool _isOpen=false;
    private float _width = 0f;
    private Button _btn;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private RectTransform _rectTransform;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(ToggleDropdown);
    }
    private void ToggleDropdown() => ToggleDropdown(_duration);
    private void ToggleDropdown(float duration)
    {
        if (_isAnim) return;
        _isOpen = !_isOpen;
        StartCoroutine(ToggleAnimation(duration,_isOpen));
    }
    private IEnumerator ToggleAnimation(float duration, bool toggle)
    {
        _width = _rectTransform.sizeDelta.x;
        float max = toggle ? _width : 0f;
        float min = toggle ? 0f : _width;
        float time = 0f;
        float y = _rectTransform.anchoredPosition.y;
        while (time < duration)
        {
            time+= Time.deltaTime;
            float v01 = time / duration;
            float x = (max - min) * v01 + min;
            _rectTransform.anchoredPosition= new Vector2(x, y);
            yield return null;
        }
    }
}
