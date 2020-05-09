using MelonLoader;
using Steamworks;
using System.Linq;
using NET_SDK.Harmony;
using NET_SDK;
using System;

namespace ChainFixer
{
    public static class BuildInfo
    {
        public const string Name = "ChainFixer"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "Alternity"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class ChainFixer : MelonMod
    {
        public static Patch TargetSpawner_SpawnCue;

        //The current way of tracking menu state.
        //TODO: Hook to the SetMenuState function without breaking the game
        public static MenuState.State menuState;
        public static MenuState.State oldMenuState;

        public static bool chainStart = false;

        public override void OnApplicationStart()
        {
            Instance instance = Manager.CreateInstance("ChainFixer");

            TargetSpawner_SpawnCue = instance.Patch(SDK.GetClass("TargetSpawner").GetMethod("SpawnCue"), typeof(ChainFixer).GetMethod("SpawnCue"));
        }

        public static unsafe void SpawnCue(IntPtr @this, IntPtr c)
        {
            SongCues.Cue cue = new SongCues.Cue(c);

            if (cue.behavior == Target.TargetBehavior.ChainStart)
            {
                TargetSpawner_SpawnCue.InvokeOriginal(@this, new IntPtr[]
                {
                    c
                });
                chainStart = true;
            }
            else if (cue.behavior == Target.TargetBehavior.Chain)
            {
                if (chainStart)
                {
                    TargetSpawner_SpawnCue.InvokeOriginal(@this, new IntPtr[]
                    {
                        c
                    });
                }
            }
            else
            {
                TargetSpawner_SpawnCue.InvokeOriginal(@this, new IntPtr[]
                {
                    c
                });
                chainStart = false;
            }
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
                    chainStart = false;
                }

                //Updating state
                oldMenuState = menuState;
            }
        }

        /*
        public override void OnLevelWasLoaded(int level)
        {
            MelonModLogger.Log("OnLevelWasLoaded: " + level.ToString());
        }

        public override void OnLevelWasInitialized(int level)
        {
            MelonModLogger.Log("OnLevelWasInitialized: " + level.ToString());
        }

        public override void OnFixedUpdate()
        {
            MelonModLogger.Log("OnFixedUpdate");
        }

        public override void OnLateUpdate()
        {
            MelonModLogger.Log("OnLateUpdate");
        }

        public override void OnGUI()
        {
            MelonModLogger.Log("OnGUI");
        }

        public override void OnApplicationQuit()
        {
            MelonModLogger.Log("OnApplicationQuit");
        }

        public override void OnModSettingsApplied()
        {
            MelonModLogger.Log("OnModSettingsApplied");
        }

        public override void VRChat_OnUiManagerInit() // Only works in VRChat
        {
            MelonModLogger.Log("VRChat_OnUiManagerInit");
        }
        */
    }
}
