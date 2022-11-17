using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Underworld
{
    public class RunOrDeadlMode : TotalMapMode
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedRotation;
        [Min(0)]
        [SerializeField] private float _warningTime;

        private List<Term> _deactiveTerms = new List<Term>();

        private bool _isActive = false;
        private IPatternState _curretState;
        private Coroutine _runMode;
        private int[] _direction = new int[] { -1, 1 };

        public override void SetConfig(PaternConfig config)
        {
            if (config is RunOrDeadConfig runOrDeadConfig)
            {
                workDuration = runOrDeadConfig.WorkDuration;
                _warningTime = runOrDeadConfig.WarningTime;
                _speedRotation = runOrDeadConfig.SpeedRotation;
            }
            else
            {
                throw new System.NullReferenceException("RunOrDeadConfig is null");
            }
        }

        public override bool Play()
        {
            if (_runMode == null)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(StartMode());
                return true;
            }
            return false;
        }
        private void Update()
        {
            
        }
        private IEnumerator StartMode()
        {
            yield return null;
            var mapActive = GetActiveTerms(_deactiveTerms);
            foreach (var term in mapActive)
            {
                term.Show();
            }
            var speed = _speedRotation * ChooseDirection();
            yield return WaitTime(_warningTime);
            foreach (var term in mapActive)
            {
                term.Activate(FireState.Stay);
            }
            transform.rotation *= Quaternion.Euler(Vector3.forward *
                DefineStartAngel(player.transform.position) * Time.deltaTime);
            yield return Rotate(speed);
            _runMode = null;
        }
        private IEnumerator Rotate(float speed)
        {
            _isActive = true;
            float progress = 0f;
            while (progress <= 1 && State != ModeState.Stop)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                progress += Time.deltaTime / workDuration;
                transform.rotation *= Quaternion.Euler(Vector3.forward * speed * Time.deltaTime);
                yield return null;
            }
            yield return WaitHideMap();
            _isActive = false;
            State = ModeState.Stop;
        }
        private float DefineStartAngel(Vector2 player)
        {
            var angel = Vector2.Angle(transform.right, player);
            if (player.y < transform.position.y)
                return -angel;
            return angel;
        }
        private int ChooseDirection()
        {
            int index = Random.Range(0, _direction.Length);
            return _direction[index];
        }
        private List<Term> GetActiveTerms(List<Term> deactiveTerms)
        {
            var list = new List<Term>();
            foreach (var term in terms)
            {
                if (!deactiveTerms.Contains(term))
                {
                    list.Add(term);
                }
            }
            return list;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                _deactiveTerms.Add(term);
                if (_isActive)
                {
                    if(term.IsActive)
                        term.Deactivate(false);
                    if(term.IsShow)
                        term.Hide();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Term term))
            {
                _deactiveTerms.Remove(term);
                if (_isActive)
                {
                    if(!term.IsShow)
                        term.Show();
                    term.Activate(FireState.Stay);
                }
            }
        }
    }
}