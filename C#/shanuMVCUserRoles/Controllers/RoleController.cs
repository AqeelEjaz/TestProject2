using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shanuMVCUserRoles.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace shanuMVCUserRoles.Controllers
{
    [Authorize(Roles = "Admin")]
	public class RoleController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		ApplicationDbContext context;

		public RoleController()
		{
			context = new ApplicationDbContext();
		}

		/// <summary>
		/// Get All Roles
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{

			if (User.Identity.IsAuthenticated)
			{


				if (!isAdminUser())
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

			var Roles = context.Roles.ToList();
			return View(Roles);

		}
		public Boolean isAdminUser()
		{

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = User.Identity;
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var s = UserManager.GetRoles(user.GetUserId());

                   // int ss = Convert.ToInt32(s);

                    if (s[0].ToString() == "Admin")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e , e);
            }
			return false;
		}
		/// <summary>
		/// Create  a New role
		/// </summary>
		/// <returns></returns>
		public ActionResult Create()
		{
			if (User.Identity.IsAuthenticated)
			{


				if (!isAdminUser())
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

			var Role = new IdentityRole();
			return View(Role);
		}

		/// <summary>
		/// Create a New Role
		/// </summary>
		/// <param name="Role"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Create(IdentityRole Role)
		{
			if (User.Identity.IsAuthenticated)
			{
				if (!isAdminUser())
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

			context.Roles.Add(Role);
            //context.Roles.Remove(Role);
            //context.Entry(Role).State = EntityState.Modified;
            context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}