using Il2Cpp;
using Il2CppEPOOutline;
using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Deducto2Utility;
using static Deducto2Utility.Stuff;

[assembly: MelonInfo(typeof(Deducto2Utility.Core), "Deducto2Utility", "1.9.0", "Mr. Dirty")]
namespace Deducto2Utility
{
    public class Core : MelonMod
    {

        DeductionGameData[] GameData = null;
        GameObject PlayerMovement = null;   /* Local Player */
        Transform TargetCamera = null;      /* Spectator ( Your target )*/
        GameObject PlayerCharacter = null;  /* Local Player Spectating */
        RoleData[] GameRoleData = null;
        ItemData[] GameItemData = null;
        AudioClip[] GameAudioClips = null;
        bool EarrapeItem = false;

        /* DECLARES */

        private bool Flying = false;

        public class WeaponData
        {
            public int Range { get; set; }
            public int RateOfUse { get; set; }

            public WeaponData(int range, int rateOfUse)
            {
                Range = range;
                RateOfUse = rateOfUse;
            }
        }

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

        private Dictionary<string, WeaponData> Weapons = new Dictionary<string, WeaponData>
        {
            { "Knife01Data", new WeaponData(int.MaxValue, 0) },
            { "NailGunData", new WeaponData(int.MaxValue, 0) },
            { "Pistol01Data", new WeaponData(int.MaxValue, 0) },
            { "StaplerData", new WeaponData(int.MaxValue, 0) },
        };

        /* -------- */

        /* ONE TIME CHECKS */

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

        private void UnlockCosmetics()
        {
            GameData[0].UnlockAllCosmetics = true;
            EasyLog("UnlockAllCosmetics: true", EasyLogColors.Green);
        }

        private void ProcessFlying(bool Enabled)
        {
            PlayerMovement = Utility.GetLocalPlayer();
            Flying = !Flying;
            if (Flying) { EasyLog("Flying was enabled.", EasyLogColors.Green); } else { EasyLog("Flying was disabled.", EasyLogColors.Red); }
        }

        [Obsolete]
        public override void OnApplicationStart()
        {
            PlayerMovement = Utility.GetLocalPlayer();
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
                    [Keypad0] -> No KillCooldowns ( Why is this client sided Patrick..... )

", EasyLogColors.Green);

            MelonCoroutines.Start(WaitForGameData());
        }

        private IEnumerator WaitForGameData()
        {
            while (GameData == null || GameData.Length == 0)
            {
                yield return new WaitForSeconds(0.5f);
                GameData = Resources.FindObjectsOfTypeAll<DeductionGameData>();
            }
            UnlockCosmetics();
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
                PlayerMovement = Utility.GetLocalPlayer();
                PlayerMovement.transform.Find("CameraRoot/CameraControls/Camera").gameObject.SetActive(true);
            }
        }

        private void SpectateCharacter()
        {
            PlayerMovement = Utility.GetLocalPlayer();
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
                    EasyErr("CameraControls not found on: " + PlayerTarget.name);
                }
            }
        }

        private void OPWeapons()
        {
            GameItemData = Resources.FindObjectsOfTypeAll<ItemData>();
            foreach (var Item in GameItemData)
            {
                if (Item.name == "SketchMarkerData" && EarrapeItem)
                {
                    // EasyLog(Item.name + " was found", EasyLogColors.Cyan);
                    GameAudioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
                    foreach(var Audio in GameAudioClips)
                    {
                        // EasyLog(Audio.name + " was found",EasyLogColors.Cyan);
                        if (Audio.name.Contains("EXPLOSION"))
                        {
                            Item.RateOfUse = 0;
                            Item.SoundEffectVolume = 999;
                            Item.UseSoundEffects.Clear();
                            Item.UseSoundEffects.Add(Audio);
                            Item.UseSoundEffects.Add(Audio);
                            Item.UseSoundEffects.Add(Audio);
                            Item.UseSoundEffects.Add(Audio);
                            Item.UseSoundEffects.Add(Audio);
                            Item.UseSoundEffects.Add(Audio);
                            Item.UseSoundEffects.Add(Audio);
                        }
                    }
                }
                if (Weapons.ContainsKey(Item.name))
                {
                    int RangeOfWeapon = Weapons[Item.name].Range;
                    int RateOfUse = Weapons[Item.name].RateOfUse;

                    Item.RateOfUse = RateOfUse;
                    Item.Range = RangeOfWeapon;
                }
                else
                {
                    // How did this even happen
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

        private void NoKillCooldowns()
        {
            foreach(var MatchSettings in Resources.FindObjectsOfTypeAll<MatchSettings>())
            {
                MatchSettings.killCooldown = 0;
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
                OPWeapons();
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                NoKillCooldowns();
            }

            if (!SetMinFPSSlider)
            {
                string[] sliderNames = { "MaxFPSSlider" };
                SetSliderMinValue(sliderNames);
                FunnyQuotes();
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                EarrapeItem = !EarrapeItem;
                OPWeapons();
            }

        }
    }
}
