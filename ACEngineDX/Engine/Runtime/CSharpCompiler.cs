using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace ACEngine.Engine.Runtime
{
    class CSharpCompiler
    {
        public void Compile() { 
        CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

        // 2.ICodeComplier
        ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

        // 3.CompilerParameters
        CompilerParameters objCompilerParameters = new CompilerParameters();
        objCompilerParameters.ReferencedAssemblies.Add("System.dll");
        objCompilerParameters.GenerateExecutable =false;
        objCompilerParameters.GenerateInMemory =true;

        // 4.CompilerResults
        CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters,"");

            if (cr.Errors.HasErrors)
        {
            Console.WriteLine("编译错误：");
            foreach (CompilerError err in cr.Errors)
            {
                Console.WriteLine(err.ErrorText);
            }
        }
        else
        {
            // 通过反射，调用HelloWorld的实例
            Assembly objAssembly = cr.CompiledAssembly;
            object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
            MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");

            Console.WriteLine(objMI.Invoke(objHelloWorld, null));
        }

        Console.ReadLine();
    }
    }
}
