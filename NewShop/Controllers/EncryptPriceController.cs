//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using NewShop.Models;
//using NewShop.Helpers;
//using NewShop.Repository;

//namespace NewShop.Controllers
//{
//    public class EncryptPriceController : Controller
//    {
//        private readonly ProductRepository _repository =  new ProductRepository();

//        /// <summary>
//        /// GET : Product/Edit/1
//        /// </summary>
//        public ActionResult Edit(int id = 1) {
//            // อ่านข้อมูลจากฐานข้อมูล
//            ProductModel model = _repository.GetProduct(id);

//            if (model == null) {
//                return HttpNotFound();
//            }

//            // สร้าง Signature
//            model.Signature = HmacHelper.GenerateSignature(model.ProductId, model.Price);

//            return View(model);
//        }

//        /// <summary>
//        /// POST : Product/Save
//        /// </summary>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Save(ProductModel model) {
//            //if (!ModelState.IsValid) {
//            //    return View("Edit", model);
//            //}

//            ////---------------------------------------------------
//            //// Verify Signature
//            ////---------------------------------------------------

//            //bool verify =
//            //    HmacHelper.VerifySignature(
//            //        model.ProductId,
//            //        model.Price,
//            //        model.Signature);

//            //if (!verify) {
//            //    return new HttpStatusCodeResult(
//            //        400,
//            //        "nvalid Signature");
//            //}

//            ////---------------------------------------------------
//            //// Save
//            ////---------------------------------------------------

//            ////_repository.SavePrice(
//            ////    model.ProductId,
//            ////    model.Price);

//            //TempData["Success"] =
//            //    "Save Success";

//            //return RedirectToAction(
//            //    "Edit",
//            //    new {
//            //        id = model.ProductId
//            //    });
//            decimal FinalPrice = 0;
//            if (!ModelState.IsValid) {
//                return View("Edit", model);
//            }

//            bool verify = HmacHelper.VerifySignature(
//                            model.ProductId,
//                            model.Price,
//                            model.Signature);

//            ViewBag.VerifyResult = verify;

//            ViewBag.VerifyMessage = verify
//                ? "✅ Signature Match"
//                : "❌ Signature Invalid";

//            ViewBag.ClientSignature = model.Signature;

//            if (!verify) {
//                FinalPrice = model.OldPrice;
//            }
//            else {
//                FinalPrice = model.Price;

//            }

//            ViewBag.FinalPrice = FinalPrice;

//            ViewBag.ServerSignature =
//                HmacHelper.GenerateSignature(
//                    model.ProductId,
//                    model.Price);

//            return View("Edit", model);
//        }
//        //public ActionResult Edit2(string productId) {

//            //long timestamp = PriceSignatureHelper.GetCurrentTimestamp();

//            //var model = new ProductPriceViewModel {
//            //    ProductId = "JAR1013",
//            //    Price = 500,
//            //    Timestamp = timestamp,
//            //    Signature = PriceSignatureHelper.GenerateSignature("JAR1013", 500, timestamp)
//            //};

//            //return View(model);
//        }
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult Save2(ProductPriceViewModel model) {
//        //    bool isValid = PriceSignatureHelper.VerifySignature(
//        //        model.ProductId, model.Price, model.Timestamp, model.Signature);

//        //    if (!isValid) {
//        //        // Log ไว้เพื่อ monitor (แยก log ระหว่าง "signature ผิด" กับ "timestamp หมดอายุ" ได้ถ้าต้องการ)
//        //        // Logger.Warn($"Price signature invalid/expired: ProductId={model.ProductId}, Price={model.Price}, Timestamp={model.Timestamp}");

//        //        TempData["Error"] = "ข้อมูลไม่ถูกต้องหรือหมดเวลาการยืนยัน กรุณาโหลดหน้าใหม่แล้วลองอีกครั้ง";
//        //        return RedirectToAction("Edit", new { productId = model.ProductId });
//        //    }

//        //    var product = db.Products.Find(model.ProductId);
//        //    if (product == null) return HttpNotFound();

//        //    product.Price = model.Price;
//        //    db.SaveChanges();

//        //    return RedirectToAction("Index");
//        //}
//    }
//}
