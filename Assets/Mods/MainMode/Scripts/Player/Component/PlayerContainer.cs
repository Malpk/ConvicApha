using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerContainer<T>  : IPlayerTask 
    {
        protected List<ContainCell<T>> contents = new List<ContainCell<T>>();

        private List<ContainCell<T>> _delteList = new List<ContainCell<T>>();

        protected System.Action changeContainerAction;

        public delegate void DeleteContent(T content);
        public event DeleteContent DeleteContentAction;

        public void Reset()
        {
            contents.Clear();
        }

        public void Add(EffectType effect ,T content, float timeActive)
        {
            foreach (var movementEffect in contents)
            {
                if (movementEffect.effect == effect)
                {
                    movementEffect.Start(timeActive);
                    return;
                }
            }
            var effectActive = new ContainCell<T>(effect,content);
            effectActive.Start(timeActive);
            effectActive.OnDelete += Delete;
            contents.Add(effectActive);
        }

        public void Update()
        {
            foreach (var update in contents)
            {
                update.Update();
            }
            while (_delteList.Count > 0)
            {
                contents.Remove(_delteList[0]);
                _delteList.Remove(_delteList[0]);
            }
        }

        private void Delete(ContainCell<T> delete)
        {
            delete.OnDelete -= Delete;
            _delteList.Add(delete);
            Change();
        }

        private void Change()
        {
            if (changeContainerAction != null)
                changeContainerAction();
        }
    }
}
