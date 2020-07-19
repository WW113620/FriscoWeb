using FriscoDev.Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriscoDev.UI.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly PMGDATABASEEntities _context;
        public AdministrationController(PMGDATABASEEntities context, IUserService userService)
        {
            this._context = context;
            this._userService = userService;
        }

        public ActionResult Index()
        {
            if (IsLogin())
                return RedirectToAction("List");

            return View();
        }

        [HttpPost]
        public JsonResult CheckPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    return Json(new BaseResult(1, "Please enter your password."));

                if (password == "135799")
                {
                    Session["AdministrationLogin"] = "success";
                    return Json(new BaseResult(0, "OK"));
                }

                return Json(new BaseResult(1, "Invalid password."));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, e.Message));
            }

        }

        public bool IsLogin()
        {
            string login = Session["AdministrationLogin"] == null ? "" : Session["AdministrationLogin"].ToString();
            if (!string.IsNullOrEmpty(login) && login.ToLower() == "success")
                return true;

            return false;
        }

        public ActionResult List()
        {
            if (!IsLogin())
                return RedirectToAction("Index");

            return View();
        }

        public JsonResult GetList(AdministrationRequest request)
        {
            LayuiPageResult<AdministrationViewModel> result = new LayuiPageResult<AdministrationViewModel>() { code = 1 };
            try
            {
                var response = this._userService.GetAdminList(request);
                foreach (var item in response.data)
                {
                    item.Active = item.UserActive ? "Active" : "Inactive";
                    item.AddTimeValue = CommonUtils.DateTimeValue(item.AddTime);
                }
                result = new LayuiPageResult<AdministrationViewModel>() { code = response.code, msg = response.msg, count = response.page.count, data = response.data };
            }
            catch (Exception e)
            {
                result.msg = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Inactive(string userId)
        {
            var user = this._context.Account.FirstOrDefault(p => p.UR_ID == userId);
            if (user != null)
            {
                var userList = this._context.Account.Where(p => p.CS_ID == user.CS_ID && p.UR_TYPE_ID == 3).ToList();
                if (userList != null && userList.Count > 0)
                {
                    foreach (var item in userList)
                        item.UR_ACTIVE = false;
                }
                user.UR_ACTIVE = false;
                this._context.SaveChanges();
            }
            return Json(new { code = 0 });
        }

        [HttpPost]
        public JsonResult Active(string userId)
        {
            var user = this._context.Account.FirstOrDefault(p => p.UR_ID == userId);
            if (user != null)
            {
                var userList = this._context.Account.Where(p => p.CS_ID == user.CS_ID && p.UR_TYPE_ID == 3).ToList();
                if (userList != null && userList.Count > 0)
                {
                    foreach (var item in userList)
                        item.UR_ACTIVE = true;
                }

                user.UR_ACTIVE = true;
                this._context.SaveChanges();
            }
            return Json(new { code = 0 });
        }

        [HttpPost]
        public JsonResult Delete(string userId)
        {
            var user = this._context.Account.FirstOrDefault(p => p.UR_ID == userId);
            if (user != null)
            {
                var customer = this._context.CustomerAccount.FirstOrDefault(p => p.Email == user.UR_NAME);
                if (customer != null)
                {
                    this._context.CustomerAccount.Remove(customer);
                }

                var userList = this._context.Account.Where(p => p.CS_ID == user.CS_ID && p.UR_TYPE_ID == 3).ToList();
                if (userList != null && userList.Count > 0)
                {
                    this._context.Account.RemoveRange(userList);
                }

                this._context.Account.Remove(user);
                this._context.SaveChanges();
            }
            return Json(new { code = 0 });
        }

        public ActionResult Add(string id)
        {
            if (!IsLogin())
                return RedirectToAction("Index");

            AdministrationViewModel model = new AdministrationViewModel() { IsAdd = 1 };
            if (string.IsNullOrEmpty(id))
            {
                model.IsAdd = 1;
            }
            else
            {

                model = _userService.GetAdministration(id);
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.UserId))
                    return Content("This user does not exist");

                if (model.UserType != 2)
                    return Content("Operation without permission");

                model.IsAdd = 2;
            }

            return View(model);
        }

        public ActionResult Password(string id)
        {
            if (!IsLogin())
                return RedirectToAction("Index");

            AdministrationViewModel model = new AdministrationViewModel() { IsAdd = 1 };

            model = _userService.GetAdministration(id);
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.UserId))
                return Content("This user does not exist");

            if (model.UserType != 2)
                return Content("Operation without permission");

            return View(model);
        }

        [HttpPost]
        public JsonResult ChangePassword(AdministrationViewModel model)
        {
            BaseResult result = new BaseResult();
            try
            {
                model.Email = model.Email.Trim();
                if (string.IsNullOrEmpty(model.Email))
                    return Json(new BaseResult(1, "User email is required"));

                //修改
                var account = this._context.Account.FirstOrDefault(p => p.UR_NAME == model.Email);
                if (account == null)
                    return Json(new BaseResult(1, "This user does not exist"));

                account.UR_PASSWD = model.Password;
                this._context.SaveChanges();


                string errorMsg = "";
                string body = string.Format(@"<div style='padding:10px;'>Login Email: <span style='margin-left: 5px;font-size: 16px;'>{0}</span></div>
                              <div style='padding:10px;'>New Password:<span style='margin-left: 5px;font-size: 16px;'>{1}</span></div>
                              <div style='padding:10px;'>Login Url: <span style='margin-left: 5px;font-size: 16px;'>{2}</span></div>", model.Email, model.Password, "http://stalkerfrisco.azurewebsites.net");
                bool bo = SendMail.Send("ACI-PMG", model.Email, "Change password", body, out errorMsg);

                return Json(new BaseResult(0, "Ok"));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception=" + e.Message));
            }
        }

        [HttpPost]
        public JsonResult Exist(string text)
        {
            string value = text.Trim();
            int count = this._context.Account.Count(p => p.UR_NAME == value);
            if (count > 0)
                return Json(new { code = 0 });

            return Json(new { code = 1 });
        }

        [HttpPost]
        public JsonResult SaveAdmin(AdministrationViewModel model)
        {
            BaseResult result = new BaseResult();
            try
            {
                model.Email = model.Email.Trim();
                if (string.IsNullOrEmpty(model.Email))
                    return Json(new BaseResult(1, "User email is required"));

                CustomerAccount customerAccount = new CustomerAccount
                {
                    Email = model.Email,
                    PoliceDeptName = model.PoliceDeptName,
                    Address = model.Address,
                    ContactOffice = model.ContactOffice,
                    ContactPhone = model.ContactPhone
                };
                //添加
                if (model.IsAdd == 1)
                {
                    var account = this._context.Account.FirstOrDefault(p => p.UR_NAME == model.Email);
                    if (account != null)
                        return Json(new BaseResult(1, "This email already exists"));

                    string UID_Guid = Guid.NewGuid().ToString().ToUpper();
                    string CS_Guid = Guid.NewGuid().ToString().ToUpper();
                    _userService.AddAccount(new Account
                    {
                        UR_ID = UID_Guid,
                        UR_NAME = model.Email,
                        UR_RealName = model.Email,
                        CS_ID = CS_Guid,
                        LN_ID = "D6CD181A-0270-456F-BE6F-8134FC564A46",
                        UR_PASSWD = model.Password,
                        UR_TYPE_ID = 2,
                        UR_ACTIVE = true,
                        UR_ADDTIME = DateTime.Now,
                        UR_UPTIME = null,
                        UR_STATUS = "0",
                        IS_ADMIN = true
                    });

                    UpdateCustomerAccount(customerAccount, model.Email);

                    string errorMsg = "";
                    string body = string.Format(@"<div style='padding:10px;'>Login Email: <span style='margin-left: 5px;font-size: 16px;'>{0}</span></div>
                              <div style='padding:10px;'>Login Password:<span style='margin-left: 5px;font-size: 16px;'>{1}</span></div>
                              <div style='padding:10px;'>Login Url: <span style='margin-left: 5px;font-size: 16px;'>{2}</span></div>", model.Email, model.Password, "http://stalkerfrisco.azurewebsites.net");
                    bool bo = SendMail.Send("ACI-PMG", model.Email, "Create account", body, out errorMsg);

                }
                else
                {
                    //修改
                    var account = this._context.Account.FirstOrDefault(p => p.UR_ID == model.UserId);
                    if (account == null)
                        return Json(new BaseResult(1, "This user does not exist"));

                    if (account.UR_NAME != model.Email)
                    {
                        int count = this._context.Account.Count(p => p.UR_NAME == model.Email);
                        if (count > 0)
                            return Json(new BaseResult(1, "This email already exists"));

                    }

                    UpdateCustomerAccount(customerAccount, account.UR_NAME);

                    account.UR_NAME = model.Email;
                    this._context.SaveChanges();
                }


                return Json(new BaseResult(0, "Ok"));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception=" + e.Message));
            }
        }

        public void UpdateCustomerAccount(CustomerAccount model, string userEmail)
        {
            var en = this._context.CustomerAccount.FirstOrDefault(p => p.Email == userEmail);
            if (en == null)
            {
                model.AddTime = DateTime.Now;
                this._context.CustomerAccount.Add(model);
            }
            else
            {
                en.Email = model.Email;
                en.PoliceDeptName = model.PoliceDeptName;
                en.Address = model.Address;
                en.ContactOffice = model.ContactOffice;
                en.ContactPhone = model.ContactPhone;
            }
            this._context.SaveChanges();
        }


    }
}