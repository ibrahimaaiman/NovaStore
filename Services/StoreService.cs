using System.Text.Json;using NovaStore.Models;
namespace NovaStore.Services;
public class StoreService {
 readonly object gate=new();readonly string folder=Path.Combine(AppContext.BaseDirectory,"App_Data");readonly JsonSerializerOptions opts=new(){WriteIndented=true,PropertyNameCaseInsensitive=true};
 public StoreService(){Directory.CreateDirectory(folder);if(!File.Exists(Path.Combine(folder,"products.json")))Save("products.json",Seed());if(!File.Exists(Path.Combine(folder,"orders.json")))Save("orders.json",new List<Order>());if(!File.Exists(Path.Combine(folder,"accounts.json")))Save("accounts.json",new List<CustomerAccount>());}
 List<T> Read<T>(string name){lock(gate){try{return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(Path.Combine(folder,name)),opts)??new();}catch{return new();}}}
 void Save<T>(string name,List<T> data){lock(gate){var file=Path.Combine(folder,name);var tmp=file+".tmp";File.WriteAllText(tmp,JsonSerializer.Serialize(data,opts));File.Move(tmp,file,true);}}
 public List<CustomerAccount> Accounts()=>Read<CustomerAccount>("accounts.json");
 public CustomerAccount? FindAccount(string email)=>Accounts().FirstOrDefault(x=>string.Equals(x.Email,email,StringComparison.OrdinalIgnoreCase));
 public CustomerAccount? FindAccount(int id)=>Accounts().FirstOrDefault(x=>x.Id==id);
 public bool AddAccount(CustomerAccount account){lock(gate){var all=Accounts();if(all.Any(x=>string.Equals(x.Email,account.Email,StringComparison.OrdinalIgnoreCase)))return false;account.Id=all.Count==0?1:all.Max(x=>x.Id)+1;all.Add(account);Save("accounts.json",all);return true;}}
 public bool UpdateAccount(CustomerAccount account){lock(gate){var all=Accounts();if(all.Any(x=>x.Id!=account.Id&&string.Equals(x.Email,account.Email,StringComparison.OrdinalIgnoreCase)))return false;var i=all.FindIndex(x=>x.Id==account.Id);if(i<0)return false;all[i]=account;Save("accounts.json",all);return true;}}
 public List<Product> Products()=>Read<Product>("products.json");public Product? Find(int id)=>Products().FirstOrDefault(x=>x.Id==id);
 public void Upsert(Product item){lock(gate){var all=Products();if(item.Id==0){item.Id=all.Count==0?1:all.Max(x=>x.Id)+1;all.Add(item);}else{var index=all.FindIndex(x=>x.Id==item.Id);if(index>=0)all[index]=item;else all.Add(item);}Save("products.json",all);}}
 public void Delete(int id){lock(gate){var all=Products();all.RemoveAll(x=>x.Id==id);Save("products.json",all);}}
 public List<Order> Orders()=>Read<Order>("orders.json").OrderByDescending(x=>x.CreatedAt).ToList();
 public void AddOrder(Order order){lock(gate){var all=Orders();all.Add(order);Save("orders.json",all);}}
 public void UpdateStatus(string id,string status){lock(gate){var all=Orders();var o=all.FirstOrDefault(x=>x.Id==id);if(o!=null){o.Status=status;Save("orders.json",all);}}}
 static List<Product> Seed(){string u(string id)=>"https://images.unsplash.com/"+id+"?auto=format&fit=crop&w=900&q=85";return new(){
 new(){Id=1,Name="Studio Wireless Headphones",Category="Audio",Description="Immersive sound, premium comfort and a beautifully minimal design.",Price=189,OldPrice=249,Image=u("photo-1505740420928-5e560c06d30e"),Featured=true,Badge="BEST SELLER",Rating=4.9},
 new(){Id=2,Name="Minimalist Wrist Watch",Category="Accessories",Description="A timeless statement for everyday moments.",Price=149,OldPrice=199,Image=u("photo-1523275335684-37898b6baf30"),Featured=true,Badge="NEW",Rating=4.8},
 new(){Id=3,Name="Urban Everyday Sneakers",Category="Fashion",Description="Clean silhouettes, lasting comfort and effortless versatility.",Price=129,OldPrice=169,Image=u("photo-1542291026-7eec264c27ff"),Featured=true,Badge="HOT",Rating=4.9},
 new(){Id=4,Name="Modern Desk Lamp",Category="Home",Description="Warm up your workspace with sculptural contemporary lighting.",Price=89,OldPrice=119,Image=u("photo-1507473885765-e6ed057f782c"),Rating=4.7},
 new(){Id=5,Name="Classic Camera",Category="Electronics",Description="Capture the everyday in extraordinary detail.",Price=429,OldPrice=499,Image=u("photo-1516035069371-29a1b244cc32"),Badge="POPULAR",Rating=4.8},
 new(){Id=6,Name="Leather Weekend Bag",Category="Fashion",Description="Refined utility for your next adventure.",Price=219,OldPrice=289,Image=u("photo-1553062407-98eeb64c6a62"),Rating=4.9},
 new(){Id=7,Name="Smart Fitness Watch",Category="Electronics",Description="Stay connected and move with purpose.",Price=199,OldPrice=259,Image=u("photo-1579586337278-3befd40fd17a"),Badge="NEW",Rating=4.7},
 new(){Id=8,Name="Premium Sunglasses",Category="Accessories",Description="Distinctive eyewear made for sunny days.",Price=79,OldPrice=109,Image=u("photo-1511499767150-a48a237f0083"),Rating=4.8},
 new(){Id=9,Name="Portable Bluetooth Speaker",Category="Audio",Description="Bold audio in a compact, take-anywhere package.",Price=119,OldPrice=159,Image=u("photo-1608043152269-423dbba4e7e1"),Rating=4.6},
 new(){Id=10,Name="Ceramic Coffee Set",Category="Home",Description="Elevate the small rituals of every morning.",Price=59,OldPrice=79,Image=u("photo-1514228742587-6b1558fcca3d"),Rating=4.9},
 new(){Id=11,Name="Everyday Backpack",Category="Fashion",Description="An essential carry for your everyday essentials.",Price=99,OldPrice=139,Image=u("photo-1553062407-98eeb64c6a62"),Rating=4.7},
 new(){Id=12,Name="Wireless Earbuds",Category="Audio",Description="Seamless listening with a pocket-sized design.",Price=109,OldPrice=149,Image=u("photo-1606220588913-b3aacb4d2f46"),Badge="TRENDING",Rating=4.8}};}
}
