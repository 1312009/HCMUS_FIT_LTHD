using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using WebAPI.Data;
namespace WebAPI.Controllers
{
    [RoutePrefix("api/foods")]
    public class FOODController : ApiController
    {
        public FOODEntities db = new FOODEntities();
        /// <summary>
        /// Lấy danh sách thức ăn
        /// </summary>
        /// <returns></returns>
        [Route("GetAllFoods")]
        [HttpGet]
        public List<FOOD> GetFOODs()
        {
            var foods = from a in db.FOODs
                        select a;
            return foods.ToList();
        }
        [Route("GetFood")]
        [HttpGet]
        public FOOD GetFood(string id)
        {
            int convert = -1;
            if(!string.IsNullOrEmpty(id))
            {
                convert = int.Parse(id);
            }
            return db.FOODs.Find(convert);
        }
        [Route("TopLike")]
        [HttpGet]
        public IEnumerable<usp_TopMonAnThich_Result> GetTopLike()
        {
            return db.usp_TopMonAnThich();
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

