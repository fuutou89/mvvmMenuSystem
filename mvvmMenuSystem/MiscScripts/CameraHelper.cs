using UnityEngine;
using System.Collections;

namespace mvvmMenuSystem
{
    public class CameraHelper : MonoBehaviour
    {
        public static Camera UICamera;
      
        void Awake()
        {
            UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }
    }
}