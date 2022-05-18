using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Underworld.Editors
{
    [CreateAssetMenu(menuName = "UnderWorld/UnderWorldsEditor")][System.Serializable]
    public class ModeSwitchController : ScriptableObject
    {

        [SerializeField] private List<Seqcunce> SeqcuncePatern = new List<Seqcunce>();

        public IReadOnlyList<Seqcunce> Seqcuncs => SeqcuncePatern;

        private void OnEnable()
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
        public void Add(Seqcunce seqcunce)
        {
            SeqcuncePatern.Add(seqcunce);
        }
        public void Remove(Seqcunce seqcunce)
        {
            SeqcuncePatern.Remove(seqcunce);
        }
    }
}