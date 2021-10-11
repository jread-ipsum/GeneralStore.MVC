using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Details(int id)
        {
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

        //POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _db.Transactions.Add(transaction);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
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