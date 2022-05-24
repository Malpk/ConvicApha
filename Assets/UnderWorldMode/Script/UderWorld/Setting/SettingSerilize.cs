using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class SettingSerilize
    {
        [SerializeField] private ModeTypeNew _type;
        [SerializeField] private string _serilizeSetting;

        public SettingSerilize(ModeTypeNew type, string jsonSetting)
        {
            _type = type;
            _serilizeSetting = jsonSetting;
        }

        public ModeTypeNew Type => _type;
        public string Setting => _serilizeSetting;
    }
}