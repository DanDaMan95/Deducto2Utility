using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;
using UnityEngine.UI;
using Il2CppEPOOutline;
using Il2Cpp;

[assembly: MelonInfo(typeof(Deducto2Utility.Class1), "Deducto2Utility", "1.9.0", "Mr. Dirty")]
namespace Deducto2Utility
{
    public class Class1 : MelonMod
    {
        private bool Flying = false;
        CosmeticData[] Cosmetics = null;
        Camera[] GameCameras = null;
        GameObject PlayerMovement = null;
        Transform TargetCamera = null; // SPECTATING PLAYER CAMERA
        GameObject PlayerCharacter = null; // SPECTATING PLAYER CHARACTERRIGGEDV7.0
        bool DeclaresInit = false;
        private void ProcessRooms()
        {
            MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique[] rooms = Object.FindObjectsOfType<MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique>();
            foreach (var room in rooms)
            {
                MelonLogger.Msg($"| Host: {room.field_Private_String_0} | Passcode: {room.field_Private_String_1} |");
            }
        }

        private void RemoveObjectsByName(string[] objectNames)
        {
            GameObject[] objects = Object.FindObjectsOfType<GameObject>();
            foreach (var gameObject in objects)
            {
                string name = gameObject.transform.name;
                if (ArrayContains(objectNames, name))
                {
                    Object.Destroy(gameObject);
                }
            }
        }

        private void AddOutlinesToObjects(string[] objectNames)
        {
            GameObject[] objects = Object.FindObjectsOfType<GameObject>();
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
                }
            }
        }

        private void EasyLog(string message, string colorCode)
        {
            string coloredMessage = $"\u001b[{colorCode}m{message}\u001b[0m"; // Have fun Mr.Dirty - OwO
            Melon<Class1>.Logger.Msg(coloredMessage);
        }

        private void UnlockCosmetics()
        {
            foreach(var Cos in Cosmetics)
            {
                Cos.PointCost = 0; // -999999999; You aren't special buddy >:(
            }
        }

        private GameObject GetLocalPlayerCamera()
        {
            GameCameras = GameObject.FindObjectsOfType<Camera>();
            //Camera ReturnCamera = null;
            foreach (var Camera in GameCameras)
            {
                EasyLog($"{Camera.name} was found", "32");
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
            if (Flying) { EasyLog("Flying was enabled.", "32"); } else { EasyLog("Flying was disabled.", "31"); }
        }

        [System.Obsolete]
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
                                ", "31");
            EasyLog(@"

                                    [BackQuote] -> Reset Parkour Level Changing Buttons
                                    [Alpha4] -> Enable Wallhacks ( OwO rawr x3 )
                                    [Keypad1] -> Unlock All Cosmetics
                                    [Mouse0 + LeftControl] -> Spectate Player ( Click on player )
                                    [F1] -> Reset Spectate Player
                                    [CapsLock] -> Fly ( NOT FINISHED. MAY NEVER BE )
                                    [KeypadPlus] -> Get Room Codes ( CRASHES GAME !! DONT USE. FIXING LATER !! )

", "32");

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

        public override void OnUpdate()
        {
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
                    EasyLog($"Going up! {PlayerMovement.name}","32");
                }
            } */ 
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Cosmetics = GameObject.FindObjectsOfType<CosmeticData>();
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

            string[] buttonNames = { "GivePositiveKarma" };
            string[] sliderNames = { "MaxFPSSlider" };

            MakeButtonsInteractable(buttonNames);
            SetSliderMinValue(sliderNames);

        }
    }
}
