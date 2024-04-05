using myTask.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace myTask.Interfaces
{
    public interface IUserService
    {
        //שליפת משתמש לפי מזהה
        List<User> GetAll();
        User Get(int userId);
        void Add(User user); 
        void Update(User user);
        void Delete(int id);
        object GetToken(List<Claim> claims);

        int Count { get; } // Added missing semicolon
    }
}
