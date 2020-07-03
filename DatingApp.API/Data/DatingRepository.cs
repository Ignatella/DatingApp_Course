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
          
        public async Task<PagedList<User>> GetUsers(UserParams userParams) =>
            await PagedList<User>
                .CreateAsync(context.Users.Include(u => u.Photos), userParams.PageNumber, userParams.PageSize);
    
        public async Task<bool> SaveAll() => 
            await context.SaveChangesAsync() > 0;
    }
}