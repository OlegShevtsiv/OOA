using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models;
using BookLibrary.ViewModels.ManageUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Filters;
using Services.DTO;
using BookLibrary.ViewModels.Sorting.States;
using BookLibrary.ViewModels.Filtration;
using BookLibrary.ViewModels.Pagination;
using BookLibrary.ViewModels.Sorting;

namespace BookLibrary.Controllers
{
    [Authorize(Roles = "user admin")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICommentService _commentService;
        private readonly IBlockedUserService _blockedUserService;

        public ManageUsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ICommentService commentService, IBlockedUserService blockedUserService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _commentService = commentService;
            _blockedUserService = blockedUserService;
        }

        //[HttpGet]
        //public IActionResult Index() => View();
        [HttpGet]
        public IActionResult Index(string searchReq = "", int page = 1, SortEnum sortOrder = SortEnum.LOGINNAME_ASC)
        {
            int pageSize = 3;

            // filtration
            List<IdentityUser> Users = new List<IdentityUser>();
            if (string.IsNullOrEmpty(searchReq))
            {
                Users = _userManager.Users.ToList();
            }
            else
            {
                List<string> keyWords = searchReq.Trim().Split(' ').ToList();
                List<IdentityUser> allUsers = _userManager.Users.ToList();
                for (int i = 0; i < keyWords.Count; i++)
                {
                    keyWords[i] = keyWords[i].ToLower().Trim();
                    foreach (var user in allUsers)
                    {
                        if (user.Email.ToLower().Contains(keyWords[i]))
                        {
                            if (!Users.Exists(b => b.Id == user.Id))
                            {
                                Users.Add(user);
                            }
                        }
                        if (user.UserName.ToLower().Contains(keyWords[i]))
                        {
                            if (!Users.Exists(b => b.Id == user.Id))
                            {
                                Users.Add(user);
                            }
                        }
                    }
                }
            }

            //sorting
            switch (sortOrder)
            {
                case SortEnum.LOGINNAME_DESC:
                    Users = Users.OrderByDescending(a => a.UserName).ToList();
                    break;
                case SortEnum.EMAIL_ASC:
                    Users = Users.OrderBy(a => a.Email).ToList();
                    break;
                case SortEnum.EMAIL_DESC:
                    Users = Users.OrderByDescending(a => a.Email).ToList();
                    break;
                default:
                    Users = Users.OrderBy(a => a.UserName).ToList();
                    break;
            }

            //pagination
            int count = Users.Count;
            var items = Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            UsersListViewModel viewModel = new UsersListViewModel
            {
                UsersFilterVM = new UsersFilterViewModel(searchReq),
                UsersPageVM = new PageViewModel(count, page, pageSize),
                UsersSortVM = new UsersSortViewModel(sortOrder),
                Users = items
            };
            return View(viewModel);
        }



        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Error");
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, LoginName = user.UserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.UserName = model.LoginName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Error");
                }
            }
            foreach (var c in _commentService.Get(new CommentFilterByOwnerId { OwnerId = id }))
            {
                _commentService.Remove(c.Id);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            // get user
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // gets user`s roles list
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            // gets user
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // gets user`s roles list
                var userRoles = await _userManager.GetRolesAsync(user);
                // gets all roles
                var allRoles = _roleManager.Roles.ToList();
                // gets roles list, that was added 
                var addedRoles = roles.Except(userRoles);
                // gets roles that was deleted
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public IActionResult BlockUser(string UserId, string EssenceId, bool isBook)
        {
            // block user
            BlockedUserDTO blockedUser = new BlockedUserDTO { UserId = UserId };
            _blockedUserService.Add(blockedUser);

            // delete all his comments
            foreach (var c in _commentService.Get(new CommentFilterByOwnerId { OwnerId = UserId }))
            {
                _commentService.Remove(c.Id);
            }

            if (isBook)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = EssenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = EssenceId });
            }
        }

        [HttpGet]
        public IActionResult UnblockUser(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return RedirectToAction("Error");
            }
            string id = _blockedUserService.GetAll().First(u => u.UserId == UserId).Id;
            _blockedUserService.Remove(id);

            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}