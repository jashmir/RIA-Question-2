using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestSimulatorForGETPOST
{
    class Program
    {
        private const string BaseUrl = "https://localhost:7160"; // Update with your actual API URL
        private static int currentId = 1; // Initialize the current ID

        static async Task Main()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseUrl);

                    // Simulate POST requests with random customer data
                    for (int i = 1; i <= 5; i++) // Change the number of requests as needed
                    {
                        var randomCustomers = GenerateRandomCustomers(2); // Generate 2 random customers
                        Console.WriteLine($"Sending POST request to add customers:");
                        await SendPostRequest(httpClient, randomCustomers);


                        //foreach (var randomCustomer in randomCustomers)
                        //{
                        //    Console.WriteLine($"{randomCustomer.FirstName} {randomCustomer.LastName}");
                        //    var customerList = new List<Customers>(randomCustomers);
                        //    await SendPostRequest(httpClient, customerList);
                        //}
                    }

                    // Simulate GET request to retrieve all customers
                    Console.WriteLine("Sending GET request to retrieve all customers:");
                    await SendGetRequest(httpClient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task SendPostRequest(HttpClient httpClient, List<Customers> customers)
        {
            try
            {
                var json = JsonConvert.SerializeObject(customers); // Serialize the list of customers
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("/api/customer", content);

                if ((int)response.StatusCode == 200)
                {
                    Console.WriteLine("POST request successful. All the customers added successfully");
                }
                else if((int)response.StatusCode == 206)
                {
                    Console.WriteLine($"POST request parially failed with status code: {response.StatusCode}. Customers added partially.");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error message: {errorMessage}");
                }
                else
                {
                    Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error message: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending POST request: {ex.Message}");
            }
        }


        static async Task SendGetRequest(HttpClient httpClient)
        {
            try
            {
                var response = await httpClient.GetAsync("/api/Customer");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var customers = JsonConvert.DeserializeObject<List<Customers>>(json);

                    Console.WriteLine("GET request successful. Customers:");
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"{customer.FirstName} {customer.LastName}, Age: {customer.Age}, ID: {customer.Id}");
                    }
                }
                else
                {
                    Console.WriteLine($"GET request failed with status code: {response.StatusCode}");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error message: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending GET request: {ex.Message}");
            }
        }

        static List<Customers> GenerateRandomCustomers(int count)
        {
            var random = new Random();
            var firstNames = new[] { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
            var lastNames = new[] { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

            var generatedCustomers = new List<Customers>();

            for (int i = 0; i < count; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var age = random.Next(10, 91); // Generates random age between 10 and 90

                generatedCustomers.Add(new Customers { FirstName = firstName, LastName = lastName, Age = age, Id = currentId });
                currentId++; // Increment the ID for the next customer
            }

            return generatedCustomers;
        }
    }

    public class Customers
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }
    }
}