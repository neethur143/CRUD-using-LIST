using Microsoft.AspNetCore.Mvc;
using RWproject1.Models;
using System.Diagnostics;

namespace RWproject1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        static List<Employee> _employees = new List<Employee>()
        {
            new Employee { Id = 1, FullName = "Niya" ,Email= "admin@gmail.com",PhoneNumber = "9061078656"},
            new Employee { Id = 2, FullName = "Neethu" ,Email= "neethu@gmail.com", PhoneNumber="5467887964" },
            new Employee { Id = 3, FullName = "Sandeep", Email = "Sandeep23@gmail.com", PhoneNumber = "54656743396" },
            new Employee { Id = 4,FullName="Athira",Email="athira@gmail.com",PhoneNumber="6735245"},
            new Employee{ Id = 5,FullName="Sneha",Email="sneha@gmail.com",PhoneNumber="354567567"}
        };
        //public IActionResult Index()
        //{
        //    return View(_employees);
        //}
        public ActionResult Index(string sortOrder)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["EmailSortParam"] = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewData["PhoneNumberSortParam"] = sortOrder == "phoneNumber_asc" ? "phoneNumber_desc" : "phoneNumber_asc";

            List<Employee> employees = _employees; // Use the list you defined earlier

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.FullName).ToList();
                    break;
                case "id_asc":
                    employees = employees.OrderBy(e => e.Id).ToList();
                    break;
                case "id_desc":
                    employees = employees.OrderByDescending(e => e.Id).ToList();
                    break;
                case "email_asc":
                    employees = employees.OrderBy(e => e.Email).ToList();
                    break;
                case "email_desc":
                    employees = employees.OrderByDescending(e => e.Email).ToList();
                    break;
                case "phoneNumber_asc":
                    employees = employees.OrderBy(e => e.PhoneNumber).ToList();
                    break;
                case "phoneNumber_desc":
                    employees = employees.OrderByDescending(e => e.PhoneNumber).ToList();
                    break;
                default:
                    employees = employees.OrderBy(e => e.FullName).ToList();
                    break;
            }

            return View(employees);
        }


        public IActionResult Edit(int id)
        {
            {
                var editemp = _employees.FirstOrDefault(e => e.Id == id);

                if (editemp == null)
                {
                    return NotFound();
                }

                return View(editemp);
            }
        }//For showing data on edit

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedEmployee"></param>
        /// <returns></returns>
        public IActionResult Update(Employee updatedEmployee)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);

            if (existingEmployee == null)
            {
                return NotFound();
            }
            existingEmployee.FullName = updatedEmployee.FullName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber;
            return RedirectToAction("Index");
        }//update Function is done here
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var emp = _employees.Find(item => item.Id == id);
            return View(emp);
        }
        public IActionResult Deleteemp(int id)
        {
            var emp = _employees.Find(item => item.Id == id);

            if (emp != null)
            {
                _employees.Remove(emp);
            }

            // Redirect to the "Index" action or any other action after deletion
            return RedirectToAction("Index");
        }

        public IActionResult Search(string SearchString)
        {
            var employees = _employees;
            if (!string.IsNullOrEmpty(SearchString))
            {
                employees = employees.Where(e =>
                    e.FullName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    e.Id.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase) //||
                    // e.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                   //e.PhoneNumber.Contains(SearchString, StringComparison.OrdinalIgnoreCase)

                ).ToList();
            }

            if (employees.Count == 0)
            {
                ViewBag.Message = "No records found.";
            }
            return View("Index", employees);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee newEmployee)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", newEmployee);
            }
            if (_employees.Any(e => e.Email == newEmployee.Email || e.PhoneNumber == newEmployee.PhoneNumber))
            {
                if (_employees.Any(e => e.Email == newEmployee.Email))
                {
                    ModelState.AddModelError("Email", "An employee with this email already exists.");
                }

                if (_employees.Any(e => e.PhoneNumber == newEmployee.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "An employee with this phone number already exists.");
                }

                return View("Add", newEmployee);
            }

            int newId = _employees.Max(e => e.Id) + 1;
            newEmployee.Id = newId;
            _employees.Add(newEmployee);
            return RedirectToAction("Index");
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}