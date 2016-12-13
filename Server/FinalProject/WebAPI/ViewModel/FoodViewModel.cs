using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.ViewModel
{
    public class FoodViewModel
    {

    }
    public class OdersUser
    {
        public string iduser { get; set; }
        public List<Oders> listoders { get; set; }
    }
    public class Oders
    {
        public string id { get; set; }
        public string number { get; set; }
    }
}