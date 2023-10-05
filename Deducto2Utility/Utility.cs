using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;
using UnityEngine.UI;
using Il2CppEPOOutline;
using Il2Cpp;
using System;

namespace Deducto2Utility
{
    internal class Utility
    {

        static DeductionGameData[] GameData = null;
        static Camera[] GameCameras = null;
        static GameObject PlayerMovement = null;   /* Local Player */
        static Transform TargetCamera = null;      /* Spectator ( Your target )*/
        static GameObject PlayerCharacter = null;  /* Local Player Spectating */
        static RoleData[] GameRoleData = null;
        static ItemData[] GameItemData = null;

        private enum EasyLogColors
        {
            Reset = 0,
            Black = 30,
            Red = 31,
            Green = 32,
            Yellow = 33,
            Blue = 34,
            Magenta = 35,
            Cyan = 36,
        }
        static void EasyLog(string message, EasyLogColors color)
        {
            int colorCode = (int)color;
            string coloredMessage = $"\u001b[{colorCode}m{message}\u001b[0m"; // Apply ANSI color code
            Melon<Core>.Logger.Msg(coloredMessage);
        }

        internal static GameObject GetLocalPlayerCamera()
        {
            GameCameras = GameObject.FindObjectsOfType<Camera>();
            //Camera ReturnCamera = null;
            foreach (var Camera in GameCameras)
            {
                EasyLog($"{Camera.name} was found", EasyLogColors.Green);
                if (Camera.enabled)
                {
                    PlayerMovement = Camera.transform.parent.parent.parent.gameObject;
                    //ReturnCamera = Camera;
                }
            }
            return PlayerMovement;
        }

    }
}
