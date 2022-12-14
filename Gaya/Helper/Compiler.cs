using Gaya.Models;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Gaya.Helper
{
    public static class Compiler
    {
        static string[] CodeWithString(string executeAction) => new string[] {
            "using System;"+
            "namespace Gaya.Helper"+
            "{"+
            "   public class ProcessAction"+
            "   {"+
            "       static public object Main(string str, string str2)"+
            "       {"+
            "           return str." + executeAction + "(str2);"+
            "       }"+
            "   }"+
            "}"};

        static string[] CodeWithFirstAsString(string executeAction, string secondParameter) => new string[] {
            "using System;"+
            "namespace Gaya.Helper"+
            "{"+
            "   public class ProcessAction"+
            "   {"+
            "       static public object Main(string str, string str2)"+
            "       {"+
            "           return str." + executeAction + "(" + secondParameter + ");"+
            "       }"+
            "   }"+
            "}"};

        static string[] CodeWithSecondAsString(string executeAction, string firstParameter) => new string[] {
            "using System;"+
            "namespace Gaya.Helper"+
            "{"+
            "   public class ProcessAction"+
            "   {"+
            "       static public object Main(string str, string str2)"+
            "       {"+
            "           return " + firstParameter + "." + executeAction + "(str2);"+
            "       }"+
            "   }"+
            "}"};

        static string[] CodeWithoutString(string executeAction, string firstParameter, string secondParameter) => new string[] {
            "using System;"+
            "namespace Gaya.Helper"+
            "{"+
            "   public class ProcessAction"+
            "   {"+
            "       static public object Main(string str, string str2)"+
            "       {"+
            "           return " + firstParameter + executeAction + secondParameter + ";"+
            "       }"+
            "   }"+
            "}"};

        private static string[] GetCode(Processor executeAction, string firstParameter, string secondParameter)
        {
            if (executeAction.FirstParameterAsString && executeAction.SecondParameterAsString)
                return CodeWithString(executeAction.Action);
            if (executeAction.FirstParameterAsString && !executeAction.SecondParameterAsString)
                return CodeWithFirstAsString(executeAction.Action, secondParameter);
            if (!executeAction.FirstParameterAsString && executeAction.SecondParameterAsString)
                return CodeWithSecondAsString(executeAction.Action, firstParameter);
            return CodeWithoutString(executeAction.Action, firstParameter, secondParameter);
        }

        public static object CompileAndRun(Processor executeAction, string firstParameter, string secondParameter)
        {
            object result = null; ;
            CompilerParameters CompilerParams = new CompilerParameters();

            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";

            string[] references = { "System.dll" };
            CompilerParams.ReferencedAssemblies.AddRange(references);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, GetCode(executeAction, firstParameter, secondParameter));

            if (compile.Errors.HasErrors)
            {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors)
                {
                    text += "rn" + ce.ToString();
                }
                throw new Exception(text);
            }

            Module module = compile.CompiledAssembly.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;

            if (module != null)
            {
                mt = module.GetType("Gaya.Helper.ProcessAction");
            }

            if (mt != null)
            {
                methInfo = mt.GetMethod("Main");
            }

            if (methInfo != null)
            {
                result = methInfo.Invoke(null, new object[] { firstParameter, secondParameter });
            }

            return result;
        }
    }
}