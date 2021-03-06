﻿using Models.EF;
using Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Models.Admin.DAO
{
    public class OrderDetailDao
    {
        private DBContext db = null;

        public OrderDetailDao()
        {
            db = new DBContext();
        }

        public IEnumerable<OrderDetailViewModel> OrderDetailListAll()
        {
            var model = from a in db.OrderDetails
                        join b in db.Products
                        on a.ProductID equals b.ID
                        select new OrderDetailViewModel()
                        {
                            OrderID = a.OrderID,
                            ProductID = b.ID,
                            ProductImage = b.Image,
                            Price = a.Price,
                            ProductName = b.Name,
                            Quantity = a.Quantity
                        };
            return model.ToList();
        }

        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}