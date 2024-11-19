using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _23DH114665_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        //tiêu chí để search theo tên, mô tả sản phẩm
        //hoặc loại sản phẩm
        public string SearchTerm { get; set; }

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //Trang chủ hiện tại
        public int PageSize { get; set; } = 10; //số sản phẩm mỗi trang

        //danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set;}

        //Danh sách sản phẩm mới đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get ; set;}
    }
}