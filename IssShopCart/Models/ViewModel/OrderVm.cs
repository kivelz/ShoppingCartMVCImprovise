using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssShopCart.Models.ViewModel
{
    public class OrderVm
    {
        public OrderVm()
        {

        }

        public OrderVm(Order row)
        {
            OrderId = row.Id;
            UserId = row.UserId;
            CreatedAt = row.CreatedAt;
        }
        public int OrderId { get; set; }
        public  int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
       
    }
}