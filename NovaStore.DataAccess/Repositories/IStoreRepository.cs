using NovaStore.DataAccess.Models;

namespace NovaStore.DataAccess.Repositories;

public interface IStoreRepository
{
    List<CustomerAccount> Accounts();
    CustomerAccount? FindAccount(string email);
    CustomerAccount? FindAccount(int id);
    bool AddAccount(CustomerAccount account);
    bool UpdateAccount(CustomerAccount account);
    List<Product> Products();
    Product? Find(int id);
    void Upsert(Product item);
    void Delete(int id);
    List<Category> Categories();
    bool AddCategory(Category category);
    bool UpdateCategory(Category category);
    bool DeleteCategory(int id);
    List<Order> Orders();
    void AddOrder(Order order);
    void UpdateStatus(string id, string status);
}
