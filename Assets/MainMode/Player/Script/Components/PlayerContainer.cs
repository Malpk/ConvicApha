using System.Collections.Generic;
using UnityEngine;
using MainMode;

namespace PlayerComponent
{
    public class PlayerContainer<T>  : IPlayerTask where T : IEffect
    {
        protected List<ContainCell<T>> contents = new List<ContainCell<T>>();

        protected System.Action changeContainerAction;

        public delegate void DeleteContent(T content);
        public event DeleteContent DeleteContentAction;

        public void Reset()
        {
            contents.Clear();
        }

        public void Add(T content, float timeActive)
        {

            foreach (var movementEffect in contents)
            {
                if (movementEffect.content.Effect == content.Effect)
                {
                    movementEffect.Start(timeActive);
                    return;
                }
            }
            var effectActive = new ContainCell<T>(content);
            effectActive.Start(timeActive);
            contents.Add(effectActive);
        }

        public void Update()
        {
            var delete = new List<ContainCell<T>>();
            foreach (var update in contents)
            {
                if (!update.Update())
                    delete.Add(update);
            }
            Delete(delete);
            Change();
        }

        private void Delete(List<ContainCell<T>> delete)
        {
            if (delete.Count > 0)
            {
                foreach (var effect in delete)
                {
                    contents.Remove(effect);
                    if (DeleteContentAction != null)
                        DeleteContentAction(effect.content);
                }
            }
            Change();
        }

        private void Change()
        {
            if (changeContainerAction != null)
                changeContainerAction();
        }
    }
}
