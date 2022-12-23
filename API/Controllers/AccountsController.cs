using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.ViewModels;
using API.Repositories.Data;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AccountsController : ControllerBase
{
    private AccountRepositories _repo;

    public AccountsController(AccountRepositories repo)
    {
        _repo = repo;
    }

    [HttpPost]
    [Route("Register")]
    public ActionResult Register(RegisterVM registerVM)
    {
        try
        {
            var result = _repo.Register(registerVM);
            return result == 0 
                ? Ok(new { statusCode = 204, message = "Email or Phone is Already Registered!" }) 
                : Ok(new { statusCode = 201, message = "Register Succesfully!" });
        }
        catch (Exception e)
        {
            return BadRequest(new { statusCode = 500, message = $"Something Wrong! : {e.Message}" });
        }
    }

    [HttpPost]
    [Route("Login")]
    public ActionResult Login(LoginVM loginVM)
    {
        try
        {
            var result = _repo.Login(loginVM);
            return result switch
            {
                0 => Ok(new { statusCode = 200, message = "Account Not Found!" }),
                1 => Ok(new { statusCode = 200, message = "Wrong Password!" }),
                2 => Ok(new { statusCode = 200, message = "Success!" })
            }; 
        }
        catch (Exception e)
        {
            return BadRequest(new { statusCode = 500, message = $"Something Wrong! : {e.Message}" });
        }

    }


    [HttpGet]
    public ActionResult GetAll()
    {
        try
        {
            var result = _repo.Get();
            return result.Count() == 0
            ? Ok(new { statusCode = 204, message = "Data Not Found!" })
            : Ok(new { statusCode = 201, message = "Success", data = result });
        }
        catch (Exception e)
        {
            return BadRequest(new { statusCode = 500, message = $"Something Wrong! : {e.Message}" });
        }

    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetById(string id)
    {
        try
        {
            var result = _repo.Get(id);
            return result == null
            ? Ok(new { statusCode = 204, message = $"Data With Id {id} Not Found!" })
            : Ok(new { statusCode = 201, message = $"Account with NIK {id}", data = result });
        }
        catch (Exception e)
        {
            return BadRequest(new { statusCode = 500, message = $"Something Wrong! : {e.Message}" });
        }
    }

    [HttpPost]
    public ActionResult Insert(Account account)
    {
        try
        {
            var result = _repo.Insert(account);
            return result == 0 ? Ok(new { statusCode = 204, message = "Data failed to Insert!" }) :
            Ok(new { statusCode = 201, message = "Data Saved Succesfully!" });
        }
        catch
        {
            return BadRequest(new { statusCode = 500, message = "" });
        }
    }

    [HttpPut]
    public ActionResult Update(Account account)
    {
        try
        {
            var result = _repo.Update(account);
            return result == 0 ?
            Ok(new { statusCode = 204, message = $"NIK {account.NIK} not found!" }) :
            Ok(new { statusCode = 201, message = "Update Succesfully!" });
        }
        catch
        {
            return BadRequest(new { statusCode = 500, message = "Something Wrong!" });
        }
    }

    [HttpDelete]

    public ActionResult Delete(string id)
    {
        try
        {
            var result = _repo.Delete(id);
            return result == 0 ? Ok(new { statusCode = 204, message = $"Id {id} Data Not Found" }) :
            Ok(new { statusCode = 201, message = "Data Delete Succesfully!" });
        }
        catch (Exception e)
        {
            return BadRequest(new { statusCode = 500, message = $"Something Wrong {e.Message}" });
        }
    }
}
