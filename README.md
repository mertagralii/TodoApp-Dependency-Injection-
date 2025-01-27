# TodoApp (Dependency Injection)

## Dependency Injection Nedir?
Dependency Injection (DI), uygulama içinde sürekli olarak SQL bağlantısı veya başka bağımlılıkları tekrar tekrar tanımlamaktan kurtarır. Bağımlılığı bir kez tanımladıktan sonra, ihtiyacımız olduğunda çağırarak kullanabiliriz.

---

## Dependency Injection İçin Yaptığımız Adımlar

### 1-) **`appsettings.json` Dosyasına ConnectionString Eklenmesi**
`appsettings.json` dosyasına aşağıdaki kodu ekleyerek veritabanı bağlantısını tanımlıyoruz:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=MERT;Database=DBTodoList;Integrated Security=true;TrustServerCertificate=True"
  }
}
```

---

### 2-) **`Program.cs` Dosyasına Servis Tanımlanması**
`Program.cs` dosyasına aşağıdaki kodu ekliyoruz. Bu kod, DI kullanarak SQL bağlantısını uygulama içinde kullanılabilir hale getirir.

```csharp
// Bu kodu Builder.Services.AddControllersWithViews'in altına yazmalısın.
builder.Services.AddTransient<IDbConnection>(serviceProvider => 
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});
```

---

### 3-) **Controller'de Dependency Injection Kullanımı**
DI ile tanımladığımız bağlantıyı Controller içinde şu şekilde çağırabiliriz:

```csharp
// Her yeni controller açtığınızda bu bağlantıyı tanımlamanız gerekiyor.

private readonly IDbConnection _connection;

public HomeController(IDbConnection connection) // Örneğin HomeController için tanımlıyoruz.
{
    _connection = connection;
}
```

> **Not:** Eğer `AdminController` gibi farklı bir controller oluşturuyorsanız, constructor kısmını ona uygun şekilde düzenlemelisiniz: `public AdminController(IDbConnection connection)` gibi.

---

Bu adımları takip ederek Dependency Injection'ı projeye entegre edebilirsiniz. Her adım, hem okunabilir hem de sürdürülebilir bir kod yapısı sağlar.
