using NovaStore.DataAccess.Models;
using NovaStore.DataAccess.Repositories;

namespace NovaStore.BusinessLogic.Services;

public sealed class StoreService(IStoreRepository repository) : IStoreService
{
    public List<CustomerAccount> Accounts() => repository.Accounts();
    public CustomerAccount? FindAccount(string email) => repository.FindAccount(email);
    public CustomerAccount? FindAccount(int id) => repository.FindAccount(id);
    public bool AddAccount(CustomerAccount account) => repository.AddAccount(account);
    public bool UpdateAccount(CustomerAccount account) => repository.UpdateAccount(account);
    public List<Product> Products() => repository.Products();
    public Product? Find(int id) => repository.Find(id);
    public void Upsert(Product item) => repository.Upsert(item);
    public void Delete(int id) => repository.Delete(id);
    public List<Category> Categories() => repository.Categories();
    public bool AddCategory(Category category) => repository.AddCategory(category);
    public bool UpdateCategory(Category category) => repository.UpdateCategory(category);
    public bool DeleteCategory(int id) => repository.DeleteCategory(id);
    public List<Order> Orders() => repository.Orders();
    public void AddOrder(Order order) => repository.AddOrder(order);
    public void UpdateStatus(string id,string status) => repository.UpdateStatus(id,status);
}
