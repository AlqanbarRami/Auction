using Auktioner.Models;
using Auktioner.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auktioner.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Customer> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<Customer> signInManager;
        private readonly IInventoryRepository inventoryRepository;
        public AdminController(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Customer> signInManager, IInventoryRepository inventoryRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.inventoryRepository = inventoryRepository;
        }

   

        public IActionResult Index()
        {

            return View();
        }


        public IActionResult AddUserAsAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsAdmin(AddUserAsAdmin addUserViewModel)
        {
            var user = new Customer()
            {
                Email = addUserViewModel.Email,
                UserName = addUserViewModel.Email,
                FirstName = addUserViewModel.FirstName,
                LastName = addUserViewModel.LastName,
                City = addUserViewModel.City,
                Country = addUserViewModel.Country,
                Address = addUserViewModel.Address
            };
            IdentityResult result = await userManager.CreateAsync(user, addUserViewModel.Password);
            await signInManager.SignInAsync(user, isPersistent: false);
            
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                var user1 = await userManager.FindByEmailAsync(addUserViewModel.Email);
                if (user1 != null)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                  
                }  
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", userManager.Users);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addUserViewModel);
        }


        //  to show the users only without admin
        public async Task<IActionResult> GetUsersAsync()
        {
            List<Customer> customer = new List<Customer>();
            var role = await roleManager.FindByNameAsync("Admin");
            foreach (var user in userManager.Users)
            {
                if (!(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    customer.Add(user);
                }
            }
            return View(customer);
        }

        // GET And send user Id to post
        public IActionResult NewAdmin(string userId)
        {
            var roleViewModel = new RoleViewModel
            {
                UserId = userId
            };


            return View(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> NewAdmin(RoleViewModel userRoleViewModel)
        {
            var myUser = await userManager.FindByIdAsync(userRoleViewModel.UserId);
            var myRole = await roleManager.FindByNameAsync("Admin");
            var found = await userManager.AddToRoleAsync(myUser, myRole.Name);

            if (found.Succeeded)
            {
                return RedirectToAction("Index");
            }


            return View(userRoleViewModel);
        }

        //To show me the only admin
        public async Task<IActionResult> GetAdminsAsync()
        {
            List<Customer> customer = new List<Customer>();
            var role = await roleManager.FindByNameAsync("Admin");
            foreach (var admin in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(admin, role.Name))
                {
                    customer.Add(admin);
                }
            }
            return View(customer);
        }

        public IActionResult DeleteAdmin(string userId)
        {
            var roleViewModel = new RoleViewModel
            {
                UserId = userId
            };


            return View(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(RoleViewModel userRoleViewModel)
        {
            var myUser = await userManager.FindByIdAsync(userRoleViewModel.UserId);
            var myRole = await roleManager.FindByNameAsync("Admin");
            var delete = await userManager.RemoveFromRoleAsync(myUser, myRole.Name);
            if (delete.Succeeded)
            {
                return RedirectToAction("Index");
            }


            return View(userRoleViewModel);
        }

    }
}
