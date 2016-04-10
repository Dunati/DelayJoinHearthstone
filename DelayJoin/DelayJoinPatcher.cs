
using System;
using System.Collections;
using System.IO;
using Mono.Cecil;
using System.Linq;
using Mono.Cecil.Cil;
using DelayJoinLib;

public class DelayJoinPatcher
{
    public static void Main(string[] args)
    {
        string assemblyName = "Assembly-CSharp.dll";
        string outAssemblyName = "Assembly-CSharp.Patched.dll";
        if (!File.Exists(assemblyName))
        {
            Console.WriteLine("Unable to find " + assemblyName + ", Please copy and run from Hearthstone/Hearthstone_Data/Managed.");
            return;
        }
        Patch(assemblyName, outAssemblyName);
        return;
    }

    private static void Patch(string assemblyName, string outAssemblyName)
    {
        try
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assemblyName);

            MethodDefinition findGame = assembly.MainModule.Types.First(x => x.Name == "GameMgr").Methods.First(x => x.Name == "FindGame");
            MethodReference delayedFindGame = assembly.MainModule.Import(typeof(DelayJoin).GetMethod("FindGame"));
            MethodDefinition normalFindGame = assembly.MainModule.Types.First(x => x.Name == "Network").Methods.First(x => x.Name == "FindGame");

            ILProcessor processor = findGame.Body.GetILProcessor();
            var instructions = findGame.Body.Instructions;
            var lastCall = instructions[findGame.Body.Instructions.Count - 2];
            if (lastCall.OpCode.Code == Mono.Cecil.Cil.Code.Callvirt && ((MethodReference)lastCall.Operand).Resolve() == normalFindGame)
            {
                Console.WriteLine("Patching "+assemblyName);
                processor.Replace(lastCall.Previous.Previous.Previous.Previous.Previous, processor.Create(OpCodes.Ldarg_0));
                processor.Replace(lastCall, processor.Create(OpCodes.Call, delayedFindGame));
                assembly.Write(outAssemblyName);
                Console.WriteLine("Written to "+outAssemblyName);
            }
            else if (lastCall.OpCode.Code == Mono.Cecil.Cil.Code.Call && ((MethodReference)lastCall.Operand).Resolve() == delayedFindGame.Resolve())
            {
                Console.WriteLine("Already Patched");
            }
            else
            {
                Console.WriteLine("Unable to find patch location");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Patching failed: " + e.Message);
            return;
        }
    }

}