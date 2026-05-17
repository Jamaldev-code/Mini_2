using Mini_Project1.Methods;
using Mini_Project1.Services;

FileServices fileService = new();
Appmanagement app = new(fileService);
app.Run();

