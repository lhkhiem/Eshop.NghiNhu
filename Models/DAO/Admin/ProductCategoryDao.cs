using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Admin.DAO
{
    public class ProductCategoryDao
    {
        private DBContext db = null;

        public ProductCategoryDao()
        {
            db = new DBContext();
        }

        public IEnumerable<ProductCategory> ListAll()
        {
            return db.ProductCategories.ToList();
        }

        public long Insert(ProductCategory entity)
        {
            if (entity.ParentID == 0) entity.DisplayOrder = GetMaxDislayOrder() + 1;
            else entity.DisplayOrder = 0;

            entity.CreateDate = DateTime.Now;
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(ProductCategory entity)
        {
            var model = db.ProductCategories.Find(entity.ID);
            model.Name = entity.Name;
            model.ParentID = entity.ParentID;
            model.Image = entity.Image;
            model.ModifiedBy = entity.CreateBy;
            model.ModifiedDate = DateTime.Now;
            model.MetaTitle = entity.MetaTitle;
            db.SaveChanges();
            return true;
        }

        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool CheckIDExist(long id)
        {
            var model = db.ProductCategories.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }

        public bool HasChild(long? id)
        {
            if (db.ProductCategories.FirstOrDefault(x => x.ParentID == id) != null)
                return true;
            else return false;
        }

        public IEnumerable<ProductCategory> GetChild(long id)
        {
            return db.ProductCategories.Where(x => x.ParentID == id);
        }

        public bool Delete(long id)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(productCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckProductIsUsed(long id)
        {
            var model = db.Products.FirstOrDefault(x => x.ProductCategoryID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }

        public int GetMaxDislayOrder()
        {
            var model = db.ProductCategories.Where(x => x.ParentID == 0).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder; ;
            }
            else return 0;
        }

        public bool ChangeOrder(long id, int order)
        {
            var productCategory = db.ProductCategories.Find(id);
            var item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order);
            if (productCategory.DisplayOrder != order)
            {
                if (item == null)
                {
                    productCategory.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    item.DisplayOrder = productCategory.DisplayOrder;
                    productCategory.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
            }
            else return false;
        }

        public bool ChangeStatus(long id)
        {
            var productCategory = db.ProductCategories.Find(id);
            productCategory.Status = !productCategory.Status;
            db.SaveChanges();
            return productCategory.Status;
        }
    }
}