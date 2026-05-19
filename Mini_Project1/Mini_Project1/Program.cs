using Mini_Project1.Methods;
using Mini_Project1.Services;
using Mini_Project1.UI;

FileServices fileService = new();
Appmanagement app = new(fileService);
StartupAnimation.Show();
app.Run();

