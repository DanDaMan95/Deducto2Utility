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
                Melon<Class1>.Logger.Msg("Alpha4 was pressed");
                string[] objectNames = { "CharacterRiggedV7.0", "RagdollPlayer(Clone)" };
                AddOutlinesToObjects(objectNames);
                EnableOutlinesForObjects(objectNames);
            }

            string[] buttonNames = { "GivePositiveKarma" };
            MakeButtonsInteractable(buttonNames);

            string[] sliderNames = { "MaxFPSSlider" };
            SetSliderMinValue(sliderNames);
        }
    }
}
