using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewShop.Models;
using NewShop.Repository;

namespace NewShop.Controllers
{
    public class ReserveController : Controller
    {
        private readonly ReserveRepository _repo = new ReserveRepository();

        // GET: /Reserve
        public ActionResult Index() {
            return View();
        }

        // GET: /Reserve/Search?CUSCOD=..&SLMCOD=..  (ทุก parameter เป็น optional)
        [HttpGet]
        public JsonResult Search(ReserveFilterModel filter) {
            var data = _repo.Search(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: /Reserve/ChartData?CUSCOD=..  (ใช้ filter ชุดเดียวกับ Search)
        [HttpGet]
        public JsonResult ChartData(ReserveFilterModel filter) {
            var data = _repo.Search(filter);

            var summary = new {
                labels = new[] { "Reserve(Dinner)", "Reserve(EV)", "Reserve(Influenser)", "Confirmed", "Actual", "ConfirmDinner" },
                values = new[]
                {
                    data.Sum(x => ParseIntSafe(x.Reserve1)),
                    data.Sum(x => ParseIntSafe(x.Reserve2)),
                    data.Sum(x => ParseIntSafe(x.Reserve3)),
                    data.Sum(x => x.Confirmed ?? 0),
                    data.Sum(x => x.Actual ?? 0),        // เพิ่มบรรทัดนี้ — วางก่อน Confirm_Dinner
                    data.Sum(x => x.ConfirmDinner ?? 0)
                },
                totalRows = data.Count
            };

            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// แปลง string เป็น int อย่างปลอดภัย สำหรับคำนวณผลรวม Reserve1/2/3
        /// ค่าที่ไม่ใช่ตัวเลข (เช่น "T84") จะถูกนับเป็น 0 ในกราฟ ไม่ throw exception
        /// </summary>
        private int ParseIntSafe(string value) {
            int result;
            return int.TryParse(value, out result) ? result : 0;
        }
    }
}
