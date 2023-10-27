using SneakerStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SneakerStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBSneakerStoreEntities database = new DBSneakerStoreEntities();
        // GET: ShoppingCart
        public ActionResult ShowCart()
        {
            Cart _cart = Session["Cart"] as Cart;
            if (Session["Cart"] == null)
                return View("EmptyCart");
            return View(_cart);
        }
        //Action tao moi gio hang
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        //Action them product vao gio hang
        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id); //Lay san pham theo id
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int maxquan = int.Parse(form["maxValue"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            if (_quantity>0)
            {
                cart.Update_quantity(id_pro, _quantity);
            }         
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            if(cart.Items.Count() == 0)
            {
                cart = null;
                Session["Cart"]= null;
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                total_quantity_item = cart.Total_quantity();
                ViewBag.QuantityCart = total_quantity_item;
            }
            return PartialView("BagCart");
        }
        //Tao Action Checkout cho món hàng khách hàng thanh toán
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();//Bang hoa don cau san pham
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail(); //Luu bang san pham vao Chi tiet hoa don
                    _order_detail.IDOrder = _order.ID;
                    _order_detail.IDProduct = item._product.ProductID;
                    _order_detail.UnitPrice = (double)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    database.OrderDetails.Add(_order_detail);
                    //xu li cap nhat lai so luong ton trong bang Product
                    foreach (var p in database.Products.Where(s => s.ProductID == _order_detail.IDProduct))//lay ID Product dang co trong gio hang
                    {
                        var update_quan_pro = p.Quantity - item._quantity;
                        p.Quantity = update_quan_pro;
                    }
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");

            }
            catch
            {
                return Content("Error checkout .Please check information of Customer. Thanks");
            }
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}