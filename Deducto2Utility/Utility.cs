using UnityEngine;
using MelonLoader;
using Il2Cpp;

namespace Deducto2Utility
{
    internal class Utility
    {

        static Camera[] GameCameras = null;
        static GameObject PlayerMovement = null;   /* Local Player */

        internal static GameObject GetLocalPlayer()
        {
            GameCameras = GameObject.FindObjectsOfType<Camera>();
            foreach (var Camera in GameCameras)
            {
                if (Camera.enabled)
                {
                    PlayerMovement = Camera.transform.parent.parent.parent.gameObject;
                }
            }
            return PlayerMovement;
        }

    }
}
