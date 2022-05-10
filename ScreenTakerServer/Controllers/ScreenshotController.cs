using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenTakerServer.Controllers
{
    using System.Drawing;
    using System.IO;
    using System.Web;
    using Microsoft.AspNet.Identity;

    using ScreenTakerServer.Models;

    public class ScreenshotController : ApiController
    {
        string folder;

        public ScreenshotController()
        {
            this.folder = AppConstants.Folder + "\\Screenshots";
            bool exists = Directory.Exists(this.folder);
            if (!exists)
            {
                var directoryInfo = Directory.CreateDirectory(this.folder);
            }
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            var base64String = Request.Content.ReadAsStringAsync().Result;
            Image img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
            string name = DateTime.Now.ToString("s").Replace("/", "_").Replace(":", "_");
            string path = $@"{this.folder}\{name}.jpeg";
            img.Save(path);
            return this.Ok(path);
        }

       
    }
}
