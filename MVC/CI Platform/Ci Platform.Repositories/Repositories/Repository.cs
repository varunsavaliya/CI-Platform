using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(T model)
        {
            _context.Add(model);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public bool IsRegistered(User user)
        {
            return (_context.Users.Any(a => a.Email == user.Email)) != null;
        }
        public bool IsRegistered(PasswordReset user)
        {
            return (_context.Users.Any(a => a.Email == user.Email)) != null;

        }
        public bool ComparePassword(User user)
        {
            return (_context.Users.Where(a => (a.Email.Equals(user.Email)) && (a.Password.Equals(user.Password))).FirstOrDefault()) != null;
        }

    }
}
