using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Admin.DAO
{
    public class VisitorDao
    {
        private DBContext db = null;

        public VisitorDao()
        {
            db = new DBContext();
        }

        public IEnumerable<Visitor> ListAll()
        {
            return db.Visitors.ToList();
        }

        public string Insert(Visitor entity)
        {
            db.Visitors.Add(entity);
            db.SaveChanges();
            return entity.Email;
        }

        public Visitor GetByEmail(string email)
        {
            return db.Visitors.Find(email);
        }

        public bool CheckEmailExist(string email)
        {
            var model = db.Visitors.Find(email);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }

        public bool Delete(string email)
        {
            try
            {
                var model = db.Visitors.Find(email);
                db.Visitors.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}