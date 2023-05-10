using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsWeb.Areas.Admin.Models;
using NewsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsWeb.Areas.Admin.Controllers
{
    [Route("Admin.html", Name ="AdminIndex")]
    [Area("Admin")]
    [Authorize()]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            //kt quyen truycap
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            //if (taikhoanID != null) return RedirectToAction("Index", "Home", new { Area = "Admin" });
            return View();


        }
    }    
}