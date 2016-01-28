using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        private static List<User> allUsers;
        private static User loggedInUser;
        
        public static void Start(List<User> users, List<Product> products)
        {           
            Tusc.allUsers = users;
            
            WriteWelcomeMessage();
            PromptToLogIn();

            if (Tusc.loggedInUser != null)
            {
                double balance = loggedInUser.Balance;
                
                WriteLoggedInWelcomeMessage();

                Console.WriteLine();
                Console.WriteLine("Your balance is " + balance.ToString("C"));

                // Show product list
                while (true)
                {
                    // Prompt for user input
                    Console.WriteLine();
                    Console.WriteLine("What would you like to buy?");
                    for (int i = 0; i < products.Count; i++)
                    {
                        Product prod = products[i];
                        Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
                    }
                    Console.WriteLine(products.Count + 1 + ": Exit");

                    // Prompt for user input
                    Console.WriteLine("Enter a number:");
                    string answer = Console.ReadLine();
                    int num = Convert.ToInt32(answer);
                    num = num - 1; 

                    // Check if user entered number that equals product count
                    if (num == products.Count)
                    {
                        // Update balance
                        foreach (var user in users)
                        {
                            // Check that username and password match
                            if (user.Username == Tusc.loggedInUser.Username && user.Password == Tusc.loggedInUser.Password)
                            {
                                user.Balance = balance;
                            }
                        }

                        // Write out new balance
                        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                        File.WriteAllText(@"Data\Users.json", json);

                        // Write out new quantities
                        string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
                        File.WriteAllText(@"Data\Products.json", json2);

                        PromptToCloseConsole();

                        return;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("You want to buy: " + products[num].Name);
                        Console.WriteLine("Your balance is " + balance.ToString("C"));

                        // Prompt for user input
                        Console.WriteLine("Enter amount to purchase:");
                        answer = Console.ReadLine();
                        int qty = Convert.ToInt32(answer);

                        // Check if balance - quantity * price is less than 0
                        if (balance - products[num].Price * qty < 0)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine("You do not have enough money to buy that.");
                            Console.ResetColor();
                            continue;
                        }

                        // Check if quantity is less than quantity
                        if (products[num].Quantity <= qty)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine("Sorry, " + products[num].Name + " is out of stock");
                            Console.ResetColor();
                            continue;
                        }

                        // Check if quantity is greater than zero
                        if (qty > 0)
                        {
                            // Balance = Balance - Price * Quantity
                            balance = balance - products[num].Price * qty;

                            // Quanity = Quantity - Quantity
                            products[num].Quantity = products[num].Quantity - qty;

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You bought " + qty + " " + products[num].Name);
                            Console.WriteLine("Your new balance is " + balance.ToString("C"));
                            Console.ResetColor();
                        }
                        else
                        {
                            // Quantity is less than zero
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine();
                            Console.WriteLine("Purchase cancelled");
                            Console.ResetColor();
                        }
                    }
                }
            }

            PromptToCloseConsole();
        }

        private static void PromptToLogIn()
        {
            bool tryLogin = true;
            string username;

            while (tryLogin)
            {
                username = PromptForUsername();

                if (string.IsNullOrEmpty(username))
                {
                    tryLogin = false;
                }
                else
                {
                    if (isValidUsername(username))
                    {
                        string password = PromptForPassword();

                        if (isValidPassword(username, password))
                        {
                            SetLoggedInUser(username);
                            tryLogin = false;
                        }
                        else
                        {
                            WriteInvalidLogIn("You entered an invalid password.");
                            tryLogin = true;
                        }
                    }
                    else
                    {
                        WriteInvalidLogIn("You entered an invalid user.");
                        tryLogin = true;
                    }
                }
            }
        }

        private static void WriteInvalidLogIn(string errorMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }

        private static void SetLoggedInUser(string username)
        {
            Tusc.loggedInUser = GetUserByUsername(username);
        }

        private static User GetUserByUsername(string username)
        {
            foreach (User user in Tusc.allUsers)
            {
                if (user.Username == username)
                {
                    return user;
                }
            }

            return null;
        }

        private static bool isValidPassword(string username, string password)
        {
            User user = GetUserByUsername(username);

            return (user.Password == password);
        }

        private static void WriteWelcomeMessage()
        {
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        private static string PromptForUsername()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Username:");

            return Console.ReadLine();
        }

        private static bool isValidUsername(string username)
        {
            User user = GetUserByUsername(username);

            return (user != null);
        }

        private static string PromptForPassword()
        {
            Console.WriteLine("Enter Password:");

            return Console.ReadLine();
        }

        private static void WriteLoggedInWelcomeMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + Tusc.loggedInUser.Username + "!");
            Console.ResetColor();
        }

        private static void PromptToCloseConsole()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }
    }
}
