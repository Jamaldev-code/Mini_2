using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project1.Interfaces
{
    internal interface IFileService
    {
        List<T> Read<T>(string filePath);
        void Write<T>(string filePath, List<T> data);
    }
}
