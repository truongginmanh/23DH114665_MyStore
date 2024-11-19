﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _23DH114665_MyStore.Models.ViewModel
{
    public class CheckoutVM /* Lưu thông tin form Checkout */
    {
        public List<CartItem> CartItems { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal TotalAccount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus {  get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        public string ShippingMethod { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress {  get; set; }

        public string UserName { get; set; }

        // Các thuộc tính khác của đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }
    }
}