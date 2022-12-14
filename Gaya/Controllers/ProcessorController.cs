using Gaya.Helper;
using Gaya.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Gaya.Controllers
{
    public class ProcessorController : ApiController
    {


        // GET: api/processor
        public List<Processor> Get() => ProcessorHelper.GetAllProcessors();
        
        // GET: api/processor/1
        public IHttpActionResult Get(int id, string first, string second)
        {
            object res;
            try
            {
                res = ProcessorHelper.ExecuteProcessor(id, first, second);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (res is string result && result.Contains("failed"))
                return BadRequest(result);

            return Ok(res);
        }
        
        // POST: api/processor
        public IHttpActionResult Post(string name, string action, string description, bool firstAsString, bool secondAsString)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(action))
                return BadRequest("you must insert name and action");

            return Ok(ProcessorHelper.AddProcessor(name, action, description, firstAsString, secondAsString));
        }
    }

    public class ProcessorController1 : ApiController
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processors.json");
        private Processor GetSpecificProcessor(List<Processor> processors, int id) => processors.FirstOrDefault(p => p.Id == id);


        // GET: api/processor
        public List<Processor> Get()
        {
            if (!File.Exists(path))
                return new List<Processor>();

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Processor>>(json);
        }

        // GET: api/processor/1
        public IHttpActionResult Get(int id, string first, string second)
        {
            var processors = Get();
            if (first == null || second == null)
                return NotFound();

            var processor = GetSpecificProcessor(processors, id);
            if (processor == null)
                return NotFound();

            object res;
            try
            {
                res = Eval(string.Format(processor.Action, first, second));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(res);
        }

        // POST: api/processor
        public IHttpActionResult Post(string name, string action, string description)
        {
            var processors = Get();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(action))
                return BadRequest("you must insert name and action");

            processors.Add(new Processor() { Id = processors.Count, Name = name, Action = action, Description = description });

            File.WriteAllText(path, JsonConvert.SerializeObject(processors));

            return Ok(processors.Count - 1);
        }

        static object Eval(String expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            return table.Compute(expression, String.Empty);
        }
    }
}
