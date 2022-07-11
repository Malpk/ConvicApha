using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    public class Triger : MonoBehaviour,IPause
    {
        [SerializeField] private bool _onStart = true;
        [SerializeField] private float _speedTriger;

        [Header("Requred Reference")]
        [SerializeField] private Image _triger;
        [SerializeField] private Image _red;
        [SerializeField] private Image _green;
        [SerializeField] private Image _yellow;

        private float _rightPosition;
        private bool _pause = false;
        private Coroutine _corotine;

        private void Awake()
        {
            _rightPosition = _triger.rectTransform.localPosition.x;
        }

        private void Start()
        {
            if(_onStart)
                Run();
        }
        #region Pause
        public void Pause()
        {
            _pause = true;
        }

        public void UnPause()
        {
            _pause = false;
        }
        #endregion
        public void Run()
        {
            if (_corotine == null)
                _corotine = StartCoroutine(Move(-_rightPosition));
        }

        private IEnumerator Move(float target)
        {
            for (int i = 0; i < 3; i++)
            {
                bool enter = false;
                while (!enter)
                {
                    while (_triger.rectTransform.localPosition.x != target && !enter)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            enter = true;
                        }
                        else
                        {
                            _triger.rectTransform.localPosition = new Vector3(
                                Mathf.MoveTowards(_triger.rectTransform.localPosition.x, target,
                                    _speedTriger * Time.deltaTime), _triger.rectTransform.localPosition.y,
                                        _triger.rectTransform.localPosition.z);
                            yield return null;
                            yield return new WaitWhile(() => _pause);
                        }
                    }
                    target = -target;
                }
                target = Mathf.Abs(target);
                _triger.rectTransform.localPosition = new Vector3(-target,
                    _triger.rectTransform.localPosition.y, _triger.rectTransform.localPosition.z);
                yield return null;
            }
         
        }
        public void Drop()
        {
            
        }


    }
}