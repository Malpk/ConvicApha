using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UserIntaface.MainMenu
{
    public class DescriptionCharacterHandler : MonoBehaviour
    {
        [SerializeField] private CharacterScroller _characterScroller;
        [SerializeField] private TMP_Text _nameCharacter;
        [SerializeField] private TMP_Text _descriptionCharacter;

        private void Awake()
        {
            _characterScroller.ChangeCharacter += OnChangeCharacter;
        }      
        private void OnChangeCharacter(DescriptionCharacter descriptionCharacterData)
        {

            if (descriptionCharacterData != null)
            {
                _nameCharacter.text = descriptionCharacterData.name;
                _descriptionCharacter.text = descriptionCharacterData.Description;
            }
        }

        private void OnDestroy()
        {
            _characterScroller.ChangeCharacter -= OnChangeCharacter;
        }
    }
}
