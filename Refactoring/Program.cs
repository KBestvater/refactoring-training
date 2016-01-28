using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Refactoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<User> users = LoadUsersFromFile();
            List<Product> products = LoadProductsFromFile();

            Tusc.Start(users, products);
        }

        private static List<Product> LoadProductsFromFile()
        {
            List<Product> products = LoadDataFromFile<Product>(@"Data\Products.json");
            
            return products;
        }

        private static List<User> LoadUsersFromFile()
        {
            List<User> users = LoadDataFromFile<User>(@"Data\Users.json");
            
            return users;
        }

        private static List<T> LoadDataFromFile<T>(string fileName)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(fileName));
            
            return list;
        }
    }
}
