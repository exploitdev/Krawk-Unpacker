using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Krawk_Unpacker.Protections
{
    internal static class antidebugger
    {
        internal static bool Run(ModuleDefMD krawk)
        {
            var instructions = krawk.GlobalType.FindStaticConstructor().Body.Instructions;

            foreach (var instruction in instructions)
            {
                if (instruction.OpCode != OpCodes.Call)
                    continue;
                var metodo = instruction.Operand as MethodDef;
                if (metodo == null)
                    continue;
                if (metodo.FindInstructionsNumber(OpCodes.Ldstr, "ENABLE_PROFILING") != 1)
                    continue;
                if (metodo.FindInstructionsNumber(OpCodes.Ldstr, "GetEnvironmentVariable") != 1)
                    continue;
                if (metodo.FindInstructionsNumber(OpCodes.Call, "System.Environment::FailFast(System.String)") != 1)
                    continue;
                instruction.OpCode = OpCodes.Nop;
                instruction.Operand = null;
                return true;
            }
            return false;
        }
    }
}
