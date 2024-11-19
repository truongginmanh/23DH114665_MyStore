using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _23DH114665_MyStore.Models;
using _23DH114665_MyStore.Models.ViewModel;
using PagedList;

namespace _23DH114665_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Products
        public ActionResult Index(string SearchTerm, int? Page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();
            // Tìm kiếm sản phẩm dựa trên từ khóa
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                model.SearchTerm = SearchTerm;
                products = products.Where(p => p.ProductName.Contains(SearchTerm) ||
                    p.ProductDescription.Contains(SearchTerm) ||
                    p.Category.CategoryName.Contains(SearchTerm));
            }

            //Đoạn code liên quan đến phân trang
            //Lấy số trang hiện tại (mặc định là trang 1 nếu không có giá trị)
            int PageNumber = Page ?? 1;
            int PageSize = 10; //Số sản phẩm mỗi trang

            //Lấy top 10 sản phẩm bán chạy nhất
            model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(5).ToList();

            //Lấy 20 sản phẩm bán ế nhất và phân trang
            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(PageNumber,PageSize);

            return View(model);
        }

        //GET: Home/ProductDetails/5
        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if ( id == null ) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if ( pro == null )
            {
                return HttpNotFound();
            }
            // lấy tất cả các sản phẩm cùng danh mục
            var products = db.Products.Where( p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();

            ProductDetailsVM model = new ProductDetailsVM();

            //Đoạn code phân trang
            // Lấy số trang hiện tại mặc định là 1 nếu không có giá trị
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize; // Số sản phẩm mỗi trang
            model.product = pro;
            model.RelatedProducts = products.OrderBy(p => p.ProductID).Take(8).ToPagedList(pageNumber, pageSize); 
            model.TopProducts = products.OrderByDescending( p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }
            return View(model);
        }

        ////GET: Home
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}