using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMode
{
    public abstract class ObjectFactroy : ScriptableObject
    {
        [SerializeField] private Scene scene;

        protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
        {
            if (!scene.isLoaded)
            {
                if (Application.isEditor)
                {
                    scene = SceneManager.GetSceneByName(name);
                    if (!scene.isLoaded)
                        scene = SceneManager.CreateScene(name);
                }
                else
                {
                    scene = SceneManager.CreateScene(name);
                }
            }
            T instance = Instantiate(prefab);
            SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
            return instance;
        }

        public async Task Unload()
        {
            if (!scene.isLoaded)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(scene);
                while (unloadOp.isDone == false)
                {
                    await Task.Delay(1);
                }
            }
        }

    }
}