using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Gaya.Data;
using Gaya.Helper;
using Gaya.Models;

namespace Gaya.Controllers
{
    public class ProcessorsController : ApiController
    {
        private GayaContext db = new GayaContext();

        // GET: api/Processors
        public IQueryable<Processor> GetProcessors()
        {
            return db.Processors;
        }

        // GET: api/Processors/5
        [ResponseType(typeof(Processor))]
        public async Task<IHttpActionResult> GetProcessor(int id)
        {
            Processor processor = await db.Processors.FindAsync(id);
            if (processor == null)
            {
                return NotFound();
            }

            return Ok(processor);
        }

        // GET: api/Processors/5
        [ResponseType(typeof(Processor))]
        public async Task<IHttpActionResult> GetProcessor(int id, string first, string second)
        {
            Processor processor = await db.Processors.FindAsync(id);
            if (processor == null)
            {
                return NotFound();
            }

            object res;
            try
            {
                res = ProcessorHelper.ExecuteProcessor(processor, first, second);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (res is string result && result.Contains("failed"))
                return BadRequest(result);

            return Ok(res);
        }

        // PUT: api/Processors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProcessor(int id, Processor processor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != processor.Id)
            {
                return BadRequest();
            }

            db.Entry(processor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Processors
        [ResponseType(typeof(Processor))]
        public async Task<IHttpActionResult> PostProcessor(string name, string action, string description, bool firstAsString, bool secondAsString)
        {
            var processor = new Processor { Name = name, Action = action, Description = description, FirstParameterAsString = firstAsString, SecondParameterAsString = secondAsString };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Processors.Add(processor);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = processor.Id }, processor);
        }

        // DELETE: api/Processors/5
        [ResponseType(typeof(Processor))]
        public async Task<IHttpActionResult> DeleteProcessor(int id)
        {
            Processor processor = await db.Processors.FindAsync(id);
            if (processor == null)
            {
                return NotFound();
            }

            db.Processors.Remove(processor);
            await db.SaveChangesAsync();

            return Ok(processor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProcessorExists(int id)
        {
            return db.Processors.Count(e => e.Id == id) > 0;
        }
    }
}