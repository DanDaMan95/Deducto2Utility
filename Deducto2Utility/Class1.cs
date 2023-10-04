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
        static Slider[] Sliders = null;
        static GameObject[] Objects = null;
        static Button[] Kudos = null;
        static Outlinable[] Outlines = null;
        static MonoBehaviourPublicTrUnique[] Names = null;
        static MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique[] GetRooms = null;
        
        public override void OnUpdate()
        {
            bool keyDown1 = Input.GetKeyDown(KeyCode.KeypadPlus);
            {
                if (keyDown1)
                {
                    GetRooms = Object.FindObjectsOfType<MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique>();
                    foreach (MonoBehaviourPublicTeGaTeBuGaRoBuroGaTMUnique room in GetRooms)
                    {
                        MelonLogger.Msg(string.Concat(new string[]
                        {
                        "| Host: ",
                        room.field_Private_String_0,
                        " | Passcode: ",
                        room.field_Private_String_1,
                        " | "
                        }));
                    }
                }
            }
            bool keyDown2 = Input.GetKeyDown(KeyCode.BackQuote);
            {
                if (keyDown2)
                {
                    Objects = Object.FindObjectsOfType<GameObject>();
                    foreach (GameObject gameObject in Objects)
                    {
                        bool flag1 = gameObject.transform.name == "ParkourSelectionBlocker";
                        bool flag2 = gameObject.transform.name == "ClearInventory";
                        if (flag1 || flag2)
                        {
                            Object.Destroy(gameObject);
                        }
                    }
                }
            }
            bool keyDown = Input.GetKeyDown(KeyCode.Alpha4);
            {
                if (keyDown)
                {
                    Objects = Object.FindObjectsOfType<GameObject>();
                    foreach (GameObject gameObject in Objects)
                    {
                        bool flag1 = gameObject.transform.name == "CharacterRiggedV7.0";
                        bool flag2 = gameObject.transform.name == "RagdollPlayer(Clone)";

                        if (flag1 || flag2)
                        {
                            Outlinable existingOutlinable = gameObject.GetComponent<Outlinable>();
                            if (existingOutlinable == null)
                            {
                                gameObject.AddComponent<Outlinable>();
                            }
                        }
                    }
                    Outlines = GameObject.FindObjectsOfType<Outlinable>();
                    foreach (var lin in Outlines)
                    {
                        bool flag1 = lin.transform.name == "CharacterRiggedV7.0";
                        bool flag2 = lin.transform.name == "RagdollPlayer(Clone)";
                        if (flag1 || flag2)
                        {
                            lin.AddAllChildRenderersToRenderingList(RenderersAddingMode.All);
                        }
                    }
                }
            }
            Outlines = GameObject.FindObjectsOfType<Outlinable>();
            {
                if (Outlines != null)
                {
                    foreach (var lins in Outlines)
                    {
                        bool flag1 = lins.transform.name == "RagdollPlayer(Clone)";
                        bool flag2 = lins.transform.name == "CharacterRiggedV7.0";
                        if (flag1 || flag2)
                        {
                            lins.enabled = true;
                            lins.outlineParameters.Color = Color.white;
                        }
                    }
                }
            }
            Kudos = GameObject.FindObjectsOfType<Button>();
            {
                if (Kudos != null)
                {
                    foreach (var kudo in Kudos)
                    {
                        bool flag1 = kudo.transform.name == "GivePositiveKarma";
                        if (flag1)
                        {
                            kudo.interactable = true;
                        }
                    }
                }
            }
            Sliders = GameObject.FindObjectsOfType<Slider>();
            {
                if (Sliders != null)
                {
                    foreach (var slider in Sliders)
                    {
                        bool flag1 = slider.transform.name == "Slider";
                        if (flag1)
                        {
                            slider.minValue = 0;
                        }
                    }
                }
            }
        }
    }
}
