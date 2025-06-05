using System;
using UnityEngine;

namespace GAFURIX
{
    public class DeveloperConsole : IPuckMod
    {
        GUIConsole guiConsole;
        GameObject guiConsoleParent;

        public bool OnEnable()
        {
            Debug.Log($"[{Constants.MOD_NAME}] Enabling...");

            try
            {
                guiConsoleParent = ModManagerV2.Instance.gameObject;
                guiConsole = guiConsoleParent.AddComponent<GUIConsole>();

                guiConsole.Open();

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Failed to enable: {e.Message}");
                return false;
            }
        }

        public bool OnDisable()
        {
            Debug.Log($"[{Constants.MOD_NAME}] Disabling...");

            try
            {
                if (guiConsole != null)
                    GameObject.Destroy(guiConsole);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Failed to disable: {e.Message}");
                return false;
            }
        }
    }
}
