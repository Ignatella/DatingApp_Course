using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext context;
        public DatingRepository(DataContext context) =>
            this.context = context;

        public void Add<T>(T entity) where T : class => 
           context.Add(entity);
        
        public void Delete<T>(T entity) where T : class =>
            context.Remove(entity);

        public async Task<Photo> GetMainPhotoForUser(int userId) =>
            await context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);

        public async Task<Photo> GetPhoto(int id) =>
          await  context.Photos.FirstOrDefaultAsync(photo => photo.Id == id);
    
        public async Task<User> GetUser(int id) => 
            await context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
          
        public async Task<PagedList<User>> GetUsers(UserParams userParams){

            var users = context.Users.Include(u => u.Photos).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if(userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                DateTime minDob = DateTime.Now.AddYears(-userParams.MaxAge - 1);
                DateTime maxDob = DateTime.Now.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth > minDob && u.DateOfBirth < maxDob);
            }

            return await PagedList<User>
                .CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
            
        public async Task<bool> SaveAll() => 
            await context.SaveChangesAsync() > 0;
    }
}