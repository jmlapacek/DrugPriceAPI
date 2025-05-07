using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DrugsAPI.Controllers
{
    public class DrugsController : Controller
    {
        // In-memory list of drugs (mock database with dummy data)
        private static List<Medication> drugs = new List<Medication>
        {
            new Medication { DrugID = 1, DrugName = "Aspirin", Description = "Pain reliever", Price = 5.99m },
            new Medication { DrugID = 2, DrugName = "Ibuprofen", Description = "Anti-inflammatory", Price = 7.99m },
            new Medication { DrugID = 3, DrugName = "Amoxicillin", Description = "Antibiotic", Price = 12.49m },
            new Medication { DrugID = 4, DrugName = "Metformin", Description = "Diabetes management", Price = 15.99m },
            new Medication { DrugID = 5, DrugName = "Lisinopril", Description = "Blood pressure", Price = 8.99m },
            new Medication { DrugID = 6, DrugName = "Omeprazole", Description = "Acid reflux", Price = 11.49m }
        };

        // Display the list of all drugs (Read)
        public IActionResult Index()
        {
            return View(drugs);
        }

        // Add a new drug (Create)
        [HttpPost]
        public IActionResult Add(string DrugName, string Description, decimal Price)
        {
            var newDrug = new Medication
            {
                DrugID = drugs.Count + 1,
                DrugName = DrugName,
                Description = Description,
                Price = Price
            };

            drugs.Add(newDrug);
            return RedirectToAction("Index");
        }

        // Display the details of a specific drug (Read)
        [HttpGet]
        public IActionResult Details(int id)
        {
            var drug = drugs.FirstOrDefault(d => d.DrugID == id);
            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        // Display the delete confirmation page (Get)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var drug = drugs.FirstOrDefault(d => d.DrugID == id);
            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        // Confirm and perform the delete (Post)
        [HttpPost]
        public IActionResult DeleteConfirmed(int DrugID)
        {
            var drug = drugs.FirstOrDefault(d => d.DrugID == DrugID);
            if (drug != null)
            {
                drugs.Remove(drug);
            }

            return RedirectToAction("Index");
        }

        // Display Medicare Prices (API)
        [HttpGet]
        public async Task<IActionResult> MedicarePrices()
        {
            string apiUrl = "https://data.cms.gov/api/your-endpoint-here"; // Replace with actual CMS API URL
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var prices = JsonConvert.DeserializeObject<List<MedicareDrugPrice>>(json);
                    return View(prices);
                }
            }

            return Content("Unable to retrieve Medicare drug prices at this time.");
        }

        // Medication model (renamed to avoid conflict)
        public class Medication
        {
            public int DrugID { get; set; }
            public string DrugName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        // Model for API Data
        public class MedicareDrugPrice
        {
            public string DrugName { get; set; }
            public decimal AveragePrice { get; set; }
            public string Manufacturer { get; set; }
        }
    }
}
