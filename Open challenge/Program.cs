using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Open_challenge
{
    class Program
    {


        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {

            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
            };
            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };
            SendEventCustomerSameCity(customer, events);
            SendEventCustomerNearestCity(customer, events);
            SendEventCustomerByClosestPrice(customer, events);
            SendEventCustomerNearestCityApi(customer);

        }

        private static List<EventDistance> GetEventDistances(List<Event> events, Customer customer)
        {
            List<EventDistance> eventDistance = new List<EventDistance>();
            foreach (var item in events)
            {
                EventDistance distanceOjb = new EventDistance { Distance = GetDistance(customer.City, item.City), Event = item };
                eventDistance.Add(distanceOjb);
            }

            return eventDistance;
        }
        private static List<EventPrice> GetEventByPrice(List<Event> events)
        {
            List<EventPrice> eventDistance = new List<EventPrice>();
            foreach (var item in events)
            {
                EventPrice distanceOjb = new EventPrice { Price = GetPrice(item), Event = item };
                eventDistance.Add(distanceOjb);
            }

            return eventDistance;
        }
        private static async Task<List<EventDistance>> GetEventDistancesApi( Customer customer, string sameCity = "same")
        {
            try
            {
                List<EventDistance> eventDistance = new List<EventDistance>();
                HttpResponseMessage response = await client.GetAsync($"api_url?customername={customer.Name}&searchType={sameCity}");
                if (response.IsSuccessStatusCode)
                {
                    eventDistance = await response.Content.ReadAsAsync<List<EventDistance>>();
                }


                return eventDistance;
            }
            // 4. TASK
            catch (Exception ex)
            {
                Console.Out.WriteLine($"An error occured while making request. Error is {ex.Message}");
                return null;
            }

            
        }


        private static void SendEventCustomerSameCity(Customer customer, List<Event> events)
        {
            List<EventDistance> eventDistance = GetEventDistances(events, customer);

            var sameCity = eventDistance.Where(x => x.Distance == 0);

            // 1. TASK
            /* 
                1. What should be your approach to getting the list of events? 
                    The list of events can be added using prompt to ask the user to inputs the name and city of the event. and then added to the list of Event.
                2. How would you call the AddToEmail method in order to send the events in an email? Then i will loop through list of Events and call the AddToEmail function 
                3. What is the expected output if we only have the client John Smith? On the Console the output will be something like this "{CustomerName}: {EventName}  in {CityName}"
                4. Do you believe there is a way to improve the code you first wrote? i could use IEmunerable to handle the list 
             */
            foreach (var cityevent in sameCity)
            {
                AddToEmail(customer, cityevent.Event);
            }
        }
        private static void SendEventCustomerNearestCity(Customer customer, List<Event> events)
        {
            // 2. TASK
            /* 
                1. What should be your approach to getting the distance between the customer’s city and
                    the other cities on the list? i will loop through the list of events and in each case call the GetDistance function and save it in a list of EventDistance
                2. How would you get the 5 closest events and how would you send them to the client in an
                    email? I will filter the list of EventDistance and then sort the list in ascending order, then i will add the first five in a new list of EventDistance called sortedByDistance,
                    then i will loop through the list and then call the AddToEmail function
                3. What is the expected output if we only have the client John Smith? On the Console the output will be something like this "{CustomerName}: {EventName}  in {CityName} {(Distance)}"
                4. Do you believe there is a way to improve the code you first wrote? break the part of the code into a new function for readability, also use dictionary to map event name to the distance 
             */
            List<EventDistance> eventDistance = GetEventDistances(events, customer);

            var otherCity = eventDistance.Where(x => x.Distance > 0);
            var sortedByDistance = otherCity.OrderBy(x => x.Distance).Take(5).ToList();
            for (int i = 0; i < sortedByDistance.Count; i++)
            {
                AddToEmail(customer, sortedByDistance[i].Event);
            }
        }

        private static async Task SendEventCustomerNearestCityApi(Customer customer)
        {
            // 3. TASK
            
            List<EventDistance> eventDistance = await GetEventDistancesApi(customer);

            var otherCity = eventDistance.Where(x => x.Distance > 0);
            var sortedByDistance = otherCity.OrderBy(x => x.Distance).Take(5).ToList();
            for (int i = 0; i < sortedByDistance.Count; i++)
            {
                AddToEmail(customer, sortedByDistance[i].Event);
            }
        }
        private static async Task SendEventCustomerByClosestPrice(Customer customer, List<Event> events)
        {
            // 5. TASK

            List<EventPrice> eventDistance = GetEventByPrice(events);

            var sortedByDistance = eventDistance.OrderBy(x => x.Price).Take(5).ToList();
            for (int i = 0; i < sortedByDistance.Count; i++)
            {
                AddToEmail(customer, sortedByDistance[i].Event);
            }
        }

        /*
            * We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */
        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }

        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }

    public class EventDistance
    {
        public int Distance { get; set; }
        public Event Event { get; set; }
    }
    public class EventPrice
    {
        public int Price { get; set; }
        public Event Event { get; set; }
    }
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
  

}
