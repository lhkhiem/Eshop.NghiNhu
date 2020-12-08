using Common;
using Models.Admin.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace EShop.Areas.Admin.Controllers
{
    public class UnitController : BaseController
    {
        [HasCredential(RoleID = "UNIT_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexUnit";
            return View();
        }

        public JsonResult LoadData()
        {
            var model = new UnitDao().ListAll();
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            byte status = 0;
            var ck = new UnitDao().CheckIsUsed(id);
            if (!ck)
            {
                var res = new UnitDao().Delete(id);
                if (res)
                {
                    status = 1;//xóa
                }
                else
                {
                    status = 0;//không xóa được
                }
            }
            else
                status = 2;//đang được sử dụng
            return Json(new
            {
                status = status
            }, JsonRequestBehavior.AllowGet);
        }
    }
}