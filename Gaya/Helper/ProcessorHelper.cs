using Gaya.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Gaya.Helper
{
    public static class ProcessorHelper
    {

        static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processors.json");

        public static List<Processor> GetAllProcessors()
        {
            if (!File.Exists(path))
                return new List<Processor>();

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Processor>>(json);
        }

        private static Processor GetSpecificProcessor(List<Processor> processors, int id) => processors.FirstOrDefault(p => p.Id == id);

        public static int AddProcessor(string name, string action, string description, bool firstAsString, bool secondAsString)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(action))
                return -1;

            var processors = GetAllProcessors();
            processors.Add(new Processor()
            {
                Id = processors.Count,
                Name = name,
                Action = action,
                Description = description,
                FirstParameterAsString = firstAsString,
                SecondParameterAsString = secondAsString
            });

            File.WriteAllText(path, JsonConvert.SerializeObject(processors));

            return processors.Count - 1;
        }

        public static object ExecuteProcessor(Processor processor, string first, string second)
        {
            if (processor == null)
                return "failed, processor not found";

            object res;
            try
            {
                res = Compiler.CompileAndRun(processor, first, second);
            }
            catch (Exception e)
            {
                throw e;
            }

            return res;
        }
    }
}