using Mini_Project1.App;
using Mini_Project1.Services;
using Mini_Project1.UI;
using System.Text;

//  Azərbaycan dilini və bütün unicode simvolları dəstəklə
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

FileServices fileService = new();
Appmanagement app = new(fileService);
StartupAnimation.Show();
app.Run();
