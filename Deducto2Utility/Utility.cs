using UnityEngine;
using MelonLoader;
using Il2Cpp;

namespace Deducto2Utility
{
    internal class Utility
    {

        static DeductionGameData[] GameData = null;
        static Camera[] GameCameras = null;
        static GameObject PlayerMovement = null;   /* Local Player */

        internal static GameObject GetLocalPlayer()
        {
            GameCameras = GameObject.FindObjectsOfType<Camera>();
            //Camera ReturnCamera = null;
            foreach (var Camera in GameCameras)
            {
                // EasyLog($"{Camera.name} was found", EasyLogColors.Green);
                if (Camera.enabled)
                {
                    PlayerMovement = Camera.transform.parent.parent.parent.gameObject;
                }
            }
            return PlayerMovement;
        }

    }
}
