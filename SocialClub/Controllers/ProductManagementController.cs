using SocialClub.Configuration;
using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SocialClub.Controllers
{
    [Authorize]
    public class ProductManagementController : Controller
    {
        private readonly AdminActivity_Logs Addlog = new AdminActivity_Logs();
        protected SocialClubEntities db = new SocialClubEntities();
        public JavaScriptSerializer sr = new JavaScriptSerializer();

        public ActionResult Product()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult ListProduct()
        {
            try
            {
                var obj = db.Products.ToList();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "ProductManagement");
            }

        }

        public ActionResult GetProductImages()
        {
            try
            {
                var obj = db.Products.ToList();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "ProductManagement");
            }
        }

        public ActionResult ImageGallery(int id)
        {
            try
            {
                var obj = db.ProductImages.AsQueryable().Where(x => x.ProductID == id).ToList();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "ProductManagement");
            }
        }
        public ActionResult ImagesCounterByProduct(int id)
        {
            try
            {
                int obj = db.ProductImages.ToList().Where(x => x.ProductID == id).ToList().Count();
                int responseObject = obj;
                var jsonResult = Json(responseObject, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("GetProductImages", "ProductManagement");
            }
        }

        public ActionResult UpdateImageGallery(ProductImage data, HttpPostedFileBase titleimg)
        {
            try
            {
                //var key = Session["key"].ToString();
                ProductImage obj = new ProductImage();
                if (titleimg != null)
                {
                    var input = titleimg.InputStream;
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    data.Image = byteData;
                }
                obj = data;
                if (data.ProductImageID > 0)
                {
                    obj.ProductImageID = Convert.ToInt32(data.ProductImageID);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
                    return jsonResult;
                    // TempData["response"] = "Update Successfully";
                    //return RedirectToAction("ProductManagement", "ImageGallery", new { id= data.ProductID });
                }
                else
                {
                    try
                    {
                        var createdobject = db.ProductImages.Add(obj);
                        db.SaveChanges();
                        var jsonResult = Json(createdobject, JsonRequestBehavior.AllowGet);
                        return jsonResult;
                    }
                    catch (Exception ex)
                    {

                        TempData["response"] = "Unable To Add Image " + ex.Message;
                        return RedirectToAction("ProductManagement", "GetProductImages");

                    }

                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable To Add Image " + ex.Message;
                return RedirectToAction("ProductManagement", "GetProductImages");

            }
        }

        public ActionResult DeleteImage(string id = "")
        {
            try
            {
                var FoundREcord = db.ProductImages.Find(Convert.ToInt32(id));
                if(FoundREcord != null)
                {
                    var PID = FoundREcord.ProductID;
                    db.ProductImages.Remove(FoundREcord);
                    db.SaveChanges();
                    //TempData["response"] = "Deleted Image Successfully";
                    var jsonResult = Json("true", JsonRequestBehavior.AllowGet);
                    return jsonResult; //RedirectToAction("ImageGallery", new { id = PID });

                }
                else
                {
                    ///TempData["response"] = "Unable To Delete Image";
                    var jsonResult = Json("false", JsonRequestBehavior.AllowGet);
                    return jsonResult;// RedirectToAction("ImageGallery", new { id = id });

                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ProductManagement", "GetProductImages");

            }

        }

        public ActionResult GetImages(int id)
        {
            try
            {
                var obj = db.ProductImages.AsQueryable().Where(x => x.ProductID == id).ToList();
                foreach (var item in obj)
                {
                    item.ImagePath = Convert.ToBase64String(item.Image);

                }
                var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListProduct", "ProductManagement");
            }
        }
        [HttpPost]
        public ActionResult AddProduct(Product data, HttpPostedFileBase titleimage)
        {
            try
            {
                var alldata = db.Products.ToList().Exists(x => x.ProductName.Equals(data.ProductName, StringComparison.CurrentCultureIgnoreCase));
                if (alldata == false)
                {
                    if (ModelState.IsValid)
                    {

                        data.CreatedDate = DateTime.Now;
                        data.CreatedBy = User.Identity.Name;
                        if (titleimage != null)
                        {
                            var input = titleimage.InputStream;
                            byte[] byteData = null, buffer = new byte[input.Length];
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int read;
                                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, read);
                                }
                                byteData = ms.ToArray();
                            }
                            data.Image = byteData;
                        }
                        db.Products.Add(data);
                        Addlog.AddLog("Save", "Save Product In Application");

                        db.SaveChanges();

                        TempData["response"] = "Product Save Successfully.";
                        return RedirectToAction("ListProduct", "ProductManagement");
                    }
                    else
                    {
                        TempData["response"] = "Invalid Model " + ModelState.Values;
                        return RedirectToAction("Product", "ProductManagement");
                    }
                }
                else
                {
                    TempData["response"] = "ProductName Already Exists !";
                    return RedirectToAction("Product", "ProductManagement");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "Product");

            }
        }

        public ActionResult UpdateProduct(int id)
        {
            try
            {
                var data = db.Products.Find(id);
                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    TempData["response"] = "Product Not Found !";
                    return RedirectToAction("ListProduct", "ProductManagement");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "ProductManagement");
            }
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product data, int id, HttpPostedFileBase titleimage)
        {
            try
            {
                var alldata = db.Products.ToList().Exists(x => x.ProductName.Equals(data.ProductName, StringComparison.CurrentCultureIgnoreCase) && x.ProductID != id);
                if (alldata == false)
                {
                    var record = db.Products.Find(id);
                    if (record != null)
                    {
                        record.ProductName = data.ProductName;
                        record.Description = data.Description;
                        record.Status = data.Status;
                        if (titleimage != null)
                        {
                            var input = titleimage.InputStream;
                            byte[] byteData = null, buffer = new byte[input.Length];
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int read;
                                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, read);
                                }
                                byteData = ms.ToArray();
                            }
                            data.Image = byteData;
                        }
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;
                        Addlog.AddLog("Update", "Update Item In Product");
                        db.SaveChanges();

                        TempData["response"] = "product Updated Successfully !";
                        return RedirectToAction("ListProduct", "ProductManagement");
                    }
                    else
                    {
                        TempData["response"] = "Product Update Failed !";
                        return RedirectToAction("ListProduct", "ProductManagement");
                    }
                }
                else
                {
                    TempData["response"] = "Same Record Already Exists !";
                    return RedirectToAction("ListProduct", "ProductManagement");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "ProductManagement");
            }
        }

        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var data = db.Products.Find(id);
                if (data != null)
                {
                    db.Products.Remove(data);
                    db.SaveChanges();
                    Addlog.AddLog("Delete", "Delete Item in Product");
                    TempData["response"] = "Product Deleted Successfully !";
                    return RedirectToAction("ListProduct", "ProductManagement");
                }
                else
                {
                    TempData["response"] = "Product Deletion Failed !";
                    return RedirectToAction("ListProduct", "ProductManagement");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListProduct", "ProductManagement");
            }
        }

        public ActionResult ChooseProduct()
        {
            try
            {
                var obj = db.Products.ToList();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Product", "ProductManagement");
            }

        }
        public ActionResult AddPackage(int id)
        {
            ViewBag.ProductID = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddPackage(PricePlan data)
        {
            try
            {
                data.CreatedDate = DateTime.Now;
                data.Status = true;
                data.CreatedBy = User.Identity.Name;
                db.PricePlans.Add(data);
                db.SaveChanges();
                var jsonResult = Json("true", JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListProduct", "ProductManagement");
            }
        }

        public ActionResult ListPackage()
        {
            try
            {
                var obj = db.PricePlans.ToList();
                var products = db.Products.ToList();
                for (int i = 0; i < obj.Count(); i++)
                {
                    foreach (var item in products.ToList().Where(x=>x.ProductID == obj[i].ProductID).ToList())
                    {
                        obj[i].ProductName = item.ProductName;
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListProduct", "ProductManagement");
            }

        }
        public ActionResult DeletePackage(int id)
        {
            try
            {
                var data = db.PricePlans.Find(id);
                if (data != null)
                {
                    db.PricePlans.Remove(data);
                    db.SaveChanges();
                    Addlog.AddLog("Delete", "Delete Package in Product");
                    TempData["response"] = "Package Deleted Successfully !";
                    return RedirectToAction("ListPackage", "ProductManagement");
                }
                else
                {
                    TempData["response"] = "Package Deletion Failed !";
                    return RedirectToAction("ListPackage", "ProductManagement");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListPackage", "ProductManagement");
            }
        }

        public ActionResult ProductPackagePlan(int id)
        {
            try
            {
                var obj = db.PricePlans.ToList().Where(x=>x.ProductID == id).ToList();
                var products = db.Products.ToList();
                for (int i = 0; i < obj.Count(); i++)
                {
                    foreach (var item in products.ToList().Where(x => x.ProductID == obj[i].ProductID).ToList())
                    {
                        obj[i].ProductName = item.ProductName;
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ChooseProduct", "ProductManagement");
            }
        }
    }
}