using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerContainer<T>  : IPlayerTask 
    {
        protected List<ContainCell<T>> contents = new List<ContainCell<T>>();

        private List<ContainCell<T>> _delteList = new List<ContainCell<T>>();

        protected System.Action changeContainerAction;

        public event System.Action<T> DeleteContentAction;

        public void Reset()
        {
            while (contents.Count > 0)
            {
                DeleteContentAction?.Invoke(contents[0].content);
                contents.Remove(contents[0]);
            }
            Change();
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
            if (_delteList.Count > 0)
            {
                while (_delteList.Count > 0)
                {
                    contents.Remove(_delteList[0]);
                    DeleteContentAction?.Invoke(_delteList[0].content);
                    _delteList.Remove(_delteList[0]);
                }
                Change();
            }
        }

        private void Delete(ContainCell<T> delete)
        {
            delete.OnDelete -= Delete;
            _delteList.Add(delete);
        }

        private void Change()
        {
            if (changeContainerAction != null)
                changeContainerAction();
        }
    }
}
