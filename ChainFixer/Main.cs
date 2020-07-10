using MelonLoader;
using System;
using System.IO;
using UnityEngine;
using Harmony;
namespace ChainFixer
{
    public static class BuildInfo
    {
        public const string Name = "ChainFixer"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "Alternity"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.1.2"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class ChainFixer : MelonMod
    {
        public static Config config = new Config();

        //The current way of tracking menu state.
        //TODO: Hook to the SetMenuState function without breaking the game
        public static MenuState.State menuState;
        public static MenuState.State oldMenuState;

        public static bool chainStartEither = false;
        public static bool chainStartRight = false;
        public static bool chainStartLeft = false;
        public static bool chainStartNone = false;

        public static void LoadConfig()
        {
            string path = Application.dataPath + "/../Mods/Config/ChainFixer.json";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Application.dataPath + "/../Mods/Config");
                string contents = Encoder.GetConfig(config);
                File.WriteAllText(path, contents);
            }
            Encoder.SetConfig(config, File.ReadAllText(path));
        }

        public override void OnApplicationStart()
        {
            LoadConfig();
            var i = HarmonyInstance.Create("SkyRotation");
            Hooks.ApplyHooks(i);
        }

        public override void OnUpdate()
        {
            //Tracking menu state
            menuState = MenuState.GetState();

            //If menu changes
            if (menuState != oldMenuState)
            {
                //Put stuff to do when a menu change triggers here
                if (menuState == MenuState.State.Launching)
                {
                    chainStartEither = false;
                    chainStartRight = false;
                    chainStartLeft = false;
                    chainStartNone = false;
                }

                //Updating state
                oldMenuState = menuState;
            }
        }
    }
}
