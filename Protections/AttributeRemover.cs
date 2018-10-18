using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
namespace Krawk_Unpacker.Protections
{
    class AttributeRemover
    {
        static IList<TypeDef> lista(ModuleDef A_0)
        {
            return A_0.Types;
        }
        public static int start(ModuleDefMD md)
        {
            int ret = 0;
            foreach (TypeDef type in md.GetTypes())
            {

                if (type.Name == "ConfusedByAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "ZYXDNGuarder") { lista(md).Remove(type); ret++; }
                if (type.Name == "YanoAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode") { lista(md).Remove(type); ret++; }
                if (type.Name == "SmartAssembly.Attributes.PoweredByAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "ObfuscatedByGoliath") { lista(md).Remove(type); ret++; }
                if (type.Name == "NineRays.Obfuscator.Evaluation") { lista(md).Remove(type); ret++; }
                if (type.Name == "EMyPID_8234_") { lista(md).Remove(type); ret++; }
                if (type.Name == "DotfuscatorAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "CryptoObfuscator.ProtectedWithCryptoObfuscatorAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "BabelObfuscatorAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == ".NETGuard") { lista(md).Remove(type); ret++; }
                if (type.Name == "OrangeHeapAttribute") { lista(md).Remove(type); ret++; }
                if (type.Name == "WTF") { lista(md).Remove(type); ret++; }
                if (type.Name == "<ObfuscatedByDotNetPatcher>") { lista(md).Remove(type); ret++; }
            }
            return ret;
        }
    }
}
