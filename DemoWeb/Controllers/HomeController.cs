using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DemoWeb.Models;
using System.IO;


namespace DemoWeb.Controllers
{
    public class HomeController : Controller
    {
        private demoDBEntities db = new demoDBEntities();
        public ActionResult Index(string searchString, int categoryID = 0)
        {
            var listcate = db.categories.Where(c => c.status == 1);
            ViewBag.categoryID = new SelectList(listcate, "id", "name");
            var product = db.products.OrderBy(f => f.category.name).Include(f => f.category).Where(f => f.category.status == 1);
            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(f => f.name.Contains(searchString));
            }
            if (categoryID != 0)
            {
                product = product.Where(x => x.categoryid == categoryID);
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\food.jpg";
            if (System.IO.File.Exists(filepath))
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                byte[] filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                foreach (var item in product)
                {
                    item.url_img = "data:image/png;base64," + Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
                }
            }

            var cates = db.categories.Where(c => c.status == 1);
            if (categoryID != 0)
            {
                cates = cates.Where(x => x.id == categoryID);
            }
            var viewModel = new MyModel();
            viewModel.ListCate = cates.ToList();
            viewModel.ListProduct = product.ToList();

            return View(viewModel);
        }

        public ActionResult ProductDetail(int id)
        {
            var product = db.products.Find(id);
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\food.jpg";
            if (System.IO.File.Exists(filepath))
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                byte[] filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                product.url_img = "data:image/png;base64," + Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
            }
            return View(product);
        }
    }
}
