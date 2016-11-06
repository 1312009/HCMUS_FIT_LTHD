using System.Linq;
using System.Net;
using WebAPI.Data;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebAPI.Controllers
{
    public class FOODController : ApiController
    {
        private FOODEntities db = new FOODEntities();

        // GET: api/FOODs
        public IQueryable<FOOD> GetFOODs()
        {
            return db.FOODs;
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

