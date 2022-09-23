using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MainMode.GameInteface;

namespace MainMode.Mode1921
{
    public class ChangeTest : UserInterface
    {
        [Header("General Setting")]
        [Min(0)]
        [SerializeField] private float _timeDelay = 1;

        [Header("FontMessange Setting")]
        [SerializeField] private float _fontMessangeStart;
        [SerializeField] private float _fontMessangeResult;
        [SerializeField] private string _startMessnage;

        [Header("Requred Reference")]
        [SerializeField] private Triger _triger;
        [SerializeField] private TextMeshProUGUI _messnga;

        private Coroutine _runGame;

        public bool IsRepairShield => _runGame != null;
        public override UserInterfaceType Type => UserInterfaceType.Other;

        public delegate void Action(int countComplite);
        public event Action CompliteGame;

        public void RunGame(OxyGenSet oxyGen, int countTest)
        {
            if (_runGame == null)
            {
                if (swithchInteface != null)
                {
                    swithchInteface.SetShow(this);
                }
                else
                {
                    OnShow();
                }
                SetMessange(_startMessnage, Color.white, _fontMessangeStart);
                _runGame = StartCoroutine(ChangeTestUpdate(oxyGen,countTest));
            }
        }
        private IEnumerator ChangeTestUpdate(OxyGenSet oxyGen,int countTest)
        {
            var hotKey = KeyCode.Space;
            int complite = 0;
            for (complite = 0; complite < countTest && oxyGen.CurretAirSupply > 0; complite++)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(hotKey));
                _messnga.enabled = false;
                _triger.Run();
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(hotKey));
                ReadMessange(_triger.GetMessange(),oxyGen);
                _triger.TurnOff();
                yield return null;
                var progress = 0f; 
                while (progress <= 1f && !Input.GetKeyDown(hotKey))
                {
                    progress += Time.deltaTime / _timeDelay;
                    yield return null;
                }
            }
            if (swithchInteface != null)
                swithchInteface.SetHide();
            else
                Hide();
            if (CompliteGame != null)
                CompliteGame((complite + 1));
            _runGame = null;
        }
        private void ReadMessange(MessangeRepairTest messange, OxyGenSet oxyGen)
        {
            oxyGen.ReduceFilltre(messange.ReduceValue);
            SetMessange(messange.Messange, messange.TextColor, _fontMessangeResult);
        }
        private void SetMessange(string messange, Color color,float fontSize)
        {
            _messnga.enabled = true;
            _messnga.text = messange;
            _messnga.fontSize = fontSize;
            _messnga.outlineColor = color;
        }
    }
}