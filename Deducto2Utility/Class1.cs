using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;
using UnityEngine.UI;
using Il2CppEPOOutline;
using Il2Cpp;
using System;

[assembly: MelonInfo(typeof(Deducto2Utility.Class1), "Deducto2Utility", "1.9.0", "Mr. Dirty")]
namespace Deducto2Utility
{
    public class Class1 : MelonMod
    {

        DeductionGameData[] GameData = null;
        Camera[] GameCameras = null;
        GameObject PlayerMovement = null;   /* Local Player */
        Transform TargetCamera = null;      /* Spectator ( Your target )*/
        GameObject PlayerCharacter = null;  /* Local Player Spectating */
        RoleData[] GameRoleData = null;

        /* ENUMS */

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
            White = 37
        }

        /* ----- */

        /* DECLARES */

        private bool Flying = false;

        private Dictionary<string, string> RoleStrings = new Dictionary<string, string>
        {
            { "ImposterData", "Kill all Co-workers, also press '4' on your keyboard. Thank me later." },
            { "CoworkerData", "I am pretty sure anyone else using this cheat is walling, Become friends with them." },
            { "InfectedData", "Haha, L + Bozo + Ratio, Anyway, go touch others or something." },
            { "JudgeData", "JUDGE JUDY ??!!! Don't be rude okay...?" },
            { "SecurityData", "Kill literally anyone but you'll be voted out. Kill Imposters" },
            { "SupervisorData", "You wote fow the pwecious pweople. It's aww about Women's Suffewage !! OwO UwU ^w^" },
            { "Patrick", "What's up!!"}
        };

        /* -------- */

        /* ONE TIME CHECKS */

        bool UnlockedAllCosmetics = false;
        bool SetMinFPSSlider = false;

        /* --------------- */

        private void ProcessRooms()
        {
            MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique[] rooms = GameObject.FindObjectsOfType<MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique>();
            foreach (var room in rooms)
            {
                MelonLogger.Msg($"| Host: {room.field_Private_String_0} | Passcode: {room.field_Private_String_1} |");
            }
        }

        private void RemoveObjectsByName(string[] objectNames)
        {
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var gameObject in objects)
            {
                string name = gameObject.transform.name;
                if (ArrayContains(objectNames, name))
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }

        private void AddOutlinesToObjects(string[] objectNames)
        {
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var gameObject in objects)
            {
                string name = gameObject.transform.name;
                if (ArrayContains(objectNames, name))
                {
                    Outlinable existingOutlinable = gameObject.GetComponent<Outlinable>();
                    if (existingOutlinable == null)
                    {
                        gameObject.AddComponent<Outlinable>();
                    }
                }
            }
        }

        private bool ArrayContains(string[] array, string value)
        {
            foreach (var item in array)
            {
                if (item == value)
                {
                    return true;
                }
            }
            return false;
        }

        private void EnableOutlinesForObjects(string[] objectNames)
        {
            Outlinable[] outlines = GameObject.FindObjectsOfType<Outlinable>();
            foreach (var outline in outlines)
            {
                string name = outline.transform.name;
                if (ArrayContains(objectNames, name))
                {
                    outline.AddAllChildRenderersToRenderingList(RenderersAddingMode.SkinnedMeshRenderer);
                    outline.enabled = true;
                    outline.OutlineParameters.Color = Color.white;
                }
            }
        }

        private void MakeButtonsInteractable(string[] buttonNames)
        {
            Button[] buttons = GameObject.FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                string name = button.transform.name;
                if (ArrayContains(buttonNames, name))
                {
                    button.interactable = true;
                }
            }
        }

        private void SetSliderMinValue(string[] sliderNames)
        {
            Slider[] sliders = GameObject.FindObjectsOfType<Slider>();
            foreach (var slider in sliders)
            {
                string name = slider.transform.parent.transform.gameObject.name;
                if (ArrayContains(sliderNames, name))
                {
                    slider.minValue = -1;
                    SetMinFPSSlider = true;
                }
            }
        }
        private void EasyLog(string message, EasyLogColors color)
        {
            int colorCode = (int)color;
            string coloredMessage = $"\u001b[{colorCode}m{message}\u001b[0m"; // Apply ANSI color code
            Melon<Class1>.Logger.Msg(coloredMessage);
        }

        private void UnlockCosmetics()
        {
            GameData[0].UnlockAllCosmetics = true;
            UnlockedAllCosmetics = true;

            /*foreach(var Cos in Cosmetics)
            {
                Cos.PointCost = 0; // -999999999; You aren't special buddy >:(
                // Cos.SpecialityUnlock = true; // meh
                Cos.UnlockAllCosmetics = true;
            }*/

        }

        private GameObject GetLocalPlayerCamera()
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

        private void ProcessFlying(bool Enabled)
        {
            PlayerMovement = GetLocalPlayerCamera();
            Flying = !Flying;
            if (Flying) { EasyLog("Flying was enabled.", EasyLogColors.Green); } else { EasyLog("Flying was disabled.", EasyLogColors.Red); }
        }

        [Obsolete]
        public override void OnApplicationStart()
        {

            /* DEDUCTO DECLARES */

            PlayerMovement = GetLocalPlayerCamera();
            EasyLog(@"
                ________             .___             __         ________  ____ ___   __  .__.__  .__  __          
                \______ \   ____   __| _/_ __   _____/  |_  ____ \_____  \|    |   \_/  |_|__|  | |__|/  |_ ___.__.
                 |    |  \_/ __ \ / __ |  |  \_/ ___\   __\/  _ \ /  ____/|    |   /\   __\  |  | |  \   __<   |  |
                 |    `   \  ___// /_/ |  |  /\  \___|  | (  <_> )       \|    |  /  |  | |  |  |_|  ||  |  \___  |
                /_______  /\___  >____ |____/  \___  >__|  \____/\_______ \______/   |__| |__|____/__||__|  / ____|
                        \/     \/     \/           \/                    \/                                 \/     
                                ", EasyLogColors.Red);
            EasyLog(@"

                                    [BackQuote] -> Reset Parkour Level Changing Buttons
                                    [Alpha4] -> Enable Wallhacks ( OwO rawr x3 )
                                    [Keypad1] -> Unlock All Cosmetics
                                    [Mouse0 + LeftControl] -> Spectate Player ( Click on player )
                                    [F1] -> Reset Spectate Player
                                    [CapsLock] -> Fly ( NOT FINISHED. MAY NEVER BE )
                                    [KeypadPlus] -> Get Room Codes ( CRASHES GAME !! DONT USE. FIXING LATER !! )

", EasyLogColors.Green);

        }
        
        private void DisableSpectate()
        {
            if (TargetCamera && PlayerCharacter != null)
            {
                TargetCamera.gameObject.SetActive(false);
                PlayerCharacter.SetActive(true);
            }
            else
            {
                PlayerMovement = GetLocalPlayerCamera();
                PlayerMovement.transform.Find("CameraRoot/CameraControls/Camera").gameObject.SetActive(true);
            }
        }

        private void SpectateCharacter()
        {
            PlayerMovement = GetLocalPlayerCamera();
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            RaycastHit hit;
            float distance = 100f;

            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.DrawLine(ray.origin, hit.point);
                GameObject PlayerTarget = hit.collider.transform.root.gameObject;

                TargetCamera = PlayerTarget.transform.Find("CameraRoot/CameraControls/Camera");
                PlayerCharacter = PlayerTarget.transform.Find("PlayerModelV2/CharacterRiggedV7.0").gameObject;
                if (PlayerCharacter == null) { PlayerCharacter = PlayerTarget.transform.Find("PlayerModel(Clone)/CharacterRiggedV7.0").gameObject; }
                if (TargetCamera != null)
                {
                    TargetCamera.gameObject.SetActive(true);
                    PlayerCharacter.SetActive(false);
                }
                else
                {
                    Debug.LogError("CameraControls not found on: " + PlayerTarget.name);
                }
            }
        }

        private void FunnyQuotes()
        {
            GameRoleData = Resources.FindObjectsOfTypeAll<RoleData>();
            foreach (var Role in GameRoleData)
            {
                if (RoleStrings.ContainsKey(Role.name))
                {
                    string GetDescription = RoleStrings[Role.name];
                    Role.Description = GetDescription;
                }
                else
                {
                    // How did this even happen
                }
            }
        }


        public override void OnUpdate()
        {

            //string[] buttonNames = { "GivePositiveKarma" };
            //MakeButtonsInteractable(buttonNames);

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                ProcessRooms();
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                RemoveObjectsByName(new string[] { "ParkourSelectionBlocker", "ClearInventory" });
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                string[] objectNames = { "CharacterRiggedV7.0", "RagdollPlayer(Clone)" };
                AddOutlinesToObjects(objectNames);
                EnableOutlinesForObjects(objectNames);
            }

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                ProcessFlying(!Flying);
            }
            /* if (Flying)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {

                    PlayerMovement.transform.localPosition += new Vector3(0,5,0);
                    EasyLog($"Going up! {PlayerMovement.name}",EasyLogColors.Green);
                }
            } */ 
            if (Input.GetKeyDown(KeyCode.Keypad1) && !UnlockedAllCosmetics)
            {
                EasyLog("Unlocked All Cosmetics",EasyLogColors.Green);
                GameData = Resources.FindObjectsOfTypeAll<DeductionGameData>(); //GameObject.FindObjectsOfType<CosmeticData>();
                UnlockCosmetics();
            }
            if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftControl))
            {
                SpectateCharacter();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                DisableSpectate();
            }

            if (!SetMinFPSSlider)
            {
                string[] sliderNames = { "MaxFPSSlider" };
                SetSliderMinValue(sliderNames);
            }

            FunnyQuotes();

        }
    }
}
