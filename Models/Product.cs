using System.ComponentModel.DataAnnotations;
namespace NovaStore.Models;
public class Product { public int Id {get;set;} [Required,StringLength(100)] public string Name {get;set;}=""; [Required] public string Category {get;set;}=""; public string Description {get;set;}=""; [Range(0,1000000)] public decimal Price {get;set;} public decimal OldPrice {get;set;} public string Image {get;set;}=""; public int Stock {get;set;}=20; public bool Featured {get;set;} public string Badge {get;set;}=""; public double Rating {get;set;}=4.8; }
public class CartLine {public int ProductId {get;set;} public int Quantity {get;set;} }
public class CartDisplay {public Product Product {get;set;}=new();public int Quantity {get;set;} public decimal Total=>Product.Price*Quantity;}
public class CheckoutInput { [Required] public string FullName {get;set;}="";[Required,EmailAddress] public string Email {get;set;}="";[Required] public string Phone {get;set;}="";[Required] public string Address {get;set;}="";[Required] public string City {get;set;}=""; public string Notes {get;set;}="";}
public class Order {public string Id {get;set;}="";public DateTime CreatedAt {get;set;}=DateTime.UtcNow;public CheckoutInput Customer {get;set;}=new();public List<OrderItem> Items {get;set;}=new();public decimal Subtotal {get;set;} public decimal Shipping {get;set;} public decimal Total=>Subtotal+Shipping;public string Status {get;set;}="New";}
public class OrderItem {public string Name {get;set;}="";public decimal Price {get;set;}public int Quantity {get;set;} public string Image {get;set;}="";}
