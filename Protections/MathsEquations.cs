using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Krawk_Unpacker.Protections
{
	internal class MathsEquations
	{
        public static void SizeofRemove(ModuleDef module)
        {
            foreach (TypeDef type in module.Types)
            {
                //On scanne toutes les méthodes du type
                foreach (MethodDef method in type.Methods)
                {
                    //On vérifie si la méthode est "correcte"
                    if (method.HasBody && method.Body.HasInstructions)
                    {
                        //On scanne instructions par instruction
                        for (int i = 1; i < method.Body.Instructions.Count - 1; i++)
                        {
                            if (method.Body.Instructions[i].OpCode == OpCodes.Sizeof)
                            {
                                try
                                {
                                    var siz = method.Body.Instructions[i].Operand.ToString();
                                    switch (siz)
                                    {
                                        case "System.Int16":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 2;

                                            break;
                                        case "System.Int32":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.Int64":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 8;

                                            break;
                                        case "System.Byte":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 1;

                                            break;
                                        case "System.SByte":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 1;

                                            break;
                                        case "System.UInt16":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 2;

                                            break;
                                        case "System.UInt32":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.UInt64":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 8;

                                            break;
                                        case "System.Double":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 8;

                                            break;
                                        case "System.Single":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.Boolean":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 1;

                                            break;
                                        case "System.Decimal":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 16;

                                            break;
                                        case "System.IntPtr":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.Convert":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.Guid":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 16;

                                            break;
                                        case "System.Type":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.String":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 1;

                                            break;
                                        case "System.UIntPtr":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 4;

                                            break;
                                        case "System.DateTime":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 8;
                                            break;
                                        case "System.Console":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 2;

                                            break;
                                        case "System.Windows.Forms.RightToLeft":
                                            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                                            method.Body.Instructions[i].Operand = 2;

                                            break;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }

        public static void MathsFixer(ModuleDefMD module)
		{
			foreach (TypeDef typeDef in module.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					bool flag = !methodDef.HasBody;
					if (!flag)
					{
						for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
						{
							bool flag2 = methodDef.Body.Instructions[i].OpCode == OpCodes.Add;
							if (flag2)
							{
								bool flag3 = methodDef.Body.Instructions[i - 1].IsLdcI4() && methodDef.Body.Instructions[i - 2].IsLdcI4();
								if (flag3)
								{
									int num = methodDef.Body.Instructions[i - 2].GetLdcI4Value() + methodDef.Body.Instructions[i - 1].GetLdcI4Value();
									methodDef.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
									methodDef.Body.Instructions[i].Operand = num;
									methodDef.Body.Instructions[i - 2].OpCode = OpCodes.Nop;
									methodDef.Body.Instructions[i - 1].OpCode = OpCodes.Nop;
									Program.MathsAmount++;
								}
							}
							else
							{
								bool flag4 = methodDef.Body.Instructions[i].OpCode == OpCodes.Mul;
								if (flag4)
								{
									bool flag5 = methodDef.Body.Instructions[i - 1].IsLdcI4() && methodDef.Body.Instructions[i - 2].IsLdcI4();
									if (flag5)
									{
										int num2 = methodDef.Body.Instructions[i - 2].GetLdcI4Value() * methodDef.Body.Instructions[i - 1].GetLdcI4Value();
										methodDef.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
										methodDef.Body.Instructions[i].Operand = num2;
										methodDef.Body.Instructions[i - 2].OpCode = OpCodes.Nop;
										methodDef.Body.Instructions[i - 1].OpCode = OpCodes.Nop;
										Program.MathsAmount++;
									}
								}
								else
								{
									bool flag6 = methodDef.Body.Instructions[i].OpCode == OpCodes.Sub;
									if (flag6)
									{
										bool flag7 = methodDef.Body.Instructions[i - 1].IsLdcI4() && methodDef.Body.Instructions[i - 2].IsLdcI4();
										if (flag7)
										{
											int num3 = methodDef.Body.Instructions[i - 2].GetLdcI4Value() - methodDef.Body.Instructions[i - 1].GetLdcI4Value();
											methodDef.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
											methodDef.Body.Instructions[i].Operand = num3;
											methodDef.Body.Instructions[i - 2].OpCode = OpCodes.Nop;
											methodDef.Body.Instructions[i - 1].OpCode = OpCodes.Nop;
											Program.MathsAmount++;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
