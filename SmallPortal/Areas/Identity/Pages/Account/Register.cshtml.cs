using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SmallPortal.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;

namespace SmallPortal.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpClientFactory _clientFactory;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IHttpClientFactory clientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Business Name")]
            public string BusinessName { get; set; }
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string Phone { get; set; }
            [RegularExpression("^\\d{3}-\\d{2}-\\d{4}$", 
                ErrorMessage = "Invalid TaxId Number")]
            [Display(Name = "Tax ID Number")]
            public string TaxIDNumber { get; set; }
            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }
            [Display(Name = "City")]
            public string City { get; set; }
            [Display(Name = "State")]
            public string State { get; set; }
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                /* Add code to create contact at intuit
                     * Authenticate first
                     * we'll make these secret later
                     */
                IntuitAuth auth;
                string username = "AB0y9jR3T1zZ2y4lgSTVXm8ZAz85V8NQLbWpyFtD7zn5n-test";
                string password = "jSQkNe8QNnOXwAOrUmXjU84ZL4DjJekADgZaMF4y";
                string IntuitAuthURL = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
                string accessToken;
                string IntuitTaxID = "";

                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                var content = new FormUrlEncodedContent(values);

                var authenticationString = $"{username}:{password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                var request = new HttpRequestMessage(HttpMethod.Post, IntuitAuthURL);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                request.Content = content;

                var client = _clientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);
                Debug.WriteLine(response.Headers.ToString());
                if (response.IsSuccessStatusCode)
                {
                    auth = await response.Content.ReadFromJsonAsync<IntuitAuth>();
                    accessToken = auth.Access_token;

   
                    string CreateContactURL = @"https://formfly.api.intuit.com/v1/contacts";



                    var CreateContactBody = new IntuitCreateContactBody
                    {
                        metadata = (new Metadata { id = "payer1" }),
                        businessName = Input.BusinessName,
                        firstName = Input.FirstName,
                        lastName = Input.LastName,
                        phone = Input.Phone,
                        email = Input.Email,
                        streetAddress = Input.StreetAddress,
                        city = Input.City,
                        state = Input.State,
                        postalCode = Input.PostalCode,
                        tin = Input.TaxIDNumber
                    };


                    var CreateContactRequest = new HttpRequestMessage(HttpMethod.Post, CreateContactURL);

                    CreateContactRequest.Content = new StringContent(
                        JsonSerializer.Serialize(CreateContactBody),
                        Encoding.UTF8,
                        "application/json"
                    );
                    //var CreateContactRequest = new HttpRequestMessage(HttpMethod.Post, CreateContactURL)
                    //{
                    //    Content = JsonContent.Create<IntuitCreateContactBody>(CreateContactBody)
                    //};

                    var ContactClient = _clientFactory.CreateClient();
                    ContactClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage ContactResponse = await ContactClient.SendAsync(CreateContactRequest);
                    if (ContactResponse.IsSuccessStatusCode)
                    {
                        IntuitCreateContact newContact = await ContactResponse.Content.ReadFromJsonAsync<IntuitCreateContact>();
                        IntuitTaxID = newContact.id;
                    }

                }
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    BusinessName =Input.BusinessName,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Phone = Input.Phone,
                    StreetAddress = Input.StreetAddress,
                    City = Input.City,
                    State = Input.State,
                    PostalCode = Input.PostalCode,
                    TaxIDNumber = Input.TaxIDNumber,
                    IntuitId = IntuitTaxID
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}