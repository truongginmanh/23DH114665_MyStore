﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using System.Web.Security;
using _23DH114665_MyStore.Models;
using _23DH114665_MyStore.Models.ViewModel;

namespace _23DH114665_MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        //GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }
                //Nếu chưa tồn tại thi tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,//Lưu ý: nên mã hóa mật khẩu trước khi lưu
                    UserRole = "Customer"
                };
                db.Users.Add(user);
                // tạo bảng ghi thông tin khách hàng trong bảng Customer
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);
                // Lưu thông tin tài khoản và thông tin khách hàng vào CSDL
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        //GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username
                && u.Password == model.Password
                && u.UserRole == "Customer");
                if (user != null)
                {
                    // Lưu trạng thái đăng nhập vào session
                    Session["UserName"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    // Lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, true);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }
        // GET:Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}