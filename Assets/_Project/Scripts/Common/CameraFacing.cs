
using UnityEngine;

namespace Common
{
    class CameraFacing : MonoBehaviour
    {
        public void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
