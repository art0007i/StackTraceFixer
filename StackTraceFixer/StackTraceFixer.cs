using HarmonyLib;
using NeosModLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using FrooxEngine;
using System.Reflection.Emit;
using BaseX;
using FrooxEngine.LogiX;

namespace StackTraceFixer
{
    public class StackTraceFixer : NeosMod
    {
        public override string Name => "StackTraceFixer";
        public override string Author => "art0007i";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/art0007i/StackTraceFixer/";

        private const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        public static HashSet<MethodInfo> toPatch = new HashSet<MethodInfo>
        {
            AccessTools.TypeByName("FrooxEngine.LogiX.LogixHelper+<EnumerateAllReachableNodes>d__17").GetMethod("MoveNext", flags),
            typeof(WorldConfiguration).GetMethod("FieldChanged", flags),

            // if you want more stack traces to be stopped just add them here!
        };

        public static HashSet<MethodInfo> logMethods = new HashSet<MethodInfo>
        {
            typeof(UniLog).GetMethod("Log", new Type[] { typeof(string), typeof(bool) }),
            typeof(UniLog).GetMethod("Log", new Type[] { typeof(object), typeof(bool) }),
            typeof(UniLog).GetMethod("Warning"),
            typeof(UniLog).GetMethod("Error")
        };
        
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("me.art0007i.StackTraceFixer");

            MethodInfo transpiler = typeof(StackTraceFixerPatch).GetMethod("Transpiler");

            foreach(MethodInfo method in toPatch)
            {
                Debug("Patching method " + method);
                harmony.Patch(method, transpiler: new HarmonyMethod(transpiler));
            }
        }
        

        class StackTraceFixerPatch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
            {
                List<CodeInstruction> instructions = new List<CodeInstruction>(codes);
                for (int i = 0; i < instructions.Count; i++)
                {
                    // detect integer one (aka true) on stack followed by a call
                    if(instructions[i].opcode == OpCodes.Ldc_I4_1 
                        && instructions[i+1].opcode == OpCodes.Call)
                    {
                        // Is it a UniLog method?
                        if (logMethods.Contains(instructions[i + 1].operand))
                        {
                            // Make it not print a stack trace
                            instructions[i].opcode = OpCodes.Ldc_I4_0;

                            Debug($"Found a {instructions[i + 1].operand}");
                        }
                    }
                }
                Debug("Done!");
                return instructions;
            }
        }
    }
}