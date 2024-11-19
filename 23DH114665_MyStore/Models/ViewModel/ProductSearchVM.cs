﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _23DH114665_MyStore.Models.ViewModel
{
    public class ProductSearchVM
    {
        //tiêu chí để search theo tên, mô tả sp
        //hoặc loiaj sảm phẩm
        public string SearchTerm { get; set; }

        //các tiêu chí để srearch theo giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        //Thứ tự sắp xếp
        public string SortOrder {  get; set; }

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //Trang hiện tại
        public int pageSize { get; set; } = 10;//Số sản phẩm mỗi trang

        //Danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> Products { get; set; }

        //danh sách sản phẩm thỏa điều kiện tìm kiếm
        //public List<Product> Products { get; set;}
    }
}