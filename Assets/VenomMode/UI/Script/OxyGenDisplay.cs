using MainMode.GameInteface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    public class OxyGenDisplay : MonoBehaviour
    {
        [Header("Requred Reference")]
        [SerializeField] private Image _fieldImage;

        public void UpdateField(float value)
        {
            _fieldImage.fillAmount =Mathf.Clamp01(value);
        }
    }
}