using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Admin.DAO
{
    public class BlogCategoryDao
    {
        private DBContext db = null;

        public BlogCategoryDao()
        {
            db = new DBContext();
        }

        public IEnumerable<BlogCategory> ListAll()
        {
            return db.BlogCategories.ToList();
        }

        public long Insert(BlogCategory entity)
        {
            if (entity.ParentID == 0) entity.DisplayOrder = GetMaxDislayOrder() + 1;
            else entity.DisplayOrder = 0;

            entity.CreateDate = DateTime.Now;
            db.BlogCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(BlogCategory entity)
        {
            var model = db.BlogCategories.Find(entity.ID);
            model.Name = entity.Name;
            model.ParentID = entity.ParentID;
            model.ModifiedBy = entity.CreateBy;
            model.ModifiedDate = DateTime.Now;
            model.MetaTitle = entity.MetaTitle;
            db.SaveChanges();
            return entity.ID;
        }

        public BlogCategory GetByID(long id)
        {
            return db.BlogCategories.Find(id);
        }

        public bool CheckIDExist(long id)
        {
            var model = db.BlogCategories.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }

        public bool IsSubMenu(long? id)
        {
            var model = db.BlogCategories.Find(id);

            if (model.ParentID != 0)//không có menu con
                return true;
            else return false;
        }

        public bool Delete(long id)
        {
            try
            {
                var newsCategory = db.BlogCategories.Find(id);
                db.BlogCategories.Remove(newsCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckBlogIsUsed(long id)
        {
            var model = db.Blogs.FirstOrDefault(x => x.BlogCategoryID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }

        public int GetMaxDislayOrder()
        {
            var model = db.BlogCategories.Where(x => x.ParentID == 0).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder;
            }
            else return 0;
        }

        public bool ChangeOrder(int id, int order)
        {
            var menu = db.BlogCategories.Find(id);
            var item = db.BlogCategories.Where(x => x.DisplayOrder == order).ToList();
            if (item.Count == 0)
            {
                menu.DisplayOrder = order;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}