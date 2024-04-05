using Microsoft.AspNetCore.Mvc;
using myTask.Models;
using myTask.Interfaces;
using Microsoft.AspNetCore.Authorization;
using myTask.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Diagnostics;

namespace myTask.Controllers{
    [ApiController]
[Route("/api/user")]
public class userController : ControllerBase
{
    IUserService userService;
    ITaskService taskService;
    readonly int UserId;

    // public object UserService { get; private set; }

    public userController(IUserService userService,ITaskService taskService, IHttpContextAccessor httpContextAccessor)
    {
        this.userService = userService;
        this.taskService = taskService;
        this.UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);
    }

     [HttpGet("/api/Allusers")]
     [Authorize(Policy="Admin")]
     public ActionResult<List<User>> GetAll()=>
         userService.GetAll();
    
    //שליפת משתמש לפי מזהה
    [HttpGet]
    [Authorize(Policy="User")]

    public ActionResult<User> Get()
    {
        // var userId = Convert.ToInt32(User.FindFirst("Id").Value);
        var user = userService.Get(UserId);
        if (user == null)
            return NotFound();
        return user;
    }


    [HttpPost]
    [Authorize(Policy="Admin")]

    public IActionResult Create(User user)
    {
        if(user is null)
            return BadRequest("user is null");
        userService.Add(user);
        return CreatedAtAction(nameof(Create), new {id=user.Id}, user);

    }
        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Policy="Admin")]
        public IActionResult Delete(int userId)
        {
            var user = userService.Get(userId);
            if (user is null)
                return  NotFound();

            userService.Delete(userId);
            taskService.DeleteByUserId(userId);

            return Content(userService.Count.ToString());
        }

    [HttpPut]
        [Authorize(Policy = "User")]

        public IActionResult Update([FromBody]User user)
        {
            // var userId = Convert.ToInt32(User.FindFirst("Id").Value);
            var userType = User.FindFirst("Type").Value.ToLower();
            // Console.WriteLine(userType);
            var existingUser = userService.Get(UserId);
            if (existingUser is null)
                return NotFound();
            user.Id = UserId;
            user.Type=userType;
            userService.Update(user);

            return NoContent();
        }}

}
