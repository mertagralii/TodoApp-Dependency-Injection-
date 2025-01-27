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
        // �lk �nce appsettings.json dosyas�nda ConnectionString ifadesini yazd�k ve default k�sm�nda da sql ba�lant� k�sm�n� yazd�k. ( ordan bakars�n) 

        //   "ConnectionStrings": {
        //           "DefaultConnection": "Server=MERT;Database=DBTodoList;Integrated Security=true;TrustServerCertificate=True"
        //                        },

        // Sonras�nda Program.cs k�sm�na geldik. (Program.cs k�sm�na not b�rakt�m orada g�r�rs�n)

        // En sonunda Controller k�sm�na geldik a�a��daki iki kodu yazd�k. Ve art�k Dependency Injection haz�r.

        private readonly IDbConnection _connection;  // 1. yaz�lan

        public HomeController(IDbConnection connection) // 2. Yaz�lan.
        {
            _connection = connection;
        }

        // Dependecy �njection asl�nda bizim s�rekli SQLConnection ba�lant� ifadesini yazmaktan kurtar�yor, ba�lant�y� tek seferde yapt�ktan sonra geriye kalanda onu �a��r�yoruz.

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
