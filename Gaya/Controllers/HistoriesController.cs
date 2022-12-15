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
using Gaya.Models;

namespace Gaya.Controllers
{
    public class HistoriesController : ApiController
    {
        private GayaContext db = new GayaContext();

        // GET: api/Histories
        public IQueryable<History> GetHistories()
        {
            return db.Histories.Include(h => h.Processor);
        }

        // GET: api/Histories
        public object GetHistories(int processorId, int count)
        {
            var histories = GetHistories().Where(h => h.ProcessorId == processorId);

            if (!histories.Any())
                return null;

            bool addClimax = !histories.FirstOrDefault().Processor.FirstParameterAsString && !histories.FirstOrDefault().Processor.SecondParameterAsString;
            float f;

            return new
            {
                Last3 = histories.Take(count),
                CurrentMonth = histories.Count(h => h.ExecutionDate.Value.Month == DateTime.Now.Month && h.ExecutionDate.Value.Year == DateTime.Now.Year),
                Climax = addClimax ? new 
                {
                    Max = histories.Max(h => h.Result),
                    Min = histories.Min(h => h.Result),
                    //Avg = histories.Average(h => float.TryParse(h.Result, out f) ? f : 0)
                    } : null
            };
        }

        float GetFloat(string s) => float.TryParse(s, out float f) ? f : 0;

        // GET: api/Histories/5
        [ResponseType(typeof(History))]
        public async Task<IHttpActionResult> GetHistory(int id)
        {
            History history = await db.Histories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }

        // PUT: api/Histories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHistory(int id, History history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != history.Id)
            {
                return BadRequest();
            }

            db.Entry(history).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryExists(id))
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

        // POST: api/Histories
        [ResponseType(typeof(History))]
        public async Task<IHttpActionResult> PostHistory(string firstField, string secondField, int processorId, string result)
        {
            var history = new History { FirstField = firstField, SecondField = secondField, ProcessorId = processorId, Result = result, ExecutionDate = DateTime.Now};
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Histories.Add(history);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = history.Id }, history);
        }

        // DELETE: api/Histories/5
        [ResponseType(typeof(History))]
        public async Task<IHttpActionResult> DeleteHistory(int id)
        {
            History history = await db.Histories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            db.Histories.Remove(history);
            await db.SaveChangesAsync();

            return Ok(history);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HistoryExists(int id)
        {
            return db.Histories.Count(e => e.Id == id) > 0;
        }
    }
}