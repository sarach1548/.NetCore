using myTask.Interfaces;
using myTask.Models;
namespace myTask.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

public class userService : IUserService
{
    List<User> users {get;}
    
    private string fileName ="users.json";
    public userService(IWebHostEnvironment  webHost)
    {
        this.fileName =Path.Combine(webHost.ContentRootPath,"Data" ,"users.json");
        using (var jsonFile = File.OpenText(fileName))
        {
            users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
           
            
        }
       
    }
    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(users));
    }

    //שליפת משתמש לפי ID
    public  User Get(int userId) 
    {
        return users.FirstOrDefault(p => p.Id == userId);
    }
    //שליפת כל המשתמשים הקיימים
    public List<User> GetAll() => users;

    //הוספת משתמש
    public void Add(User newUser)
    {
        newUser.Id = GetNextId();
        users.Add(newUser);
        saveToFile();
    }
    public void Update(User user)
    {
        var index = users.FindIndex(u => u.Id == user.Id);
        if (index == -1)
            return;

        users[index] = user;
        saveToFile();
    }
    //מחיקת משתמש לפי ID
    public void Delete(int userId)
    {
        var user = Get(userId);
        if (user is null)
            return;

        users.Remove(user);
        saveToFile();
    }

    public object GetToken(List<Claim> claims)
    {
        throw new NotImplementedException();
    }

    public int GetNextId()=>users.Max(user=>user.Id)+1;
    public int Count => users.Count();

    // private string GetDebuggerDisplay()
    // {
    //     return ToString();
    // }

}
    public static class UserUtils{
        public static void AddUser(this IServiceCollection service){
            service.AddSingleton<IUserService,userService>();
        }
    }


