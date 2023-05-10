using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsWeb.Helpers
{
    public static class Utilities
    {
        public static int PAGE_SIZE = 20;
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static string RemovedLinks(string html)
        {
            try
            {
                if (!string.IsNullOrEmpty(html))
                {
                    Regex r = new Regex(@"\<a href=.*?\>");
                    html = r.Replace(html, "");
                    r = new Regex(@"\</a\>");
                    html = r.Replace(html, "");
                }
                return html;
            }
            catch
            {
                return html;
            }
        }
        public static string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }
        public static string SEOurl(string url)
        {
                url = url.ToLower();
                url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
                url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ ]", "e");
                url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
                url = Regex.Replace(url, @"[íìịỉĩ]", "i");
                url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
                url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
                url = Regex.Replace(url, @"[đ]", "d");

                url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
                url = Regex.Replace(url.Trim(), @"\s+", "-");
                url = Regex.Replace(url, @"\s", "-");
                while (true)
                {
                    if (url.IndexOf("--") != -1)
                    {
                        url = url.Replace("--", "-");
                    }
                    else
                    {
                        break;
                    }
                }
                return url;
        }

        public static string GetRandomKey(int length = 5)
            {
                //chuỗi mẫu (pattern)
                string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
                Random rd = new Random();
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < length; i++)
                {
                    sb.Append(pattern[rd.Next(0, pattern.Length)]);
                }

                return sb.ToString();
            }
        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname = null)
            {
                try
                {
                    if (newname == null) newname = file.FileName;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                    string path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                    if (!System.IO.Directory.Exists(path2))
                    {
                        System.IO.Directory.CreateDirectory(path2);
                    }
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt.ToLower())) // Khác các file định nghĩa
                    {
                        return null;
                    }
                    else
                    {
                        //string fullPath = path + "//" + newname;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        return newname;
                    }
                }
                catch
                {
                    return null;
                }
            }
        public static List<string> ExtractLink(string html)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(html))
            {
                foreach (Match match in regex.Matches(html))
                {
                    list.Add(match.Groups[1].Value);
                }
            }
            return list;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {

                return false;
            }
        }
        public static string RandTime()
        {
            Random r = new Random();
            string rand = DateTime.Now.ToString().Replace("/", ":").Replace(":", "-").Replace(" ", "-").ToLower();
            rand = rand + r.Next(100, 999);
            return rand;
        }
        public static void Great_folder(string link)
        {
            string path = link;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}

