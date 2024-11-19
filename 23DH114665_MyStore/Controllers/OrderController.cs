using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _23DH114665_MyStore.Models;
using _23DH114665_MyStore.Models.ViewModel;


namespace _23DH114665_MyStore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        // GET: Order/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            //Kiểm tra giỏ hàng trong sesion
            //nếu giỏ hàng rỗng thì chuyển về trang chủ
            var cart = Session["Cart"] as List<CartItem>;
                if(cart == null || !cart.Any()) {
                return RedirectToAction("Index", "Home");
            }
            //Xác thực người dùng đã đăng nhập chưa, nếu chưa thì chuyển hướng đến trang đăng nhập
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if(user == null) { return RedirectToAction("Login", "Account"); }

            //Lấy thông tin khách hàng từ CSDL, nếu chưa có thì chuyển hướng đến trang đăng nhập
            //Nếu có rồi thì lấy địa chỉ khách hàng và gán vào ShippingAddress của CheckoutVM
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);  
            if(customer == null) { return RedirectToAction("Login", "Account"); }

            var model = new CheckoutVM // Tạo CSDL hiển thị cho CheckoutVM
            {
                CartItems = cart, //Lấy danh sách các sản phẩm trong giỏ hàng
                TotalAccount = cart.Sum(item => item.TotalPrice), //Tổng giá trị các sản phẩm trong giỏ
                OrderDate = DateTime.Now, //Mặc định lấy bằng thời điểm đặt hàng
                ShippingAddress = customer.CustomerAddress, //Lấy địa chỉ mặc định từ bảng Customer
                CustomerId = customer.CustomerID, //Lấy mã khách hàng từ bảng Customer
                UserName = customer.Username, //Lấy tên đăng nhập từ bảng Customer
            };
            return View(model);
        }
        // POST:Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if(ModelState.IsValid)
            {
                var cart = Session["Cart"] as Cart /*List<CartItem>*/;
                //nếu giỏ hàng rỗng thì chuyển về trang chủ
                if (cart.Items == null || !cart.Items.Any())
                {
                    return RedirectToAction("Index", "Home");
                }
                //nếu chưa đăng nhập thì chuyển hướng đến trang đăng nhập
                var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }
                //nếu khách hàng không khớp với tên đăng nhập thì chuyển đến Login
                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }
                //Nếu người dùng chọn thanh toán bằng Paypal, sẽ điều hướng đến trang PaymentWithPaypal
                if (model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "Paypal", model);
                }
                //Thiết lập PaymentStatus dựa trên PaymentMethod
                string paymentStatus = "Chưa thanh toán";
                switch(model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Paypal": paymentStatus = " Thanh toán Paypal";break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp";break;
                    default: paymentStatus = "Chưa thanh toán";break;
                }
                //Tạo đơn hàng và chi tiết đơn hàng liên quan 
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAccount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.Items.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };
                //Lưu đơn hàng vào CSDL
                db.Orders.Add(order);
                db.SaveChanges();
                //Xóa giỏ hàng sau khi đặt thành công
                Session["Cart"] = null;
                //Điều hướng đến trang xác nhận đơn hàng
                return RedirectToAction("OrderSccess", new {id = order.OrderID});
            }
            return View(model);
        }
    }
}