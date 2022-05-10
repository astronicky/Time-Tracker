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

    using Microsoft.AspNet.Identity;

    using ScreenTakerServer.Models;

    public class MouseMoveCaptureController : ApiController
    {
        private string filePath;

        public MouseMoveCaptureController()
        {
            string today = "MouseStatus\\MouseStatus " + DateTime.Now.Date.ToString("dd MMMM yyyy");
            bool existsDir = Directory.Exists(AppConstants.Folder + "\\MouseStatus");
            if (!existsDir)
            {
                var directoryInfo = Directory.CreateDirectory(AppConstants.Folder + "\\MouseStatus");
            }
            this.filePath = $@"{AppConstants.Folder}\{today}.txt";
            bool exists = File.Exists(this.filePath);
            if (!exists)
            {
                //File.Create(this.filePath);
            }
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            string data = this.Request.Content.ReadAsStringAsync().Result;
            var writer = new StreamWriter(this.filePath, true);
            writer.WriteLine($"{DateTime.Now} : \t {data}");
            writer.Close();
            return this.Ok();
        }

       
    }
}
