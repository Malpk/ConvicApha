using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UserIntaface.MainMenu
{
    [CreateAssetMenu(menuName ="Create Description Character",fileName = "DescriptionCharacter",order =2)]
    public class DescriptionCharacter: ScriptableObject
    {
        public string Name;
        [TextArea]
        public string Description;
    }
}
