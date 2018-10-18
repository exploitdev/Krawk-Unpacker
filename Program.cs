using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System.Reflection;
using Krawk_Unpacker.Protections;

namespace Krawk_Unpacker
{
    class Program
    {
        public static bool veryVerbose = false;
        public static string Asmpath;
        public static ModuleDefMD module;
        public static Assembly asm;
        private static string path = null;
        public static int MathsAmount;
        static void Main(string[] args)
        {
            Console.Title = "Krawk Unpacker v2.0";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
==============================================================================
  _  __                   _      _    _                        _             
 | |/ /                  | |    | |  | |                      | |            
 | ' / _ __ __ ___      _| | __ | |  | |_ __  _ __   __ _  ___| | _____ _ __ 
 |  < | '__/ _` \ \ /\ / / |/ / | |  | | '_ \| '_ \ / _` |/ __| |/ / _ \ '__|
 | . \| | | (_| |\ V  V /|   <  | |__| | | | | |_) | (_| | (__|   <  __/ |   
 |_|\_\_|  \__,_| \_/\_/ |_|\_\  \____/|_| |_| .__/ \__,_|\___|_|\_\___|_|   
                                             | |                             
                                             |_|            {Release v2.0}                 
             Contact
 
   Skype:    live:krawkreverser
   Discord:  CrIsT#5619

          Program Info

   Language: C#
   Framework: 4.5.2
   Created Date: 10/07/2018
   Create By Krawk

  { Obs: Not Supported x86 Mixed Mode }
==============================================================================
");
            string diretorio = args[0];
            try
            {
                    Program.module = ModuleDefMD.Load(diretorio);
                    Program.asm = Assembly.LoadFrom(diretorio);
                    Program.Asmpath = diretorio;     
			}
			catch (Exception)
			{
                Console.WriteLine("Not .NET Assembly...");
			}
			string text = Path.GetDirectoryName(diretorio);
			bool flag = !text.EndsWith("\\");
			if (flag)
			{
				text += "\\";
			}
            Console.ForegroundColor = ConsoleColor.White;
            antitamper();
            Staticpacker();
            packer();
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[!] Removendo AntiDebugger");
                antidebugger.Run(module);
                Console.WriteLine("[!] Call Do AntiDebbuger Removida do Module");
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Remover o AntiDebugger");
            }
            try
            {
                Console.WriteLine("[!] Removendo AntiDumper");
                antidumper.Run(module);
                Console.WriteLine("[!] Call Do AntiDumper Removida do Module");
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Remover o AntiDumper");
            }
            try
            {
                Console.WriteLine("[!] Limpando Cases De Control-Flow");
                Protections.ControlFlowRun.cleaner(module);
                Console.WriteLine("[!] Sucesso ao Limpar Cases De Control-Flow");

            }
             catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Limpar Cases De Control-Flow");
            }
            try
            {
                Console.WriteLine("[!] Fixando Proxy-Calls");
                int amountProxy = Protections.ReferenceProxy.ProxyFixer(module);
                Console.WriteLine("[!] Proxy Calls Fixadas: " + amountProxy);
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Fixar Proxy-Calls");
            }
            try
            {
                Console.WriteLine("[!] Resolvendo Equações Matemáticas");
                MathsEquations.MathsFixer(module);
                Console.WriteLine("[!] Equações Matemáticas Resolvidas: " + MathsAmount);
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Resolver Equações Matemáticas");
            }
            try
            {
                Console.WriteLine("[!] Resolvendo SizeOf's");
                MathsEquations.SizeofRemove(module);
                Console.WriteLine("[!] SizeOf's Resolvidas: " + MathsAmount);
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Resolver SizeOf's");
            }
            try
            {
                Console.WriteLine("[!] Resolvendo Invokes");
                MathsEquations.SizeofRemove(module);
                Console.WriteLine("[!] Invokes Resolvidos Com Sucesso !");
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro em Remover Os Invokes !");
            }
            try
            {
                Console.WriteLine("[!] Removendo Strings Staticas");
                int strings = Protections.StaticStrings.Run(module);
                Console.WriteLine("[!] Strings Removidas: " + strings);
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Remover Strings Staticas");
            }
            try
            {
                Console.WriteLine("[!] Removendo Strings Dynamicas");
                int strings2 = Protections.Constants.constants();
                Console.WriteLine("[!] Strings Removidas: " + strings2);
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Remover Strings Staticas");
            }
            try
            {
                Console.WriteLine("[!] Decodificando Resources...");
                ResourcesDeobfuscator.Deobfuscate(module);
            }
            catch (Exception)
            {
                Console.WriteLine("[!] Erro ao Decodificar Resources");
            }
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[!] Removendo Attributes");

                Console.WriteLine("[!] Attributes Removidas: " + AttributeRemover.start(module));
            }
            catch { }
            string filename = string.Format("{0}{1}_Unpacked{2}", text, Path.GetFileNameWithoutExtension(diretorio), Path.GetExtension(diretorio));
            ModuleWriterOptions writerOptions = new ModuleWriterOptions(module);
            writerOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            writerOptions.Logger = DummyLogger.NoThrowInstance;
            Program.module.Write(filename, writerOptions);
			Console.WriteLine("");
			Console.WriteLine("Salvo Com Sucesso !");
			Console.ReadLine();
		}


        static void packer()
        {
                if (Protections.Packer.IsPacked(module))
                {
                Console.WriteLine("[!] Compressor Dynamico Detectado");
                    try
                    {
                    Protections.Packer.findLocal();
                        Console.WriteLine("[!] Compressor Removido");
                    }
                    catch
                    {
                    Console.WriteLine("[!] Compressor Erro ao Remover");
                    }

                    antitamper();
                    module.EntryPoint = module.ResolveToken(Protections.StaticPacker.epToken) as MethodDef;

                }
        }
        static void Staticpacker()
        {
                if (Protections.Packer.IsPacked(module))
                {
                Console.WriteLine("[!] Compressor Statico Detectado");
                    try
                    {
                    Protections.StaticPacker.Run(module);
                        Console.WriteLine("[!] Compressor Removido");
                    }
                    catch
                    {
                    Console.WriteLine("[!] Compressor Erro ao Remover");
                    }
                antitamper();
                    module.EntryPoint = module.ResolveToken(Protections.StaticPacker.epToken) as MethodDef;
                }
        }
        static void antitamper()
        {
                if (Protections.AntiTamper.IsTampered(module) == true)
                {
                    Console.WriteLine("[!] Anti-Tamper Detectado");

                    byte[] rawbytes = null;

                    var htdgfd = (module).MetaData.PEImage.CreateFullStream();

                    rawbytes = htdgfd.ReadBytes((int)htdgfd.Length);
                    try
                    {
                        module = Protections.AntiTamper.UnAntiTamper(module, rawbytes);
                        Console.WriteLine("[!] Anti-Tamper Removido Com Sucesso");
                    }
                    catch
                    {
                        Console.WriteLine("[!] Anti-Tamper Falhou em remover");
                    }

                }
          
        }

    }

}