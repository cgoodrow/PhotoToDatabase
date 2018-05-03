using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoToDatabase.Controllers
{
    public class ImageUploadController : Controller
    {
        // GET: ImageUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(UploadedImage uploadimage)
        {
            HttpPostedFileBase file = Request.Files["ImageUpload"];

            uploadimage.ImageId = Guid.NewGuid();

            if (file != null && file.FileName != null && file.FileName != "")
            {
                FileInfo fi = new FileInfo(file.FileName);
                if (fi.Extension != ".jpeg" && fi.Extension != ".jpg" && fi.Extension != ".png")
                {
                    TempData["Errormsg"] = "Image File Extension is Not valid";
                    return View(uploadimage);
                }
                else
                {
                    uploadimage.ImageUpload = uploadimage.ImageId + fi.Extension;

                    file.SaveAs(Server.MapPath("~/Content/Image/" + uploadimage.ImageId + fi.Extension));

                }
            }
            using (ImageUploadsEntities1 db = new ImageUploadsEntities1())
            {
                db.UploadedImages.Add(uploadimage);
                db.SaveChanges();
                return RedirectToAction("Gallery");
            }
        }

        public ActionResult Gallery()
        {
            using (ImageUploadsEntities1 db = new ImageUploadsEntities1())
            {
                var image = db.UploadedImages.ToList();
                return View(image);
            }

        }
    }
}