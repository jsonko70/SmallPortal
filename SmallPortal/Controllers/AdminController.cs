using SmallPortal.Models;
using SmallPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmallPortal.ViewsModel;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using SmallPortal.Data;

namespace SmallPortal.Controllers
{
    [Authorize(Policy = "AdminRolePolicy")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdminController> logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;


        public UserManager<ApplicationUser> UserManager { get; }

        public AdminController(RoleManager<IdentityRole> roleManager,
                        UserManager<ApplicationUser> userManager,
                                ILogger<AdminController> logger,
                                IHttpClientFactory clientFactory,
                                ApplicationDbContext context
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
            _context = context;
            _clientFactory = clientFactory;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            result = await userManager.AddClaimsAsync(user,
                model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId });

        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            // foreach (var role in roleManager.Roles)
            foreach (var role in roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListRoles");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Exception Occured : {ex}");

                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " +
                        $"in this role. If you want to delete this role, please remove the users from " +
                        $"the role and then try to delete.";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                //City = user.City,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                //user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult List1099s()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            // Retrieve all the Users
            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
        public IActionResult Recipient1099Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recipient1099Create([Bind("Email,Password,ConfirmPassword,BusinessName,FirstName,LastName,Phone,TaxIDNumber,StreetAddress,City,State,PostalCode,box1,box2,box3,box4,box5,box6,box7,box8,box9,box10,box12,box13,box14,box15,box16,box17")] Recipient1099InputModel recipient1099InputModel)
        {
            if (ModelState.IsValid)
            {
                // Getting payer info to submit in 1099MISC 
                var intuitPayerId = (from u in _context.Users
                                     where u.UserName == User.Identity.Name
                                     select u).SingleOrDefault();

                IntuitAuth auth;
                string username = "AB0y9jR3T1zZ2y4lgSTVXm8ZAz85V8NQLbWpyFtD7zn5n-test";
                string password = "jSQkNe8QNnOXwAOrUmXjU84ZL4DjJekADgZaMF4y";
                string IntuiAuthURL = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
                string accessToken;
                string intuitRecipientId = "";

                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                var content = new FormUrlEncodedContent(values);


                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                var request = new HttpRequestMessage(HttpMethod.Post, IntuiAuthURL);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                request.Content = content;

                var client = _clientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    auth = await response.Content.ReadFromJsonAsync<IntuitAuth>();
                    accessToken = auth.Access_token;


                    string CreateContactURL = @"https://formfly.api.intuit.com/v1/contacts";

                    var CreateContactBody = new IntuitCreateContactBody
                    {
                        //metadata = (new Metadata { id = "payer1" }),
                        businessName = recipient1099InputModel.BusinessName,
                        firstName = recipient1099InputModel.FirstName,
                        lastName = recipient1099InputModel.LastName,
                        streetAddress = recipient1099InputModel.StreetAddress,
                        city = recipient1099InputModel.City,
                        state = recipient1099InputModel.State,
                        postalCode = recipient1099InputModel.PostalCode,
                        phone = recipient1099InputModel.Phone,
                        email = recipient1099InputModel.Email,
                        tin = recipient1099InputModel.TaxIDNumber
                    };

                    var CreateContactRequest = new HttpRequestMessage(HttpMethod.Post, CreateContactURL);

                    CreateContactRequest.Content = new StringContent(
                        JsonSerializer.Serialize(CreateContactBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var ContactClient = _clientFactory.CreateClient();
                    ContactClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage ContactResponse = await ContactClient.SendAsync(CreateContactRequest);
                    if (ContactResponse.IsSuccessStatusCode)
                    {
                        IntuitCreateContact newContact = await ContactResponse.Content.ReadFromJsonAsync<IntuitCreateContact>();
                        intuitRecipientId = newContact.id;
                    }

                    var New1099 = new Recipient1099
                    {
                        //actions = new SmallPortal.Models.Actions { submit = true },
                        deliveryOptions = (new DeliveryOptions { mail = true }),
                        payer = (new Payer
                        {
                            id = intuitPayerId.IntuitId,
                            firstName = intuitPayerId.FirstName,
                            lastName = intuitPayerId.LastName,
                            streetAddress = intuitPayerId.StreetAddress,
                            city = intuitPayerId.City,
                            state = intuitPayerId.State,
                            postalCode = intuitPayerId.PostalCode,
                            phone = intuitPayerId.Phone,
                            email = intuitPayerId.Email,
                            tin = intuitPayerId.TaxIDNumber,
                            validationStatus = "valid"
                        }),
                        recipient = (new Recipient
                        {
                            businessName = recipient1099InputModel.BusinessName,
                            streetAddress = recipient1099InputModel.StreetAddress,
                            city = recipient1099InputModel.City,
                            state = recipient1099InputModel.State,
                            postalCode = recipient1099InputModel.PostalCode,
                            phone = recipient1099InputModel.Phone,
                            email = recipient1099InputModel.Email,
                            tin = recipient1099InputModel.TaxIDNumber
                        }),
                        boxValues = (new BoxValues
                        {
                            box1 = recipient1099InputModel.box1,
                            box2 = recipient1099InputModel.box2,
                            box3 = recipient1099InputModel.box3,
                            box4 = recipient1099InputModel.box4,
                            box5 = recipient1099InputModel.box5,
                            box6 = recipient1099InputModel.box6,
                            box7 = recipient1099InputModel.box7,
                            box8 = recipient1099InputModel.box8,
                            box9 = recipient1099InputModel.box9,
                            box10 = recipient1099InputModel.box10,
                            box12 = recipient1099InputModel.box12,
                            box13 = recipient1099InputModel.box13,
                            box14 = recipient1099InputModel.box14,
                            box15 = recipient1099InputModel.box15,
                            box16 = recipient1099InputModel.box16,
                            box17 = recipient1099InputModel.box17
                        }),
                    };

                    string CreateRecipient1099URL = "https://formfly.api.intuit.com/v2/forms/2020/1099-misc";

                    var CreateRecipient1099 = new HttpRequestMessage(HttpMethod.Post, CreateRecipient1099URL);

                    CreateRecipient1099.Content = new StringContent(
                        JsonSerializer.Serialize(New1099),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var Recipient1099Client = _clientFactory.CreateClient();
                    Recipient1099Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage Recipient1099Response = await Recipient1099Client.SendAsync(CreateRecipient1099);
                    if (Recipient1099Response.IsSuccessStatusCode)
                    {
                        IntuitCreate1099 new1099 = await Recipient1099Response.Content.ReadFromJsonAsync<IntuitCreate1099>();
                        string formId = new1099.id;
                    }
                    else
                    {
                        IntuitError newError = await Recipient1099Response.Content.ReadFromJsonAsync<IntuitError>();
                        Console.WriteLine(newError);
                    }
                }
                //    _context.Add(recipient1099InputModel);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipient1099InputModel);
        }
        public IActionResult Recipient1099NecCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recipient1099NecCreate([Bind("Email,Password,ConfirmPassword,BusinessName,FirstName,LastName,Phone,TaxIDNumber,StreetAddress,City,State,PostalCode,box1,box2,box3,box4,box5,box6,box7")] Recipient1099NecInputModel recipient1099NecInputModel)
        {
            if (ModelState.IsValid)
            {
                // Getting payer info to submit in 1099NEC 
                var intuitPayerId = (from u in _context.Users
                                     where u.UserName == User.Identity.Name
                                     select u).SingleOrDefault();

                IntuitAuth auth;
                string username = "AB0y9jR3T1zZ2y4lgSTVXm8ZAz85V8NQLbWpyFtD7zn5n-test";
                string password = "jSQkNe8QNnOXwAOrUmXjU84ZL4DjJekADgZaMF4y";
                string IntuiAuthURL = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
                string accessToken;
                string intuitRecipientId = "";

                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                var content = new FormUrlEncodedContent(values);


                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                var request = new HttpRequestMessage(HttpMethod.Post, IntuiAuthURL);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                request.Content = content;

                var client = _clientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    auth = await response.Content.ReadFromJsonAsync<IntuitAuth>();
                    accessToken = auth.Access_token;


                    string CreateContactURL = @"https://formfly.api.intuit.com/v1/contacts";

                    var CreateContactBody = new IntuitCreateContactBody
                    {
                        //metadata = (new Metadata { id = "payer1" }),
                        businessName = recipient1099NecInputModel.BusinessName,
                        firstName = recipient1099NecInputModel.FirstName,
                        lastName = recipient1099NecInputModel.LastName,
                        streetAddress = recipient1099NecInputModel.StreetAddress,
                        city = recipient1099NecInputModel.City,
                        state = recipient1099NecInputModel.State,
                        postalCode = recipient1099NecInputModel.PostalCode,
                        phone = recipient1099NecInputModel.Phone,
                        email = recipient1099NecInputModel.Email,
                        tin = recipient1099NecInputModel.TaxIDNumber

                    };

                    var CreateContactRequest = new HttpRequestMessage(HttpMethod.Post, CreateContactURL);

                    CreateContactRequest.Content = new StringContent(
                        JsonSerializer.Serialize(CreateContactBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var ContactClient = _clientFactory.CreateClient();
                    ContactClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage ContactResponse = await ContactClient.SendAsync(CreateContactRequest);
                    if (ContactResponse.IsSuccessStatusCode)
                    {
                        IntuitCreateContact newContact = await ContactResponse.Content.ReadFromJsonAsync<IntuitCreateContact>();
                        intuitRecipientId = newContact.id;
                    }

                    var New1099Nec = new Recipient1099Nec
                    {
                        //actions = new SmallPortal.Models.Actions { submit = true },
                        deliveryOptions = (new DeliveryOptions { mail = true }),
                        payer = (new Payer
                        {
                            id = intuitPayerId.IntuitId,
                            firstName = intuitPayerId.FirstName,
                            lastName = intuitPayerId.LastName,
                            streetAddress = intuitPayerId.StreetAddress,
                            city = intuitPayerId.City,
                            state = intuitPayerId.State,
                            postalCode = intuitPayerId.PostalCode,
                            phone = intuitPayerId.Phone,
                            email = intuitPayerId.Email,
                            tin = intuitPayerId.TaxIDNumber,
                            validationStatus = "valid"
                        }),
                        recipient = (new Recipient
                        {
                            businessName = recipient1099NecInputModel.BusinessName,
                            streetAddress = recipient1099NecInputModel.StreetAddress,
                            city = recipient1099NecInputModel.City,
                            state = recipient1099NecInputModel.State,
                            postalCode = recipient1099NecInputModel.PostalCode,
                            phone = recipient1099NecInputModel.Phone,
                            email = recipient1099NecInputModel.Email,
                            tin = recipient1099NecInputModel.TaxIDNumber
                        }),
                        boxValues = (new BoxValuesNec
                        {
                            box1 = recipient1099NecInputModel.box1,
                            box4 = recipient1099NecInputModel.box4,
                            box5 = recipient1099NecInputModel.box5,
                            box6 = recipient1099NecInputModel.box6,
                            box7 = recipient1099NecInputModel.box7,

                        })
                    };

                    string CreateRecipient1099NecURL = "https://formfly.api.intuit.com/v2/forms/2020/1099-nec";

                    var CreateRecipient1099Nec = new HttpRequestMessage(HttpMethod.Post, CreateRecipient1099NecURL);

                    CreateRecipient1099Nec.Content = new StringContent(
                        JsonSerializer.Serialize(New1099Nec),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var Recipient1099NecClient = _clientFactory.CreateClient();
                    Recipient1099NecClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage Recipient1099Response = await Recipient1099NecClient.SendAsync(CreateRecipient1099Nec);
                    if (Recipient1099Response.IsSuccessStatusCode)
                    {
                        IntuitCreate1099 new1099Nec = await Recipient1099Response.Content.ReadFromJsonAsync<IntuitCreate1099>();
                        string formId = new1099Nec.id;
                    }
                    else
                    {
                        IntuitError newError = await Recipient1099Response.Content.ReadFromJsonAsync<IntuitError>();
                        Console.WriteLine(newError);
                    }
                }
                //    _context.Add(recipient1099InputModel);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipient1099NecInputModel);

        }
    }
}