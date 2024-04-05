using myTask.Interfaces;
using myTask.Models;
namespace myTask.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;


public class taskService : ITaskService
{
    List<TheTask> tasks {get;}
    
    private string fileName ="tasks.json";
    public taskService(IWebHostEnvironment  webHost)
    {
        this.fileName =Path.Combine(webHost.ContentRootPath,"Data" ,"tasks.json");
        using (var jsonFile = File.OpenText(fileName))
        {
            tasks = JsonSerializer.Deserialize<List<TheTask>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });  
        }
    }
    
    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(tasks));
    }

    //שליפת כל המשימות של משתמש מסויים
    public List<TheTask> GetAll(int userId)
    {
        List<TheTask> userTasks=new List<TheTask>();
        userTasks=tasks.Where(t=>t.UserId==userId).ToList();
        return userTasks;
    }

    //שליפה של משימה מסויימת של משתמש מסויים
    public TheTask Get(int taskId, int userId)
    {
        return tasks.FirstOrDefault(t=> t.Id==taskId&&t.UserId==userId);
    }

    //הוספת משימה מסויימת למשתמש מסויים
    public void Add(int userId, TheTask newTask)
    {
        newTask.UserId=userId;
        newTask.Id = GetNextId();
        tasks.Add(newTask);
        saveToFile();
    }

   
    //עידכון משימה
    public void Update(TheTask task)
    {
        var index = tasks.FindIndex(t => t.Id == task.Id);
        if (index == -1)
            return;

        tasks[index] = task;
        saveToFile();
    }

    // מחיקת משימה
    public void Delete(int taskId, int userId)
    {
        TheTask task = Get(taskId,userId);
        if (task is null)
            return;

        tasks.Remove(task);
        saveToFile();
    }
    public void DeleteByUserId(int userId)
    {
        tasks.RemoveAll(task => task.UserId == userId);
    }

    public int GetNextId()=>tasks.Max(task=>task.Id)+1;
    //מספר המשימות
    public int Count => tasks.Count();

}
    public static class TaskUtils{
        public static void AddTask(this IServiceCollection service)
        {
            service.AddSingleton<ITaskService,taskService>();
        }
    }
