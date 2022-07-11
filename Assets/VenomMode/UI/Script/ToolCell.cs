using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(Animator))]
    public class ToolCell : MonoBehaviour
    {
        [Header("Requred Setting")]
        [SerializeField]private Image _toolCell;

        private Animator _aniamtor;

        public bool IsBusy => _toolCell.sprite != null;

        private void Awake()
        {
            _toolCell.sprite = null;
            _toolCell.enabled = false;
            _aniamtor = GetComponent<Animator>();
        }

        public void ShowHint()
        {
            if (!IsBusy)
                _aniamtor.SetTrigger("Messange");
        }
        public void SetSprite(Sprite sprite)
        {
            if (!IsBusy)
            {
                _toolCell.sprite = sprite;
                _toolCell.enabled = true;
            }
        }
    }
}