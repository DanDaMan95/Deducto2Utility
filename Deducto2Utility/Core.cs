using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using static Deducto2Utility.Stuff;
using static Deducto2Utility.Dicts;
using Il2Cpp;
using System;
using Il2CppEPOOutline;

[assembly: MelonInfo(typeof(Deducto2Utility.Core), "Deducto2Utility", "1.9.0", "Mr. Dirty, SomeDudeWhoOwO's")]
namespace Deducto2Utility
{
    public class Core : MelonMod
    {
        private bool flying = false;
        private bool earrapeItemEnabled = false;
        private bool unlockCosmetics = true;
        private bool wallhacksEnabled = false;
        private bool noKillCooldownsEnabled = false;
        private bool skipLoadingCooldownEnabled = false;

        private GameObject playerMovement = null;
        private Transform targetCamera = null;
        private GameObject playerCharacter = null;

        private DeductionGameData[] gameData = null;
        private ItemData[] gameItemData = null;
        private RoleData[] gameRoleData = null;
        private AudioClip[] gameAudioClips = null;

        private bool minFPSSliderSet = false;

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

        private void ToggleOutlinesForObjects(string[] objectNames, bool enable)
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
                        Outlinable a = gameObject.AddComponent<Outlinable>();
                        a.AddAllChildRenderersToRenderingList(RenderersAddingMode.SkinnedMeshRenderer);
                        a.enabled = true;
                        a.OutlineParameters.Color = Color.white;
                    }
                }
            }
        }

        private bool ArrayContains(string[] array, string value)
        {
            return System.Array.Exists(array, element => element == value);
        }

        private void ToggleSliderCustomValues(string[] sliderNames)
        {
            Slider[] sliders = GameObject.FindObjectsOfType<Slider>();
            foreach (var slider in sliders)
            {
                string name = slider.transform.parent.gameObject.name;
                if (ArrayContains(sliderNames, name) && Sliders.TryGetValue(name, out SliderInfo sliderInfo))
                {
                    slider.maxValue = sliderInfo.IsMax ? sliderInfo.Value : slider.minValue;
                    minFPSSliderSet = true;
                }
            }
        }

        private void ToggleUnlockCosmetics()
        {
            if (gameData != null && gameData.Length > 0)
            {
                gameData[0].UnlockAllCosmetics = unlockCosmetics;
                EasyLog($"UnlockAllCosmetics: {unlockCosmetics}", EasyLogColors.Green);
            }
        }

        private void ToggleFlying(bool enable)
        {
            playerMovement = Utility.GetLocalPlayer();
            flying = enable;
            //flying ? EasyLog("Flying was enabled.", EasyLogColors.Green) : EasyLog("Flying was disabled.", EasyLogColors.Red);
            EasyLog(flying ? "Flying was enabled." : "Flying was disabled.", flying ? EasyLogColors.Green : EasyLogColors.Red);
        }

        [Obsolete]
        public override void OnApplicationStart()
        {
            playerMovement = Utility.GetLocalPlayer();
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
                [Keypad7] -> OP Weapons
                [Keypad2] -> Earrape Marker
                [Keypad3] -> SkipLoadingCooldown ( Client sided again... ? )
                [Keypad0] -> No KillCooldowns ( Why is this client sided Patrick..... )
                ", EasyLogColors.Green);

            MelonCoroutines.Start(WaitForGameData());
        }

        private IEnumerator WaitForGameData()
        {
            while (gameData == null || gameData.Length == 0)
            {
                yield return new WaitForSeconds(0.5f);
                gameData = Resources.FindObjectsOfTypeAll<DeductionGameData>();
            }
            ToggleUnlockCosmetics();
        }

        private void DisableSpectate()
        {
            if (targetCamera && playerCharacter != null)
            {
                targetCamera.gameObject.SetActive(false);
                playerCharacter.SetActive(true);
            }
            else
            {
                playerMovement = Utility.GetLocalPlayer();
                playerMovement.transform.Find("CameraRoot/CameraControls/Camera").gameObject.SetActive(true);
            }
        }

        private void SpectateCharacter()
        {
            playerMovement = Utility.GetLocalPlayer();
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            RaycastHit hit;
            float distance = 100f;

            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.DrawLine(ray.origin, hit.point);
                GameObject playerTarget = hit.collider.transform.root.gameObject;

                targetCamera = playerTarget.transform.Find("CameraRoot/CameraControls/Camera");
                playerCharacter = playerTarget.transform.Find("PlayerModelV2/CharacterRiggedV7.0").gameObject;
                if (playerCharacter == null) { playerCharacter = playerTarget.transform.Find("PlayerModel(Clone)/CharacterRiggedV7.0").gameObject; }
                if (targetCamera != null)
                {
                    targetCamera.gameObject.SetActive(true);
                    playerCharacter.SetActive(false);
                }
                else
                {
                    EasyErr("CameraControls not found on: " + playerTarget.name);
                }
            }
        }

        private void ToggleOPWeapons()
        {
            gameItemData = Resources.FindObjectsOfTypeAll<ItemData>();
            foreach (var item in gameItemData)
            {
                if (item.name == "SketchMarkerData" && earrapeItemEnabled)
                {
                    gameAudioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
                    foreach (var audio in gameAudioClips)
                    {
                        if (audio.name.Contains("EXPLOSION"))
                        {
                            item.RateOfUse = 0;
                            item.SoundEffectVolume = 999;
                            item.UseSoundEffects.Clear();
                            item.UseSoundEffects.Add(audio);
                            item.UseSoundEffects.Add(audio);
                            item.UseSoundEffects.Add(audio);
                            item.UseSoundEffects.Add(audio);
                            item.UseSoundEffects.Add(audio);
                            item.UseSoundEffects.Add(audio);
                            item.UseSoundEffects.Add(audio);
                        }
                    }
                }
                if (Weapons.ContainsKey(item.name))
                {
                    int rangeOfWeapon = Weapons[item.name].Range;
                    int rateOfUse = Weapons[item.name].RateOfUse;

                    item.RateOfUse = rateOfUse;
                    item.Range = rangeOfWeapon;
                }
                else
                {
                    // How did this even happen
                }
            }
        }

        private void ToggleFunnyQuotes()
        {
            gameRoleData = Resources.FindObjectsOfTypeAll<RoleData>();
            foreach (var role in gameRoleData)
            {
                if (RoleStrings.ContainsKey(role.name))
                {
                    string getDescription = RoleStrings[role.name];
                    role.Description = getDescription;
                }
                else
                {
                    // How did this even happen
                }
            }
        }

        private void ToggleNoKillCooldowns()
        {
            foreach (var matchSettings in Resources.FindObjectsOfTypeAll<MatchSettings>())
            {
                matchSettings.killCooldown = noKillCooldownsEnabled ? 0 : 10; // Adjust the cooldown value as needed
            }
        }

        private void ToggleSkipLoadingCooldown()
        {
            if (gameData != null && gameData.Length > 0)
            {
                gameData[0].SkipMatchCountdown = skipLoadingCooldownEnabled;
                EasyLog($"SkipMatchCountdown: {skipLoadingCooldownEnabled}", EasyLogColors.Green);
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
                wallhacksEnabled = !wallhacksEnabled;
                ToggleOutlinesForObjects(objectNames, wallhacksEnabled);
            }

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                ToggleFlying(!flying);
            }

            if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftControl))
            {
                SpectateCharacter();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                DisableSpectate();
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                ToggleOPWeapons();
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                noKillCooldownsEnabled = !noKillCooldownsEnabled;
                ToggleNoKillCooldowns();
            }

            if (!minFPSSliderSet)
            {
                string[] sliderNames = { "MaxFPSSlider", "Slider_01", "Slider", "LobbySizeSlider" };
                ToggleSliderCustomValues(sliderNames);
                ToggleFunnyQuotes();
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                earrapeItemEnabled = !earrapeItemEnabled;
                ToggleOPWeapons();
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                skipLoadingCooldownEnabled = !skipLoadingCooldownEnabled;
                ToggleSkipLoadingCooldown();
            }
        }
    }
}
