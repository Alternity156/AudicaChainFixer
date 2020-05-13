using MelonLoader;
using NET_SDK.Harmony;
using NET_SDK;
using System;
using System.IO;
using UnityEngine;

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
        public static Patch Target_OnCreated;

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

            Instance instance = Manager.CreateInstance("ChainFixer");

            TargetSpawner_SpawnCue = instance.Patch(SDK.GetClass("TargetSpawner").GetMethod("SpawnCue"), typeof(ChainFixer).GetMethod("SpawnCue"));
            Target_OnCreated = instance.Patch(SDK.GetClass("Target").GetMethod("OnCreated"), typeof(ChainFixer).GetMethod("OnCreated"));
        }

        public static unsafe void OnCreated(IntPtr @this, Target.TargetBehavior behavior, Target.TargetHandType handType)
        {
            Target_OnCreated.InvokeOriginal(@this, new IntPtr[]
            {
                new IntPtr((void*)(&behavior)),
                new IntPtr((void*)(&handType))
            });

            if (behavior == Target.TargetBehavior.Chain)
            {
                Target target = new Target(@this);

                Color rightColor;
                Color leftColor;

                if (config.handColor)
                {
                    rightColor = PlayerPreferences.I.GunColorRight.Get();
                    leftColor = PlayerPreferences.I.GunColorLeft.Get();
                }
                else
                {
                    rightColor = new Color(config.rightR, config.rightG, config.rightB);
                    leftColor = new Color(config.leftR, config.leftG, config.leftB);
                }

                if (handType == Target.TargetHandType.Right)
                {
                    target.chainLine.startColor = rightColor;
                    target.chainLine.endColor = rightColor;
                }
                else if (handType == Target.TargetHandType.Left)
                {
                    target.chainLine.startColor = leftColor;
                    target.chainLine.endColor = leftColor;
                }
                else
                {
                    target.chainLine.startColor = KataConfig.I.eitherHandColor;
                    target.chainLine.endColor = KataConfig.I.eitherHandColor;
                }
            }
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
                if (cue.handType == Target.TargetHandType.Either)
                {
                    chainStartEither = true;
                }
                else if (cue.handType == Target.TargetHandType.Right)
                {
                    chainStartRight = true;
                }
                else if (cue.handType == Target.TargetHandType.Left)
                {
                    chainStartLeft = true;
                }
                else if (cue.handType == Target.TargetHandType.None)
                {
                    chainStartNone = true;
                }
            }
            else if (cue.behavior == Target.TargetBehavior.Chain)
            {
                void SpawnTarget()
                {
                    TargetSpawner_SpawnCue.InvokeOriginal(@this, new IntPtr[]
                    {
                        c
                    });
                }
                if (cue.handType == Target.TargetHandType.Either)
                {
                    if (chainStartEither)
                    {
                        SpawnTarget();
                    }
                }
                else if (cue.handType == Target.TargetHandType.Right)
                {
                    if (chainStartRight)
                    {
                        SpawnTarget();
                    }
                }
                else if (cue.handType == Target.TargetHandType.Left)
                {
                    if (chainStartLeft)
                    {
                        SpawnTarget();
                    }
                }
                else if (cue.handType == Target.TargetHandType.None)
                {
                    if (chainStartNone)
                    {
                        SpawnTarget();
                    }
                }
            }
            else
            {
                TargetSpawner_SpawnCue.InvokeOriginal(@this, new IntPtr[]
                {
                    c
                });
                if (cue.handType == Target.TargetHandType.Either)
                {
                    chainStartEither = false;
                }
                else if (cue.handType == Target.TargetHandType.Right)
                {
                    chainStartRight = false;
                }
                else if (cue.handType == Target.TargetHandType.Left)
                {
                    chainStartLeft = false;
                }
                else if (cue.handType == Target.TargetHandType.None)
                {
                    chainStartNone = false;
                }
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
                    chainStartEither = false;
                    chainStartRight = false;
                    chainStartLeft = false;
                    chainStartNone = false;
                }

                //Updating state
                oldMenuState = menuState;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Target[] targets = UnityEngine.GameObject.FindObjectsOfType<Target>();

                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].chainLine.startColor = new Color(1, 1, 1, 1);
                    targets[i].chainLine.endColor = new Color(1, 1, 1, 1);
                }
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
