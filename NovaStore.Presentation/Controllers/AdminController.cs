using Microsoft.AspNetCore.Mvc;
using NovaStore.BusinessLogic.Services;
using NovaStore.DataAccess.Models;

namespace NovaStore.Presentation.Controllers;

public class AdminController(IStoreService store, IConfiguration config) : Controller
{
    private bool IsAdmin => HttpContext.Session.GetString("AdminRole") == "Admin" || HttpContext.Session.GetString("admin") == "yes";
    [HttpGet] public IActionResult Login() => IsAdmin ? RedirectToAction(nameof(Index)) : View();
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Login(string password)
    {
        if (string.Equals(password, config["AdminPassword"], StringComparison.Ordinal))
        { HttpContext.Session.SetString("AdminRole", "Admin"); HttpContext.Session.SetString("admin", "yes"); return RedirectToAction(nameof(Index)); }
        ViewBag.Error = "Incorrect password"; return View();
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Logout() { HttpContext.Session.Remove("admin"); HttpContext.Session.Remove("AdminRole"); return RedirectToAction("Index", "Home"); }
    public IActionResult Index()
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        ViewBag.Orders = store.Orders(); return View(store.Products());
    }
    public IActionResult Details(int id)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        var product = store.Find(id); return product is null ? NotFound() : View(product);
    }
    public IActionResult Edit(int id = 0)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        ViewBag.Categories = store.Categories(); return View(id == 0 ? new Product() : store.Find(id) ?? new Product());
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        if (!ModelState.IsValid || !store.Categories().Any(category => category.Name.Equals(product.Category, StringComparison.OrdinalIgnoreCase)))
        {
            if (ModelState.IsValid) ModelState.AddModelError(nameof(product.Category), "Choose an existing category.");
            ViewBag.Categories = store.Categories();
            return View(product);
        }
        store.Upsert(product); return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        store.Delete(id); return RedirectToAction(nameof(Index));
    }
    public IActionResult Categories()
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        ViewBag.Products = store.Products(); return View(store.Categories());
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult AddCategory(Category category)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        if (!ModelState.IsValid || !store.AddCategory(category)) TempData["CategoryError"] = "Enter a valid category name that is not already in use.";
        return RedirectToAction(nameof(Categories));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult EditCategory(Category category)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        if (!ModelState.IsValid || !store.UpdateCategory(category)) TempData["CategoryError"] = "Category could not be updated. Check its name and try again.";
        return RedirectToAction(nameof(Categories));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult DeleteCategory(int id)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        if (!store.DeleteCategory(id)) TempData["CategoryError"] = "Move or delete this category's products before deleting the category.";
        return RedirectToAction(nameof(Categories));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Status(string id, string status)
    {
        if (!IsAdmin) return RedirectToAction(nameof(Login));
        if (new[] { "New", "Processing", "Shipped", "Delivered", "Cancelled" }.Contains(status)) store.UpdateStatus(id, status);
        return RedirectToAction(nameof(Index));
    }
}
