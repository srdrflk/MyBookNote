using BookNoteCommon;
using MyBooknote.BookNoteCore.DataAccess;
using MyBooknote.DataAccessLayer;
using MyBooknote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBooknote.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IRepository<T> where T : class
    {
        private DatabaseContext db;
        private DbSet<T> _Objset;

        public Repository()
        {
            db = RepositoryBase.CreateContext();
            _Objset = db.Set<T>();
        }

        public List<T> List()
        {
            return _Objset.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _Objset.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _Objset.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _Objset.Add(obj);

            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.common.GetCurrentUserName();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.common.GetCurrentUserName();
            }

            return Save();
        }

        public int Delete(T obj)
        {
            _Objset.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _Objset.FirstOrDefault(where);
        }
    }
}
