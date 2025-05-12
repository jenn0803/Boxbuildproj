//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BoxBuildproj.Services
//{
//    public class CategoryService
//    {
//        private readonly BoxBuildprojContext _context;

//        public CategoryService(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<Category>> GetCategoriesAsync()
//        {
//            return await _context.Categories.ToListAsync();
//        }

//        public async Task<Category> GetCategoryByIdAsync(int id)
//        {
//            return await _context.Categories.FindAsync(id);
//        }

//        public async Task AddCategoryAsync(Category category)
//        {
//            _context.Categories.Add(category);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateCategoryAsync(Category category)
//        {
//            _context.Categories.Update(category);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteCategoryAsync(int id)
//        {
//            var category = await _context.Categories.FindAsync(id);
//            if (category != null)
//            {
//                _context.Categories.Remove(category);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}
