using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public Task<User> CreateUserAsync(User user)
        {
            user.Id = _nextId++;
            user.CreatedAt = DateTime.UtcNow;
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Task.FromResult(false);
            
            _users.Remove(user);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> UpdateUserAsync(int id, User userUpdate)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return Task.FromResult<User?>(null);
            
            existingUser.Username = userUpdate.Username;
            existingUser.Email = userUpdate.Email;
            existingUser.FirstName = userUpdate.FirstName;
            existingUser.LastName = userUpdate.LastName;
            existingUser.IsActive = userUpdate.IsActive;
            
            return Task.FromResult<User?>(existingUser);
        }
    }
} 