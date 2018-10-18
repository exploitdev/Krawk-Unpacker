using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Krawk_Unpacker.Protections
{
    internal static class antidumper
    {
        internal static int FindInstructionsNumber(this MethodDef method, OpCode opCode, object operand)
        {
            var num = 0;
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode != opCode)
                    continue;
                if (operand is int)
                {
                    var value = instruction.GetLdcI4Value();
                    if (value == (int)operand)
                        num++;
                }
                else if (operand is string)
                {
                    var value = instruction.Operand.ToString();
                    if (value.Contains(operand.ToString()))
                        num++;
                }
            }
            return num;
        }
        internal static bool Run(ModuleDefMD module)
        {
            var instructions = module.GlobalType.FindStaticConstructor().Body.Instructions;
            foreach (var instr in instructions)
            {
                if (instr.OpCode != OpCodes.Call)
                    continue;

                var dumperMethod = instr.Operand as MethodDef;
                if (dumperMethod == null)
                    continue;
                if (!dumperMethod.DeclaringType.IsGlobalModuleType)
                    continue;

                const MethodAttributes attributes = MethodAttributes.Assembly | MethodAttributes.Static |
                                                    MethodAttributes.HideBySig;
                if (dumperMethod.Attributes != attributes)
                    continue;
                if (dumperMethod.CodeType != MethodImplAttributes.IL)
                    continue;

                if (dumperMethod.ReturnType.ElementType != ElementType.Void)
                    continue;

            
                if (dumperMethod.FindInstructionsNumber(OpCodes.Call, "(System.Byte*,System.Int32,System.UInt32,System.UInt32&)") != 14)
                    continue;
                instr.OpCode = OpCodes.Nop;
                instr.Operand = null;
                return true;
            }
            return false;
        }
    }
}
