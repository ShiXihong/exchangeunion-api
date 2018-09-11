using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using API.WebServices.Models;
using API.WebServices.Data;
using Microsoft.EntityFrameworkCore;

namespace API.WebServices.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public AttachmentController(ApplicationDbContext context, 
            IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] string pdf, [FromQuery] string json)
        {
            var files = Request.Form.Files;
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            if (pdf == "zh")
            {
                uploads = Path.Combine(_environment.WebRootPath, "pdf/zh");

            }
            else if (pdf == "en")
            {
                uploads = Path.Combine(_environment.WebRootPath, "pdf/en");
            }

            if (json == "zh" || json == "en")
            {
                uploads = Path.Combine(_environment.WebRootPath, "i18n");

            }

            var response = new ArrayList();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string newFilename = Guid.NewGuid().ToString() + ext;
                    if (pdf == "zh" || pdf == "en")
                    {
                        newFilename = "ExchangeUnion-WhitePaper" + ext;
                    }

                    

                    using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        string remoteName = "/uploads/" + newFilename;
                        response.Add(remoteName);
                    }

                    if (json == "zh" || json == "en")
                    {
                        using (FileStream fsRead = new FileStream(Path.Combine(uploads, newFilename), FileMode.Open))
                        {
                            int fsLen = (int)fsRead.Length;
                            byte[] heByte = new byte[fsLen];
                            int r = fsRead.Read(heByte, 0, heByte.Length);
                            string result = System.Text.Encoding.UTF8.GetString(heByte);
                            SaveTranslation.Save(result, json, _context);
                        }
                    }
                }
            }
            return Json(new { files = response });
        }
    }
}

class SaveTranslation
{
    public static void Save(string jsonResult, string lang, ApplicationDbContext _context)
    {
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
        
        string language = "";
        if (lang == "zh")
        {
            language = "zh_CN";
        }
        else if(lang == "en"){
            language = "en_US";
        }
        //_context.Translation.RemoveRange(_context.Translation.Where(p => p.Language == language));
        //_context.SaveChanges();
        _context.Database.ExecuteSqlCommand("DELETE FROM Translation WHERE Language = '" + language + "'");

        foreach (var item in dict)
        {
            Translation translation = new Translation();
            translation.Key = item.Key;
            translation.Value = item.Value;
            translation.Language = language;
            _context.Add(translation);
        }

        _context.SaveChanges();

    }
}