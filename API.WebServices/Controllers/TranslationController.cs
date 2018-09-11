using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using API.WebServices.Data;
using API.WebServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.WebServices.Controllers
{
    [Authorize]
    public class TranslationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private string _enI18nPath = "https://raw.githubusercontent.com/ExchangeUnion/ExchangeUnion.github.io/master/i18n/en-US.json";
        private string _zhI18nPath = "https://raw.githubusercontent.com/ExchangeUnion/ExchangeUnion.github.io/master/i18n/zh-CN.json";

        public TranslationController(ApplicationDbContext context, 
            IHostingEnvironment environment)
        {
            _environment = environment;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Translation.ToListAsync());
        }

        public async Task<IActionResult> Edit(string key)
        {
            return View(await _context.Translation.Where(p => p.Key == key).ToListAsync());
        }

        [HttpGet]
        public async Task<FileStreamResult> Download(string lang)
        {
            var data = await _context.Translation.Where(p => p.Language == lang).ToListAsync();
            Hashtable output = new Hashtable();
            foreach (Translation item in data)
            {
                output[item.Key] = item.Value;
            }

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(output)));
            return new FileStreamResult(stream, "text/plain")
            {
                FileDownloadName = lang + ".json"
            };
        }

        [HttpGet]
        public Response Deploy()
        {
            Response response = new Response();
            GetJson("zh_cn", _zhI18nPath);
            GetJson("en_us", _enI18nPath);
            response.Success = true;
            response.Message = "subscribe emial send success";
            return response;
        }

        private void GetJson(string lang, string jsonUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(jsonUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                string result = reader.ReadToEnd();
                SaveJson(lang, result);
            }
        }

        private void SaveJson(string lang, string jsonResult)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

            _context.Database.ExecuteSqlCommand("DELETE FROM Translation WHERE Language = '" + lang + "'");

            foreach (var item in dict)
            {
                Translation translation = new Translation();
                translation.Key = item.Key;
                translation.Value = item.Value;
                translation.Language = lang;
                _context.Add(translation);
            }

            _context.SaveChanges();
        }
    }
}