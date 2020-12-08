using Common;
using Models.Admin.DAO;
using Models.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace EShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var res = dao.LoginByEmail(model.Email, Encryptor.MD5Hash(Encryptor.EncodeTo64(model.Password)), true);
                if (res == 1)
                {
                    var user = dao.GetUserExist(model.Email);
                    var userSession = new UserLoginSession();
                    userSession.UserID = user.ID;
                    userSession.UserGroupID = user.UserGroupID;
                    userSession.Name = user.Name;
                    var listCredentials = dao.GetListCredential(model.Email);

                    Session.Add(ConstantSession.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(ConstantSession.USER_SESSION, userSession);
                    ViewBag.UserName = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                    return RedirectToAction("Index", "Screen");
                }
                else if (res == 0)
                {
                    TempData["Notify"] = "Tài khoản không tồn tại!";
                }
                else if (res == -1)
                {
                    TempData["Notify"] = "Tài khoản đang tạm khóa!";
                }
                else if (res == -2)
                {
                    TempData["Notify"] = "Mật khẩu không đúng!";
                }
                else if (res == -3)
                {
                    TempData["Notify"] = "Tài khoản bạn không có quyền truy cập trang này!";
                }
            }
            else
            {
                TempData["Notify"] = "Vui lòng nhập đầy đủ thông tin";
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session[ConstantSession.USER_SESSION] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}