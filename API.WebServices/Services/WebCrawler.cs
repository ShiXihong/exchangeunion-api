using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.WebServices.Models;
using Microsoft.AspNetCore.Http;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using API.WebServices.Data;
using Microsoft.EntityFrameworkCore;

namespace API.WebServices.Services
{
    public class WebCrawler
    {
        public static void GetAndSaveBlog(ApplicationDbContext _context)
        {
            var result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://blog.exchangeunion.com/");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(result);
                IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("div.streamItem > section > div.row > div.js-trackedPost");
                bool isSave = false;

                foreach (HtmlNode nodeItem in nodes)
                {
                    Blog blog = new Blog();
                    HtmlNode node = nodeItem.QuerySelector("div.postItem a");
                    string remoteImageUrl = node.Attributes["style"].Value.Split(";")[1].Replace("&quot", "");
                    string title = node.QuerySelector("span").InnerText;

                    blog.Link = node.Attributes["href"].Value;
                    blog.ImageUrl = remoteImageUrl;
                    blog.Title = title;
                    blog.Body = nodeItem.QuerySelector("div.col > a > div > div").InnerText;

                    HtmlNode anthorNode = nodeItem.QuerySelector("div.col > div.u-clearfix > div");
                    string authorPhotoUrl = anthorNode.QuerySelector("div.postMetaInline-avatar > a > img").Attributes["src"].Value;
                    string author = anthorNode.QuerySelector("div.postMetaInline > a").InnerText;
                    string authorLink = anthorNode.QuerySelector("div.postMetaInline > a").Attributes["href"].Value;
                    string postDate = anthorNode.QuerySelector("div.postMetaInline > div.ui-caption > time").Attributes["datetime"].Value;
                    blog.Author = author;
                    blog.AuthorLink = authorLink;
                    blog.Photo = authorPhotoUrl;
                    blog.PostDate = postDate;
                    blog.CreateDate = DateTime.Now.ToString("G");

                    Blog existBlog = _context.Blog.Where(p => p.Title == blog.Title && p.PostDate == blog.PostDate).SingleOrDefault();
                    if (existBlog == null)
                    {
                        _context.Add(blog);
                        isSave = true;
                    }
                }
                if (isSave) _context.SaveChanges();
            }
        }

        public static void GetAndSaveJobs(ApplicationDbContext _context)
        {
            var result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://angel.co/exchange-union/jobs");
            request.Headers.Add("Translate: f");
            request.Headers.Add("Host:angel.co");
            request.Headers.Add("Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.Headers.Add("User-Agent:Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {

                result = reader.ReadToEnd();

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(result);
                IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("ul.job-listings > li.job-listing-role");
                _context.Database.ExecuteSqlCommand("delete from [Jobs]");

                foreach (HtmlNode nodeItem in nodes)
                {
                    IList<HtmlNode> itemNode = nodeItem.QuerySelectorAll("div.group-listings > div.s-grid");
                    foreach (HtmlNode jobItemNode in itemNode)
                    {
                        Jobs job = new Jobs();
                        job.Category = nodeItem.QuerySelector("div.group").InnerText.Replace("\n", "");
                        job.Title = jobItemNode.QuerySelector("div.listing > div.listing-title a").InnerText.Replace("\n", "");
                        job.Link = jobItemNode.QuerySelector("div.listing > div.listing-title a").Attributes["href"].Value;
                        job.Description = jobItemNode.QuerySelector("div.listing > div.listing-title div.listing-data").InnerText.Replace("\n", "").Replace("&middot;", "·").Replace("&ndash;", "-")
                            + jobItemNode.QuerySelector("div.listing > div.listing-title div.listing-data > span.listing-break-middot").InnerText.Replace("\n", "").Replace("&middot;", "·").Replace("&ndash;", "-");

                        job.CreateDate = DateTime.Now.ToString("G");

                        _context.Add(job);
                    }
                }
                _context.SaveChanges();
            }
        }

        public static List<TokenHolders> GetTokens(ApplicationDbContext _context)
        {
            List<TokenHolders> list = new List<TokenHolders>();
            var result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://etherscan.io/token/generic-tokenholders2?a=0xc324a2f6b05880503444451b8b27e6f9e63287cb&s=3000000000000000000000000000");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(result);
                IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("div#maintable > table.table > tr");
                Int32 i = 0;
                foreach (HtmlNode nodeItem in nodes)
                {
                    if (i >= 4)
                    {
                        break;
                    }

                    if (i != 0)
                    {
                        TokenHolders tokenHolders = new TokenHolders();
                        IList<HtmlNode> item = nodeItem.QuerySelectorAll("td");
                        tokenHolders.Rand = i.ToString();
                        tokenHolders.Address = item[1].QuerySelector("span > a").InnerText;
                        tokenHolders.Quantity = item[2].InnerText;
                        tokenHolders.Percentage = item[3].InnerText;
                        list.Add(tokenHolders);
                    }

                    i++;

                }
            }
            return list;
        }

        public static string DownloadImage(string imageUrl, string path)
        {
            string imageBase64String = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
                using (HttpWebResponse wresp = (HttpWebResponse)request.GetResponse())
                {
                    Stream readStream = wresp.GetResponseStream();
                    int fsLen = (int)readStream.Length;
                    byte[] heByte = new byte[fsLen];
                    int r = readStream.Read(heByte, 0, heByte.Length);
                    imageBase64String = Convert.ToBase64String(heByte);
                }
                //WebClient client = new WebClient();
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //client.DownloadFile(new Uri(imageUrl, UriKind.RelativeOrAbsolute), path);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return imageBase64String;
        }
    }
}
