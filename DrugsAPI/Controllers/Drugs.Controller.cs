using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DrugsAPI.Controllers
{
    public class DrugsController : Controller
    {
        private static List<Drug> drugs = new List<Drug>
        {
            new Drug { DrugID = 1, DrugName = "Aspirin", Description = "Pain reliever", Price = 5.99m },
            new Drug { DrugID = 2, DrugName = "Ibuprofen", Description = "Anti-inflammatory", Price = 7.99m }
        };

        public IActionResult Index()
        {
            return View(drugs);
        }

        [HttpPost]
        public IActionResult Add(string DrugName, string Description, decimal Price)
        {
            var newDrug = new Drug
            {
                DrugID = drugs.Count + 1,
                DrugName = DrugName,
                Description = Description,
                Price = Price
            };

            drugs.Add(newDrug);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var drug = drugs.FirstOrDefault(d => d.DrugID == id);
            if (drug != null)
            {
                drugs.Remove(drug);
            }
            return RedirectToAction("Index");
        }
    }

    // Drug model for simplicity (usually this would go in Models folder)
    public class Drug
    {
        public int DrugID { get; set; }
        public string DrugName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
