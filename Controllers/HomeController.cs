using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        // NOTLAR : 
        // Ýlk önce appsettings.json dosyasýnda ConnectionString ifadesini yazdýk ve default kýsmýnda da sql baðlantý kýsmýný yazdýk. ( ordan bakarsýn) 

        //   "ConnectionStrings": {
        //           "DefaultConnection": "Server=MERT;Database=DBTodoList;Integrated Security=true;TrustServerCertificate=True"
        //                        },

        // Sonrasýnda Program.cs kýsmýna geldik. (Program.cs kýsmýna not býraktým orada görürsün)

        // En sonunda Controller kýsmýna geldik aþaðýdaki iki kodu yazdýk. Ve artýk Dependency Injection hazýr.

        private readonly IDbConnection _connection;  // 1. yazýlan

        public HomeController(IDbConnection connection) // 2. Yazýlan.
        {
            _connection = connection;
        }

        // Dependecy Ýnjection aslýnda bizim sürekli SQLConnection baðlantý ifadesini yazmaktan kurtarýyor, baðlantýyý tek seferde yaptýktan sonra geriye kalanda onu çaðýrýyoruz.

        [HttpGet]
        public IActionResult Index(int? id)
        {
            var todos = _connection.Query<Todo>("SELECT * FROM TBLTodo ORDER BY Id DESC").ToArray();
            ViewBag.Id = id;
            return View(todos);
        }

        [HttpPost]
        public IActionResult Index(Todo model)
        {
            if (ModelState.IsValid)
            {
                var sql = "INSERT INTO TBLTodo (JobName, IsApproved) VALUES(@JobName, @IsApproved)";
                _connection.Execute(sql, model);
            }
            else
            {
                ViewBag.ErrorMsg = "Todo eklenemedi. Daha sonra tekrar deneyin.";
            }

            var todos = _connection.Query<Todo>("SELECT * FROM TBLTodo ORDER BY Id DESC").ToArray();
            return View(todos);
        }

        [HttpPost]
        public IActionResult Update(Todo model)
        {
            if (ModelState.IsValid)
            {
                _connection.Execute("UPDATE TBLTodo SET JobName = @JobName, IsApproved = @IsApproved WHERE Id = @Id", model);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _connection.Execute("DELETE FROM TBLTodo WHERE Id = @id", new { id });
            return RedirectToAction("Index");
        }

        public IActionResult MarkComplete(int id)
        {
            _connection.Execute("UPDATE TBLTodo SET IsApproved = 1 WHERE Id = @id", new { id });
            return RedirectToAction("Index");
        }

        public IActionResult MarkInComplete(int id)
        {
            _connection.Execute("UPDATE TBLTodo SET IsApproved = 0 WHERE Id = @id", new { id });
            return RedirectToAction("Index");
        }
    }
}
