using System.Linq;
using System.Net;
using WebAPI.Data;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/foods")]
    [Authorize(Roles = "CUSTOMER")]
    public class FOODController : ApiController
    {
        public FOODEntities1 db = new FOODEntities1();
        [Route("")]
        [HttpGet]
        public List<FOOD> GetFOODs()
        {
            return db.FOODs.ToList();
        }

        // GET: api/FOODs/5
        [ResponseType(typeof(FOOD))]
        public async Task<IHttpActionResult> GetFOOD(int id)
        {
            FOOD FOOD = await db.FOODs.FindAsync(id);
            if (FOOD == null)
            {
                return NotFound();
            }

            return Ok(FOOD);
        }

        // PUT: api/FOODs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFOOD(int id, FOOD FOOD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != FOOD.ID)
            {
                return BadRequest();
            }

            db.Entry(FOOD).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FOODExists(id))
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

        // POST: api/FOODs
        [ResponseType(typeof(FOOD))]
        public async Task<IHttpActionResult> PostFOOD(FOOD FOOD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FOODs.Add(FOOD);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = FOOD.ID }, FOOD);
        }

        // DELETE: api/FOODs/5
        [ResponseType(typeof(FOOD))]
        public async Task<IHttpActionResult> DeleteFOOD(int id)
        {
            FOOD FOOD = await db.FOODs.FindAsync(id);
            if (FOOD == null)
            {
                return NotFound();
            }

            db.FOODs.Remove(FOOD);
            await db.SaveChangesAsync();

            return Ok(FOOD);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FOODExists(int id)
        {
            return db.FOODs.Count(e => e.ID == id) > 0;
        }
    }
}

