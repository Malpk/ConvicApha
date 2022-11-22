using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbillityEffectCell : MonoBehaviour
{

    [SerializeField] private Color _colorActive;
    [SerializeField] private Color _colorDeactive;
    [Header("Reference")]
    [SerializeField] private Image _icon;
    [SerializeField] private Animator _effects;
    [SerializeField] private TextMeshProUGUI _hotKey;
    [SerializeField] private TextMeshProUGUI _kdTimer;
    public void Intializate(Sprite sprite, bool handAbillity)
    {
        _icon.sprite = sprite;
        _hotKey.gameObject.SetActive(handAbillity);
    }

    public void SetState(bool mode)
    {
        _effects.SetBool("Active", mode);
        _icon.color = mode ? _colorActive : _colorDeactive;
        _kdTimer.gameObject.SetActive(!mode);
    }

    public void UpdateTime(float second)
    {
        second = Mathf.Clamp(second, 0, 99);
        _kdTimer.text = second.ToString();
    }


}
