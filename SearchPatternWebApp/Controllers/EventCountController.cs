using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchPatternWebApp.Helpers;
using System.IO;
using SearchPatternWebApp.Models;

namespace SearchPatternWebApp.Controllers
{
    public class EventCountController : Controller
    {
        //
        // GET: /Index/
        public ActionResult Index()
        {
            string fileName1 = Server.MapPath(Url.Content("~/App_Data/myData.csv"));
            var eventParser = new EventCounter();

            try
            {
                using (var eventLog1 = new StreamReader(fileName1))
                {
                    eventParser.ParseEvents("HV1", eventLog1);
                }
            }
            catch (IOException)
            {
                throw;
            }

            var model = new EventCountModel { EventCount = eventParser.EventCounts.First() };

            return View(model);
        }
    }
}
