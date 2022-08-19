using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserIntaface.MainMenu
{
    public class CharacterScroller : BaseScroller<CharacterView>
    {
        public Action<DescriptionCharacter> ChangeCharacter;
        protected override void Awake()
        {
            base.Awake();
            _ringList = new RingListCharacter(_listPrefabs, _transforms, _parentElements);          
        }
        private void Start()
        {
            ChangeCharacter?.Invoke(SelectedElement.Description);
        }
        public override void Next()
        {
            base.Next();
            ChangeCharacter?.Invoke(SelectedElement.Description);
        }

        public override void Previous()
        {
            base.Previous();
            ChangeCharacter?.Invoke(SelectedElement.Description);
        }

    }
}