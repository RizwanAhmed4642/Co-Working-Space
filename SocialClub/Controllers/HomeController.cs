using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SocialClub.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        protected SocialClubEntities db = new SocialClubEntities();
        public JavaScriptSerializer sr = new JavaScriptSerializer();
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            var productimages = db.ProductImages.ToList();
            var productpackages = db.PricePlans.ToList();
            for (int i = 0; i < products.Count(); i++)
            {
                foreach (var item in productimages.ToList().Where(x=>x.ProductID == products[i].ProductID).ToList())
                {
                    item.Product = null;
                    products[i].ProductImages.Add(item);
                }
                foreach (var item in productpackages.ToList().Where(x => x.ProductID == products[i].ProductID).ToList())
                {
                    item.Product = null;
                    products[i].PricePlans.Add(item);
                }
               
            }
          return View(products);
        }

        public ActionResult Details(int id)
        {
            var products = db.Products.ToList().Where(x=>x.ProductID == id).ToList();
            var productimages = db.ProductImages.ToList();
            var productpackages = db.PricePlans.ToList();
            for (int i = 0; i < products.Count(); i++)
            {
                foreach (var item in productimages.ToList().Where(x => x.ProductID == products[i].ProductID).ToList())
                {
                    item.Product = null;
                    products[i].ProductImages.Add(item);
                }
                foreach (var item in productpackages.ToList().Where(x => x.ProductID == products[i].ProductID).ToList())
                {
                    item.Product = null;
                    products[i].PricePlans.Add(item);
                }

            }
            return View(products.FirstOrDefault());
        }
    }
}