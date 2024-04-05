
using Microsoft.AspNetCore.Mvc;
using myTask.Models;
using myTask.Interfaces;
using myTask.Services;
using System.Security.Claims;
using System.Diagnostics;

namespace myTask.Controllers{
[ApiController]
[Route("[controller]")]
public class loginController : ControllerBase
{
    IUserService userService;
    

    public loginController(IUserService userService){
        this.userService=userService;
    }

    [HttpPost("/api/login")]
    public ActionResult<string> login([FromBody] User user){
        User myUser=userService.GetAll().FirstOrDefault(u=>u.Name==user.Name&&u.Password==user.Password);
        if(myUser==null)
            return Unauthorized();
        var claims = new List<Claim>
        {
            new Claim("Type","User"),
            new Claim("Id",myUser.Id.ToString())
        };
        if(myUser.Type=="admin")
            claims.Add(new Claim("Type","Admin"));
        var token= tokenService.GetToken(claims);
        
        return new OkObjectResult(tokenService.WriteToken(token));


         }   
    }
}

