using Harmony;
using MelonLoader;
using UnityEngine;
using System.Reflection;
using System;
using OVR.OpenVR;
using TwitchChatter;
using Il2CppSystem.Collections.Generic;

namespace ChainFixer
{
    internal static class Hooks
    {
        public static void ApplyHooks(HarmonyInstance instance)
        {
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(Target), "OnCreated", new Type[] {typeof(Target.TargetBehavior), typeof(Target.TargetHandType) })]
        private static class OnCreated
        {
            private static void Postfix(Target __instance, Target.TargetBehavior behavior, Target.TargetHandType handType)
            {
                if (behavior == Target.TargetBehavior.Chain)
                {
                    Target target = __instance;

                    Color rightColor;
                    Color leftColor;

                    if (ChainFixer.config.handColor)
                    {
                        rightColor = PlayerPreferences.I.GunColorRight.Get() / 2;
                        leftColor = PlayerPreferences.I.GunColorLeft.Get() / 2;
                    }
                    else
                    {
                        rightColor = new Color(ChainFixer.config.rightR, ChainFixer.config.rightG, ChainFixer.config.rightB);
                        leftColor = new Color(ChainFixer.config.leftR, ChainFixer.config.leftG, ChainFixer.config.leftB);
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
        }



        [HarmonyPatch(typeof(TargetSpawner), "SpawnCue", new Type[] { typeof(SongCues.Cue) })]
        private static class SpawnCue
        {
            private static void Prefix(TargetSpawner __instance, SongCues.Cue cue)
            {
                if (cue.behavior == Target.TargetBehavior.ChainStart)
                {
                    TargetSpawner_SpawnCue.InvokeOriginal(@this, new IntPtr[]
                    {
                    c
                    });
                    if (cue.handType == Target.TargetHandType.Either)
                    {
                        ChainFixer.chainStartEither = true;
                    }
                    else if (cue.handType == Target.TargetHandType.Right)
                    {
                        ChainFixer.chainStartRight = true;
                    }
                    else if (cue.handType == Target.TargetHandType.Left)
                    {
                        ChainFixer.chainStartLeft = true;
                    }
                    else if (cue.handType == Target.TargetHandType.None)
                    {
                        ChainFixer.chainStartNone = true;
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
                        if (ChainFixer.chainStartEither)
                        {
                            SpawnTarget();
                        }
                    }
                    else if (cue.handType == Target.TargetHandType.Right)
                    {
                        if (ChainFixer.chainStartRight)
                        {
                            SpawnTarget();
                        }
                    }
                    else if (cue.handType == Target.TargetHandType.Left)
                    {
                        if (ChainFixer.chainStartLeft)
                        {
                            SpawnTarget();
                        }
                    }
                    else if (cue.handType == Target.TargetHandType.None)
                    {
                        if (ChainFixer.chainStartNone)
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
                        ChainFixer.chainStartEither = false;
                    }
                    else if (cue.handType == Target.TargetHandType.Right)
                    {
                        ChainFixer.chainStartRight = false;
                    }
                    else if (cue.handType == Target.TargetHandType.Left)
                    {
                        ChainFixer.chainStartLeft = false;
                    }
                    else if (cue.handType == Target.TargetHandType.None)
                    {
                        ChainFixer.chainStartNone = false;
                    }
                }
            }
        }
    }
}
