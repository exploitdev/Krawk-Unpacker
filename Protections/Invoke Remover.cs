using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System.Reflection;
using dnlib.DotNet.Writer;

namespace Krawk_Unpacker.Protections
{
    class Invoke_Remover
    {
        public static void run(ModuleDefMD module) {
                int count = 0;
                foreach (TypeDef type in module.GetTypes())
                {
                    if (!type.IsGlobalModuleType) continue;
                    foreach (MethodDef method in type.Methods)
                    {
                            if (!method.HasBody && !method.Body.HasInstructions) continue;
                            for (int i = 0; i < method.Body.Instructions.Count; i++)
                            {
                                if (method.Body.Instructions[i].OpCode == OpCodes.Call && method.Body.Instructions[i].Operand.ToString().Contains("CallingAssembly"))
                                {
                                    method.Body.Instructions[i].Operand = (method.Body.Instructions[i].Operand = module.Import(typeof(Assembly).GetMethod("GetExecutingAssembly")));
                                    count++;
                                }
                            }
                    }
            }  
            }
    }
}









