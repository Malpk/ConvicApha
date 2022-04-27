using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Underworld
{
    [System.Serializable]
    public class BaseSetting 
    {
        [Header("Spawn Setting")]
        [Min(0)]
        [SerializeField] private int _spawnDistance;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _delay;
        [Min(0)]
        [SerializeField] private float _timeFireActive;
        [Min(0)]
        [SerializeField] private float _workDuration;

        public ModeTypeNew type => ModeTypeNew.BaseMode;

        public int SpawnDistance => _spawnDistance;
        public float Delay => _delay;
        public float TimeFireActive => _timeFireActive;
        public float WorDuration => _workDuration;



        //public VisualElement[] Fields =>
    }
}