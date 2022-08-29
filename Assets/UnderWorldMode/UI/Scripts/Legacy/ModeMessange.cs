using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "Messange/UnderworldMessange")]
    public class ModeMessange : ScriptableObject
    {
        [SerializeField] private List<MessangeElement> _modeMessgangs;
        [SerializeField] private List<string> _baseMessangs;

        private List<string> _messangeList = new List<string>(); 

        public string GetMessgange(ModeType typeMode = ModeType.BaseMode)
        {
            var listMessange = FiltrModeMessange(typeMode);
            if (_messangeList.Count == 0)
                _messangeList.AddRange(_baseMessangs);
            listMessange.AddRange(_messangeList);
            var index = Random.Range(0, listMessange.Count);
            _messangeList.Remove(listMessange[index]);
            return listMessange[index];
        }
        private List<string> FiltrModeMessange(ModeType typeMode)
        {
            var listMessange = new List<string>();
            for (int i = 0; i < _modeMessgangs.Count; i++)
            {
                if (_modeMessgangs[i].type == typeMode)
                    listMessange.Add(_modeMessgangs[i].messange);
            }
            return listMessange;
        }
    }
}