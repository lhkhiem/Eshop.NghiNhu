using Common;
using Models.Admin.DAO;
using System.Web.Mvc;
using Models.EF;

namespace EShop.Areas.Admin.Controllers
{
    public class UnitController : BaseController
    {
        [HasCredential(RoleID = "UNIT_VIEW")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Unit entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UnitDao().Insert(entity);
                if (dao > 0)
                {
                    SetAlert("success", "Thêm mới thành công");
                    return View();
                }
            }
            else
            {
                SetAlert("error", "Thêm mới không thành công");
            }
            return View(entity);
        }

        public ActionResult Edit(byte id)
        {
            var res = new UnitDao().GetByID(id);
            return View(res);
        }

        [HttpPost]
        public ActionResult Edit(Unit entity)
        {
            if (ModelState.IsValid)
            {
                bool res = new UnitDao().Update(entity);
                if (res) SetAlert("success", "Cập nhật thành công!");
                else SetAlert("error", "Cập nhật không thành công!");
            }
            return View(entity);
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
            byte result = 0;
            var ck = new UnitDao().CheckIsUsed(id);
            if (!ck)
            {
                var res = new UnitDao().Delete(id);
                if (res)
                {
                    result = 1;//đã xóa
                }
                else
                {
                    result = 0;//không xóa được
                }
            }
            else
                result = 2;//đang được sử dụng
            return Json(new
            {
                status = result
            }, JsonRequestBehavior.AllowGet);
        }
    }
}