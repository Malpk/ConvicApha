using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/UnderWorldsEditor")][System.Serializable]
    public class ModeSwitchController : ScriptableObject
    {
        [SerializeField] private string _fuckField;
        [SerializeField] private List<Seqcunce> SeqcuncePatern = new List<Seqcunce>();

        private string Path => Application.dataPath + "\\Makc\\Script\\Editor\\" + $"{name}.asset";

        public IReadOnlyList<Seqcunce> Seqcuncs => SeqcuncePatern;
        public void OnEnable()
        {
            Load();
        }
        public void Load()
        {
            foreach (var sequnce in SeqcuncePatern)
            {
                sequnce.Load();
            }
        }
        private void OnDisable()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }
        public void Add(Seqcunce seqcunce)
        {
            SeqcuncePatern.Add(seqcunce);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        public void Remove(Seqcunce seqcunce)
        {
            SeqcuncePatern.Remove(seqcunce);
        }
    }
}