using Common;
using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Admin.DAO
{
    public class BlogDao
    {
        private DBContext db = null;

        public BlogDao()
        {
            db = new DBContext();
        }

        public IEnumerable<BlogViewModel> ListAll()
        {
            var model = from a in db.Blogs
                        join b in db.BlogCategories
                        on a.BlogCategoryID equals b.ID
                        select new BlogViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            Status = a.Status,
                            Detail = a.Detail,
                            Tag = a.Tag,
                            BlogCategoryID = a.BlogCategoryID,
                            BlogCategoryName = b.Name,
                            BlogCategoryMetaTitle = b.MetaTitle
                        };
            return model.ToList();
        }

        public List<BlogViewModel> ListAllByTag(string tagId)
        {
            var model = (from a in db.Blogs
                         join b in db.BlogTags on a.ID equals b.BlogID
                         join c in db.BlogCategories on a.BlogCategoryID equals c.ID
                         where b.TagID == tagId
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreateDate = a.CreateDate,
                             CreateBy = a.CreateBy,
                             ID = a.ID,
                             BlogCategoryName = c.Name
                         }).AsEnumerable().Select(x => new BlogViewModel()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateBy = x.CreateBy,
                             CreateDate = x.CreateDate,
                             ID = x.ID,
                             BlogCategoryName = x.BlogCategoryName
                         });
            return model.ToList();
        }

        public List<BlogViewModel> ListAllByCategory(long cateId)
        {
            var model = (from a in db.Blogs
                         join c in db.BlogCategories on a.BlogCategoryID equals c.ID
                         where c.ID == cateId
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreateDate = a.CreateDate,
                             CreateBy = a.CreateBy,
                             ID = a.ID,
                             BlogCategoryName = c.Name
                         }).AsEnumerable().Select(x => new BlogViewModel()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateBy = x.CreateBy,
                             CreateDate = x.CreateDate,
                             ID = x.ID,
                             BlogCategoryName = x.BlogCategoryName
                         });
            return model.ToList();
        }

        public bool Insert(Blog entity)
        {
            try
            {
                entity.Status = true;
                entity.CreateDate = DateTime.Now;
                entity.MetaKeywords = entity.MetaTitle;
                entity.MetaDescriptions = entity.MetaTitle;
                db.Blogs.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        var existedBlog = this.CheckBlogTag(entity.ID, tagId);

                        if (!existedTag)
                        {
                            //insert to Tag table
                            this.InsertTag(tagId, tag);
                        }
                        //Insert to BlogTag table
                        if (!existedBlog)
                        {
                            this.InsertBlogTag(entity.ID, tagId);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Blog entity)
        {
            try
            {
                var model = db.Blogs.Find(entity.ID);
                model.Name = entity.Name;
                model.Description = entity.Description;
                model.Image = entity.Image;
                model.MetaDescriptions = entity.MetaDescriptions;
                model.MetaKeywords = entity.MetaKeywords;
                model.MetaTitle = entity.MetaTitle;
                model.ModifiedBy = entity.ModifiedBy;
                model.ModifiedDate = DateTime.Now;
                model.Detail = entity.Detail;
                model.Tag = entity.Tag;
                model.BlogCategoryID = entity.BlogCategoryID;
                db.SaveChanges();
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    this.RemoveAllBlogTag(entity.ID);
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        var existedBlog = this.CheckBlogTag(entity.ID, tagId);

                        if (!existedTag)
                        {
                            //insert to Tag table
                            this.InsertTag(tagId, tag);
                        }
                        //Insert to BlogTag table
                        if (!existedBlog)
                        {
                            this.InsertBlogTag(entity.ID, tagId);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Blog GetByID(long id)
        {
            return db.Blogs.Find(id);
        }

        public BlogViewModel GetByIDView(long id)
        {
            var model = from a in db.Blogs.Where(x => x.ID.Equals(id))
                        join b in db.BlogCategories
                        on a.BlogCategoryID equals b.ID
                        select new BlogViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            Status = a.Status,
                            Detail = a.Detail,
                            Tag = a.Tag,
                            BlogCategoryID = a.BlogCategoryID,
                            BlogCategoryName = b.Name,
                            BlogCategoryMetaTitle = b.MetaTitle
                        };
            return model.FirstOrDefault();
        }

        public bool Delete(long id)
        {
            try
            {
                var entity = db.Blogs.Find(id);
                db.Blogs.Remove(entity);
                db.SaveChanges();
                this.RemoveAllBlogTag(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var entity = db.Blogs.Find(id);
            entity.Status = !entity.Status;
            db.SaveChanges();
            return entity.Status;
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertBlogTag(long newsId, string tagId)
        {
            var newsTag = new BlogTag();
            newsTag.BlogID = newsId;
            newsTag.TagID = tagId;
            db.BlogTags.Add(newsTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        public bool CheckBlogTag(long newsId, string tagId)
        {
            return db.BlogTags.Count(x => x.TagID == tagId && x.BlogID == newsId) > 0;
        }

        public void RemoveAllBlogTag(long newsId)
        {
            db.BlogTags.RemoveRange(db.BlogTags.Where(x => x.BlogID == newsId));
            db.SaveChanges();
        }

        public List<Tag> ListTag(long newsId)
        {
            var model = (from a in db.Tags
                         join b in db.BlogTags on a.ID equals b.TagID
                         where b.BlogID == newsId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }

        public List<BlogViewModel> ListRelateBlog(long id)
        {
            var entity = this.GetByIDView(id);
            var list = this.ListAll();
            return list.Where(x => x.ID != id && x.BlogCategoryID.Equals(entity.BlogCategoryID)).ToList();
        }

        public List<BlogViewModel> ListRecentBlog(int number)
        {
            return this.ListAll().OrderByDescending(x => x.CreateDate).Take(number).ToList();
        }

        public List<string> ListName(string keyword)
        {
            return db.Blogs.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
    }
}