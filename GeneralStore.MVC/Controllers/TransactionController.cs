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
            return View(_db.Transaction.ToList());
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
        public ActionResult Create(CreateTransactionViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}