using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        public FOODEntities db = new FOODEntities();
        [Route("FindFood")]
        [HttpGet]
        public IEnumerable<usp_TimKiemMonAn_Result> FindFood(string name, string row, string count)
        {
            int convertrow = -1, convertcount = -1;

            if (!string.IsNullOrEmpty(row))
            {
                convertrow = int.Parse(row);
            }
            if (!string.IsNullOrEmpty(count))
            {
                convertrow = int.Parse(count);
            }
            if (!string.IsNullOrEmpty(row) && !string.IsNullOrEmpty(count))
            {
                return db.usp_TimKiemMonAn(name, convertrow, convertcount);
            }
            return db.usp_TimKiemMonAn(name, null, null);
        }
        [Route("AddFood")]
        [HttpPost]
        public bool AddFood(FOOD food)
        {
            db.usp_ThemMonAn(food.NAME, food.DECRIPTION, food.ISSALE, food.IDTYPE, food.IMGFOOD, food.PRICE, food.NUMBER);
            return true;
        }
        [Route("DeleteFood")]
        [HttpDelete]
        public bool DeleteFood(int id)
        {
            db.usp_XoaMonAn(id);
            return true;
        }
        [Route("EditFood")]
        [HttpPut]
        public bool EditFood(FOOD food)
        {
            db.Entry(food).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}
