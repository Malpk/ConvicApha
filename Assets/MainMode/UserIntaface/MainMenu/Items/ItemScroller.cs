using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserIntaface.MainMenu
{
    public class ItemScroller : BaseScroller<ItemView>
    {            
        protected override void Awake()
        {
           base.Awake();
            _ringList = new RingListItems(_listPrefabs, _transforms,_parentElements);
        }    

    }
}