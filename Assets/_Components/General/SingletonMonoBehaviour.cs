using System;
using UnityEngine;

namespace FurtleGame.Singleton
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static readonly UnityEngine.Object syncRoot = new UnityEngine.Object();
        public static T Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (!instance)
                    {
                        instance = FindObjectOfType(typeof(T)) as T;

                        //if (!instance)
                        //{
                        //    GameObject go = new GameObject(typeof(T).ToString());
                        //    instance = go.AddComponent<T>();
                        //}
                    }

                    return instance;
                }
            }
        }

        [SerializeField]
        private bool dontDestroyOnLoad;

        private static T instance;

        protected void Awake()
        {
            if (Instance != this)
            {
                Destroy(this);
            }
            else if (Instance != null)
            {
                if (dontDestroyOnLoad)
                {
                    transform.SetParent(null);
                    DontDestroyOnLoad(gameObject);
                }

                ChildAwake();
            }
        }

        protected virtual void ChildAwake() { }
    }
}