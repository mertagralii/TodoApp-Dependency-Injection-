# TodoApp (Dependency Injection)

#Dependency Injection
Dependecy İnjection aslında bizim sürekli SQLConnection bağlantı ifadesini yazmaktan kurtarıyor, bağlantıyı tek seferde yaptıktan sonra geriye kalanda onu çağırıyoruz.

#Dependecy İnjeciton İçin Yaptığımız Adımlar:

# 1-) Appsettings.Json'a ConnectionString bağlantısı Kurulması : 

appsettings.json Kısmına Eklediğimiz Kod : 

 "ConnectionStrings": {
     "DefaultConnection": "Server=MERT;Database=DBTodoList;Integrated Security=true;TrustServerCertificate=True"
 },

# 2-) Program.cs'e Servisin Kurulması : 

Program.cs kısmına eklediğimiz kod : 

// Unutma bunu Builder.Services. AddControllersWithViews'in altına yazman gerekiyor.


 builder.Services.AddTransient<IDbConnection>(serviceProvider => 
 {
     var configuration = serviceProvider.GetRequiredService<IConfiguration>();
     var connectionString = configuration.GetConnectionString("DefaultConnection");

     return new SqlConnection(connectionString);
 }); 

# 3-) Kullanılacak Controller'de Dependecy İnjection'un Çağrılması : 

Ardından Controller Kısmında yapılması gereken işlemler : 

1-) Her yeni bir controller açtığın zaman bu bağlantıyı yapman gerekiyor.

 private readonly IDbConnection _connection;  

 public HomeController(IDbConnection connection)  // NOT : HomeController'de bu işlemi gördüğümüz için public HomeController yazdık. Eğer AdminController'de olsaydık Public AdminController yazardık.
 {
     _connection = connection;
 }
