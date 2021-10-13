using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Transaction
        public ActionResult Index()
        {
            return View(_db.Transactions.ToList());
        }

        //GET: Transaction/{id}
        public ActionResult Details(int? id)
        {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No Transaction Id Present.");

            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null)
                return HttpNotFound();

            return View(transaction);
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            var viewModel = new CreateTransactionViewModel();

            viewModel.Customers = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerId.ToString()
            });

            viewModel.Products = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            return View(viewModel);
        }

        // ViewBag use for same method
        /*public  ActionResult Create()
        {
            ViewBag.CustomerItems = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerId.ToString()
            });
            ViewBag.ProductItems = new SelectList(_db.Products, "ProductId", "Name");
            return View();
        }*/

        //POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTransactionViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                Transaction transaction = new Transaction()
                {
                    CustomerId = viewModel.CustomerId,
                    ProductId = viewModel.ProductId
                };

                _db.Transactions.Add(transaction);
                if (_db.SaveChanges() == 1)
                {
                    return RedirectToAction("Index");
                }
                ViewData["ErrorMessage"] = "Couldn't save your transaction. Please try again later";
            }
            ViewData["ErrorMessage"] = "Model view was invalid.";

            viewModel.Customers = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerId.ToString()
            });

            viewModel.Products = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            return View(viewModel);
        }

        //GET: Transaction/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null)
                return HttpNotFound();

            return View(transaction);
        }

        //POST: Transaction/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //GET: Transaction/Edit/{id}
        public ActionResult Edit(int? id)
        {
            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null)
                return HttpNotFound();

            ViewData["Customers"] = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerId.ToString()
            });

            ViewData["Products"] = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            return View(transaction);
        }

        //POST: Transaction/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            ViewData["Customers"] = _db.Customers.Select(customer => new SelectListItem
            {
                Text = customer.FirstName + " " + customer.LastName,
                Value = customer.CustomerId.ToString()
            });

            ViewData["Products"] = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });

            if(ModelState.IsValid)
            {
                _db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transaction);
        }
    }
}