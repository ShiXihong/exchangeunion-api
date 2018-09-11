using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.WebServices.Data;
using API.WebServices.Services;
using API.WebServices.Models;
using Microsoft.Extensions.Options;
using API.WebServices.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Collections;
using Microsoft.AspNetCore.Hosting;

namespace API.WebServices.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class APIController : Controller
    {

        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<EmailConfigList> _emailConfigList;
        private readonly IOptions<MailChimpConfig> _mailChimpConfig;
        private IHostingEnvironment _environment;

        private readonly DateTime _createDate = DateTime.Now;

        public APIController(
            IHostingEnvironment environment,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<EmailConfigList> emailConfigList,
            IOptions<MailChimpConfig> mailChimpConfig)
        {
            _environment = environment;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailConfigList = emailConfigList;
            _mailChimpConfig = mailChimpConfig;
        }


        [HttpGet]
        [Route("press/{id}")]
        public IActionResult Get(string id, [FromQuery] string lang)
        {
            string category = "", language = "";
            if(id == "dfg")
            {
                category = EnumUtils.Category.DigitalFinanceGroup.ToString();
            }else if (id == "xuc")
            {
                category = EnumUtils.Category.ExchangeUnion.ToString();
            }
            else if (id == "etclabs")
            {
                category = EnumUtils.Category.ETCLabs.ToString();
            }

            if (lang == "zh-CN")
            {
                language = EnumUtils.Language.Chinese.ToString();
            }
            else if (lang == "en-US")
            {
                language = EnumUtils.Language.English.ToString();
            }

            var presses = _context.Press.Where(p => p.Category == category && p.Status == EnumUtils.Status.Active.ToString() && (p.Language == EnumUtils.Language.All.ToString() || p.Language == language)).OrderByDescending(p => p.Date).ToList();
            return Json(presses);
        }

        [HttpGet]
        [Route("team/{id}")]
        public IActionResult GetTeams(string id, [FromQuery] string lang)
        {
            object team = new object();
            if (id == "xuc")
            {
                team = _context.Team.Where(p => p.Status == EnumUtils.Status.Active.ToString()).OrderBy(p => p.Order).ToList();
            }
            else if (id == "dfg")
            {
                team = _context.DFGTeam.Where(p => p.Status == EnumUtils.Status.Active.ToString()).OrderBy(p => p.Order).ToList();
            }

            return Json(team);
        }

        [HttpGet]
        [Route("portfolio")]
        public IActionResult GetPortfolio(string id)
        {
            return Json(_context.Portfolio.Where(p => p.Status == EnumUtils.Status.Active.ToString()).OrderBy(p => p.Title).ToList());
        }

        [HttpGet]
        [Route("exchange")]
        public IActionResult GetXUCExchange(string id)
        {
            return Json(_context.XUCExchange.Where(p => p.Status == EnumUtils.Status.Active.ToString()).OrderBy(p => p.Order).ToList());
        }

        [HttpGet]
        [Route("blog")]
        public IActionResult GetBlog()
        {
            var blogList = _context.Blog.ToList();
            if(blogList.Count <= 0)
            {
                WebCrawler.GetAndSaveBlog(_context);
                blogList = _context.Blog.ToList();
            }

            return Json(blogList);
        }

        [HttpGet]
        [Route("jobs")]
        public IActionResult GetJobs()
        {
            var jobsList = _context.Jobs.ToList();
            if (jobsList.Count <= 0)
            {
                WebCrawler.GetAndSaveJobs(_context);
                jobsList = _context.Jobs.ToList();
            }

            return Json(jobsList);
        }

        [HttpGet]
        [Route("tokens")]
        public IActionResult GetToken()
        {
            List<TokenHolders> list =  WebCrawler.GetTokens(_context);

            return Json(list);
        }

        [HttpGet]
        [Route("transparency")]
        public IActionResult GetTransparency()
        {
            List<Transparency> list = _context.Transparency.Where(p => p.Active == "Active").ToList();

            return Json(list);
        }

        [HttpGet]
        [Route("i18n")]
        public async Task<JsonResult> GetI18n([FromQuery] string lang)
        {
            var data = await _context.Translation.Where(p => p.Language == lang).OrderBy(p => p.Key).ToListAsync();
            Hashtable output = new Hashtable();
            foreach (Translation item in data)
            {
                output[item.Key] = item.Value;
            }
            return Json(output);
        }


        [HttpPost]
        [Route("subscribe")]
        public async Task<Response> PostSubscribeEmail([FromQuery] string lang, [FromBody] Email email)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "api error";
                return response;
            }

            email.IsActive = false;
            email.SendDate = DateTime.Now;

            _context.Email.Add(email);
            await _context.SaveChangesAsync();

            var emailServices = new SendEmail(_emailConfigList, _mailChimpConfig, email.Category);

            string emailBody, emailSubject = Constant.SUBSCRIBE_EMAIL_SUBJECT_ENUS;

            if(lang == "zh-CN")
            {
                emailSubject = Constant.SUBSCRIBE_EMAIL_SUBJECT_ZHCN;
            }

            if(email.Category == EnumUtils.Category.ExchangeUnion.ToString())
            {
                emailBody = String.Format(lang == "zh-CN" ? Constant.XUC_SUBSCRIBE_EMAIL_TEMPLETE_ZHCN : Constant.XUC_SUBSCRIBE_EMAIL_TEMPLETE, Convert.ToBase64String(Encoding.Default.GetBytes(email.EmailAddress)));
            }
            else
            {
                emailBody = String.Format(Constant.DFG_SUBSCRIBE_EMAIL_TEMPLETE, Convert.ToBase64String(Encoding.Default.GetBytes(email.EmailAddress)));
            }

            Task<EmailResult> sendEmailSuccess = emailServices.SendEmailAsync(email.EmailAddress, emailSubject, emailBody);

            Task.WaitAll(sendEmailSuccess);

            response.Success = sendEmailSuccess.Result.Success;
            response.Message = sendEmailSuccess.Result.Message;
            return response;
        }

        [HttpPost]
        [Route("subscribe/success")]
        public async Task<IActionResult> PostSubscribeSuccess([FromBody] Subscribe subscribe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            subscribe.SubscribeDate = _createDate;      
            subscribe.CreateDate = _createDate;
            subscribe.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            byte[] bytes = Convert.FromBase64String(subscribe.Email);
            subscribe.Email = Encoding.Default.GetString(bytes);


            if (!SubscribeExists(subscribe.Email))
            {
                _context.Subscribe.Add(subscribe);
            }

            await _context.SaveChangesAsync();

            var emailServices = new SendEmail(_emailConfigList, _mailChimpConfig, subscribe.Category);

            /*string emailBody, email;

            if (subscribe.Category == EnumUtils.Category.ExchangeUnion.ToString())
            {
                emailBody = String.Format(Constant.XUC_SUBSCRIBE_SUCCESS_TEMPLETE, subscribe.Email, DateTime.Now.ToShortDateString());
                email = _emailConfigList.Value.XUCEmailConfig.Form;
            }
            else
            {
                emailBody = String.Format(Constant.DFG_SUBSCRIBE_SUCCESS_TEMPLETE, subscribe.Email, DateTime.Now.ToShortDateString());
                email = _emailConfigList.Value.DFGEmailConfig.Form;
            }

            Task<bool> sendEmailSuccess = emailServices.SendEmailAsync(email, email + "  subscribe complete", emailBody);
            Task.WaitAll(sendEmailSuccess);*/

            Task<bool> testMailChimp = emailServices.AddMemberToMailChimp(subscribe.Email);
            Task.WaitAll(testMailChimp);

            return CreatedAtAction("GetSubscribe", new { id = subscribe.Id }, subscribe);
        }

        [HttpPost]
        [Route("contact")]
        public async Task<Response> PostConcatEmail([FromQuery] string lang, [FromBody] Contact contact)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "api error";
                return response;
            }

            contact.CreateDate = _createDate;

            _context.Contact.Add(contact);
            await _context.SaveChangesAsync();

            string emailBody, message = contact.Message.Replace("\n", "<br />"), emailSubject = Constant.CONTACT_EMAIL_SUBJECT_ENUS;

            if (lang == "zh-CN")
            {
                emailSubject = Constant.CONTACT_EMAIL_SUBJECT_ZHCN;
            }

            if (contact.Category == EnumUtils.Category.ExchangeUnion.ToString())
            {
                emailBody = String.Format(lang == "zh-CN" ? Constant.XUC_COCTACT_EMAIL_TEMPLETE_ZHCN : Constant.XUC_COCTACT_EMAIL_TEMPLETE, contact.Name, message);
            }
            else
            {
                emailBody = String.Format(Constant.DFG_COCTACT_EMAIL_TEMPLETE, contact.Name, message);
            }

            var emailServices = new SendEmail(_emailConfigList, _mailChimpConfig, contact.Category);

            Task<EmailResult> sendEmailSuccess = emailServices.SendEmailAsync(contact.Email, emailSubject, emailBody);

            Task.WaitAll(sendEmailSuccess);

            response.Success = sendEmailSuccess.Result.Success;
            response.Message = sendEmailSuccess.Result.Message;
            return response;
        }

        private bool SubscribeExists(string email)
        {
            return _context.Subscribe.Any(e => e.Email == email);
        }

        [HttpGet]
        [Route("events")]
        public IActionResult GetEvents()
        {
            return Json(_context.Events.Where(p => p.Status == EnumUtils.Status.Active.ToString()).OrderByDescending(p => p.Date).ToList());
        }

        [HttpGet]
        [Route("education")]
        public IActionResult GetEducations()
        {
            return Json(_context.Education.Where(p => p.Status == EnumUtils.Status.Active.ToString()).OrderByDescending(p => p.Date).ToList());
        }
    }
}
