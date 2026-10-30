using System.ComponentModel.DataAnnotations;
namespace NovaStore.Models;

public class CustomerAccount { public int Id { get; set; } public string FullName { get; set; } = ""; public string Email { get; set; } = ""; public string Phone { get; set; } = ""; public string PasswordHash { get; set; } = ""; public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }
public class RegisterInput { [Required, StringLength(80, MinimumLength=2)] public string FullName { get; set; } = ""; [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = ""; [Phone, StringLength(30)] public string Phone { get; set; } = ""; [Required, MinLength(8), StringLength(100)] public string Password { get; set; } = ""; [Required, Compare(nameof(Password), ErrorMessage="Passwords do not match.")] public string ConfirmPassword { get; set; } = ""; }
public class LoginInput { [Required, EmailAddress] public string Email { get; set; } = ""; [Required] public string Password { get; set; } = ""; public bool RememberMe { get; set; } }
public class ProfileInput { [Required, StringLength(80, MinimumLength=2)] public string FullName { get; set; } = ""; [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = ""; [Phone, StringLength(30)] public string Phone { get; set; } = ""; }
