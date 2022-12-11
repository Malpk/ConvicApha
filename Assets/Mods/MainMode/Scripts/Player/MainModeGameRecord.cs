using UnityEngine;

namespace MainMode
{
    public class MainModeGameRecord : MonoBehaviour
    {
        [SerializeField] private string _savePath = "veryBigGameRecords";
        [SerializeField] private TimerDisplay _timer;

        private float _time;

        public void Play()
        {
            _time = 0f;
            enabled = true;
        }
        public void Stop()
        {
            var bestRecord = GetBestRecord();
            var intTime = (int)_time;
            if (intTime > bestRecord)
                PlayerPrefs.SetInt(_savePath, intTime);
            enabled = false;
        }

        public int GetBestRecord()
        {
            if (PlayerPrefs.HasKey(_savePath))
            {
                return PlayerPrefs.GetInt(_savePath);
            }
            return 0;
        }

        void Update()
        {
            _time += Time.deltaTime;
            _timer.Output((int)_time);
        }
    }
}