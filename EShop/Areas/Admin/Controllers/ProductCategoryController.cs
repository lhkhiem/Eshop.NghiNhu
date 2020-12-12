using Common;
using Models.Admin.DAO;
using Models.EF;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        [HasCredential(RoleID = "PRODUCTCATEGORY_VIEW")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCategory entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao().Insert(entity);
                if (dao > 0)
                {
                    SetAlert("success", "Thêm mới thành công");
                    SetDropdownList();
                    return View();
                }
            }
            else
            {
                SetAlert("error", "Thêm mới không thành công");
            }
            SetDropdownList(entity.ID);
            return View(entity);
        }

        public ActionResult Edit(long id)
        {
            SetDropdownList(id);
            var res = new ProductCategoryDao().GetByID(id);
            return View(res);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory entity)
        {
            if (ModelState.IsValid)
            {
                bool res = new ProductCategoryDao().Update(entity);
                SetDropdownList(entity.ParentID);
                if (res) SetAlert("success", "Cập nhật thành công!");
                else SetAlert("error", "Cập nhật không thành công!");
            }
            return View(entity);
        }

        public JsonResult LoadData()
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAll().OrderBy(x => x.DisplayOrder);
            //Sắp xếp theo danh mục cha
            var list = new List<ProductCategory>();
            foreach (var item in model)
            {
                if (item.ParentID == 0)
                {
                    list.Add(item);
                    var child = dao.GetChild(item.ID);
                    foreach (var subitem in child)
                    {
                        list.Add(subitem);
                    }
                }
            }
            return Json(new
            {
                data = list,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long id)
        {
            byte result;
            var dao = new ProductCategoryDao();
            if (!dao.CheckProductIsUsed(id))
            {
                if (!dao.HasChild(id))
                {
                    var res = new ProductCategoryDao().Delete(id);
                    if (res)
                    {
                        result = 1;//đã xóa
                    }
                    else
                    {
                        result = 0;//lỗi không xóa được
                    }
                }
                else result = 3;//có menu con ko cho xóa
            }
            else
                result = 2;//đang được sử dụng
            return Json(new
            {
                status = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(long id)
        {
            var res = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }

        public JsonResult ChangeOrder(long id, int order)
        {
            var res = new ProductCategoryDao().ChangeOrder(id, order);
            if (res)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public void SetDropdownList(long? selectedId = null)
        {
            var model = new ProductCategoryDao().ListAll();
            model = model.Where(x => x.ParentID == 0).OrderBy(x => x.DisplayOrder);
            List<SelectListItem> categoryList = new List<SelectListItem>();
            categoryList.Add(new SelectListItem()
            {
                Text = "---------",
                Value = "0",
            });
            foreach (var item in model)
            {
                categoryList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = selectedId != null && item.ID == selectedId ? true : false
                });
            }

            ViewBag.ParentID = categoryList;//new SelectList(model, "ID", "Name", selectedId);
        }

        public JsonResult ConvertString(string str)
        {
            string strConvert = StringHelper.ToUnsignString(str);
            return Json(new
            {
                str = strConvert
            }, JsonRequestBehavior.AllowGet);
        }
    }
}