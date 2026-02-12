//🔁 Full relationship recap (important for interviews)
//Person → Order (one-to-many)

//Order → OrderItem (one-to-many)

//Product ↔ Category (many-to-many via ProductCategory)
//Product ──< ProductCategory >── Category

using LINQPractice;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

var categories = new List<Category>
{
    new() { Id = 1, Name = "Electronics" },
    new() { Id = 2, Name = "Books" },
    new() { Id = 3, Name = "Fitness" },
    new() { Id = 4, Name = "Clothing" },
    new() { Id = 5, Name = "Home" }
};
var products = new List<Product>
{
    new() { Id = 1, Name = "Laptop", Price = 2500 },
    new() { Id = 2, Name = "Headphones", Price = 300 },
    new() { Id = 3, Name = "Book - Clean Code", Price = 45 },
    new() { Id = 4, Name = "Dumbbells", Price = 150 },
    new() { Id = 5, Name = "T-Shirt", Price = 40 },
    new() { Id = 6, Name = "Coffee Machine", Price = 800 },
    new() { Id = 7, Name = "Running Shoes", Price = 220 },
    new() { Id = 8, Name = "Tablet", Price = 1200 }
};
var productCategories = new List<ProductCategory>
{
    new() { ProductId = 1, CategoryId = 1 },
    new() { ProductId = 2, CategoryId = 1 },
    new() { ProductId = 3, CategoryId = 2 },
    new() { ProductId = 4, CategoryId = 3 },
    new() { ProductId = 5, CategoryId = 4 },
    new() { ProductId = 6, CategoryId = 5 },
    new() { ProductId = 7, CategoryId = 3 },
    new() { ProductId = 7, CategoryId = 4 },
    new() { ProductId = 8, CategoryId = 1 }
};
var people = new List<Person>
{
    new() { Id = 1, FullName = "Samir Eldarov", Country = "AZ", IsActive = true, CreatedAt = DateTime.UtcNow.AddYears(-2) },
    new() { Id = 2, FullName = "Aysel Aliyeva", Country = "AZ", IsActive = true, CreatedAt = DateTime.UtcNow.AddYears(-1) },
    new() { Id = 3, FullName = "Murad Hasanov", Country = "TR", IsActive = false, CreatedAt = DateTime.UtcNow.AddYears(-3) },
    new() { Id = 4, FullName = "John Smith", Country = "UK", IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-8) },
    new() { Id = 5, FullName = "Emily Brown", Country = "UK", IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-4) },
    new() { Id = 6, FullName = "Carlos Ruiz", Country = "ES", IsActive = false, CreatedAt = DateTime.UtcNow.AddYears(-1) }
};
var orders = new List<Order>
{
    new() { Id = 1, PersonId = 1, OrderDate = DateTime.UtcNow.AddMonths(-10), Status = "Completed",
        OrderItems = new()
        {
            new() { ProductId = 1, Quantity = 1 },
            new() { ProductId = 2, Quantity = 2 }
        }
    },
    new() { Id = 2, PersonId = 1, OrderDate = DateTime.UtcNow.AddMonths(-6), Status = "Completed",
        OrderItems = new()
        {
            new() { ProductId = 3, Quantity = 1 }
        }
    },
    new() { Id = 3, PersonId = 2, OrderDate = DateTime.UtcNow.AddMonths(-3), Status = "Pending",
        OrderItems = new()
        {
            new() { ProductId = 8, Quantity = 1 }
        }
    },
    new() { Id = 4, PersonId = 3, OrderDate = DateTime.UtcNow.AddYears(-1), Status = "Cancelled",
        OrderItems = new()
        {
            new() { ProductId = 5, Quantity = 3 }
        }
    },
    new() { Id = 5, PersonId = 4, OrderDate = DateTime.UtcNow.AddMonths(-2), Status = "Completed",
        OrderItems = new()
        {
            new() { ProductId = 6, Quantity = 1 },
            new() { ProductId = 3, Quantity = 2 }
        }
    },
    new() { Id = 6, PersonId = 5, OrderDate = DateTime.UtcNow.AddMonths(-1), Status = "Completed",
        OrderItems = new()
        {
            new() { ProductId = 7, Quantity = 1 }
        }
    }
};

//1) Total spent per person

//Return total money spent per person
//(Quantity * Product.Price, only Completed orders).

//List<(string FullName, decimal TotalSpent)>
var totalSpentPerPuerson = people.Select(x =>
{
    var personFullName = x.FullName;
    var totalSpent = x.Orders
        .Where(o => o.Status == "Completed")
        .SelectMany(o => o.OrderItems)
        .Sum(x => x.Quantity * x.Product.Price);
    return (personFullName, totalSpent);
}).ToList();


//2) People who never ordered

//Return people who have 0 orders.

//List<Person>
var peoplethatNeverOrdered = people.Where(x => !x.Orders.Any()).ToList();


//3) Orders with total amount

//For each order return its total amount.

//List<(int OrderId, decimal Total)>
var p = orders.Select(x =>
{
    var orderId = x.Id;
    var total = x.OrderItems.Sum(y => y.Quantity * y.Product.Price);
    return (orderId, total);
}).ToList();
//4
var p2 = people.Select(x =>
{
    var person = x;
    var totalSpent = x.Orders.Sum(o => o.OrderItems.Sum(y => y.Quantity * y.Product.Price));
    return (person, totalSpent);
}).OrderByDescending(x=>x.totalSpent).Take(3).ToList();

//5) Most sold product (by quantity)

//Return the product sold in highest total quantity.

//(Product Product, int TotalQuantity)
var mostSoldProduct = orders
    .SelectMany(o => o.OrderItems)
    .GroupBy(oi => oi.ProductId)
    .Select(g => new
    {
        Product = g.Key,
        TotalQuantity = g.Sum(oi => oi.Quantity)
    })
    .OrderByDescending(x => x.TotalQuantity)
    .FirstOrDefault();
//7) Active users with cancelled orders

//Return active people who have at least one cancelled order.

//List<Person>
var p22 = people.Where(x => x.IsActive).Select(x =>
{
    var person = x;
    var check = x.Orders.Any(o => o.Status == "Cancelled");
    return (person, check);
}).Where(x => x.check).Select(x => x.person).ToList();

var activeUsersWithCancelledOrders = people
    .Where(p => p.IsActive && p.Orders.Any(o => o.Status == "Cancelled"))
    .ToList();

//8) Orders containing multiple categories

//Return orders that include products from more than one category.

//List<Order>

var ordersWithMultipleCategories = orders.Where(o =>
{
    var categoryCount = o.OrderItems
        .Select(oi => oi.Product.ProductCategories.Select(pc => pc.CategoryId))
        .SelectMany(c => c)
        .Distinct()
        .Count();
    return categoryCount > 1;
}).ToList();

var p333 = orders.Where(x => x.OrderItems.SelectMany(x => x.Product.ProductCategories.Select(x => x.CategoryId).Distinct()).Count() > 1).ToList();


var p3 = people.Select(x =>
{
    var FullName=x.FullName;
    var AvgOrderValue = x.Orders.Any() ? x.Orders.Average(x => x.OrderItems.Sum(x => x.Quantity * x.Product.Price)) : 0;
    return (FullName, AvgOrderValue);
}).ToList();

var p4 = people.Select(x =>
{
    var person = x;
    var order = x.Orders.OrderByDescending(x => x.OrderDate).FirstOrDefault();
    return (person, order);
}).ToList();

var p5 = orders.Where(x => x.Status == "Completed").GroupBy(x => new
{
    x.OrderDate.Year,
    x.OrderDate.Month
}).Select(x =>
{
    var Year = x.Key.Year;
    var Month = x.Key.Month;
    var Revenue = x.SelectMany(x=>x.OrderItems).Sum(x=>x.Quantity * x.Product.Price);
    return (Year, Month, Revenue);
}).OrderBy(x=>x.Year).ThenBy(x=>x.Month).ToList();
//var p7 = categories.Select(x =>
//{
//    var Category = x.Name;
//    var OrderCount=orders.Count(x=>x.)

//}).ToList();
var p6 = products.Where(x => orders.Any(o => o.OrderItems.Any(oi => oi.ProductId == x.Id)));
//12) Products never ordered

//Return products that were never ordered.

//List<Product>


var productIds = orders.SelectMany(x => x.OrderItems).Select(x => x.OrderId).Distinct();
var produscts = products.Where(x => !productIds.Contains(x.Id)).ToList();


var productsNeverOrdered = products.Where(p => !orders.Any(o => o.OrderItems.Any(oi => oi.ProductId == p.Id))).ToList();

//13) Category popularity
//For each category return:
//number of distinct orders

//total quantity sold

//List<(string Category, int OrderCount, int TotalQuantity)>


var p8 = categories.Select(x =>
{
    var Category = x.Name;
    var relatedOrderItems = orders.SelectMany(x => x.OrderItems).Where(oi => oi.Product.ProductCategories.Any(pc => pc.CategoryId == x.Id)).Select(x => new { x.OrderId, x.Quantity }).ToList();
    var OrderCount = relatedOrderItems.Select(x => x.OrderId).Distinct().Count();
    var TotalQuantity = relatedOrderItems.Sum(x => x.Quantity);
    return (Category, OrderCount, TotalQuantity);
}).ToList();

//14) Customer retention

//Return people who placed more than 1 completed order.

//List<Person>
var p9 = people.Where(x => x.Orders.Where(x => x.Status == "Completed").SelectMany(o => o.OrderItems).Select(x=>x.OrderId).Count()>1).ToList();

var p10 = people.Where(x => x.Orders.Count(o => o.Status == "Completed") > 1).ToList();

//15) Highest single order

//18) Top product per category

//For each category return the top-selling product.

//List<(string Category, Product Product)>
//Return the order with the highest total value.

//(Order Order, decimal Total)
var p11 = orders.Select(x =>
{
    var Order = x;
    var Total = x.OrderItems.Sum(x => x.Quantity * x.Product.Price);
    return (Order, Total);
}).OrderByDescending(x=>x.Total).FirstOrDefault();

//16) Cross - category buyers

//Return people who bought products from at least 2 different categories.

//List<Person>
var p12 = people.Where(x =>
    x.Orders.Where(y => y.Status == "Completed").SelectMany(x => x.OrderItems).SelectMany(x => x.Product.ProductCategories).Select(x => x.CategoryId)
    .Distinct().Count() >= 2
).ToList();

//var p13 = products.Select(x =>
//{
//    var Category = x.ProductCategories.FirstOrDefault(y=>)
//})

var p111 = people.Where(x => x.IsActive).ToList();
//2) People with at least one order
//Return people who have any order.
//List<Person>
var peopleWithAtLeastOneOrder = people
    .Where(p => p.Orders.Any())
    .ToList();
var p112 = orders.Where(x => x.Status == "Completed").ToList();

//5) Total items per order
//For each order, return total quantity of items.
//List<(int OrderId, int TotalQuantity)>

var p113 = orders.Select(x =>
{
    var OrderId = x.Id;
    var TotalQuantity = x.OrderItems.Sum(x => x.Quantity);
    return (OrderId, TotalQuantity);
}).ToList();
var p114 = products.Where(x => x.Price < 100).ToList();
var p115 = orders.Where(x => x.OrderDate >= DateTime.UtcNow.AddDays(-30)).ToList();

//9) Products with at least one category

//Return products that belong to any category.

//List<Product>

var p116 = products.Where(x=>x.ProductCategories.Any()).ToList();

//10) Orders without items
//Return orders that have 0 order items.
//List<Order>
var pp17 = orders.Where(x => !x.OrderItems.Any()).ToList();

//1) Active people with no completed orders
//Return people who are active but never completed an order.
//List<Person>

var p118 = people.Where(x => x.IsActive && !x.Orders.Any(o => o.Status == "Completed"));

//2) People with exactly 1 completed order
//Return people who placed exactly one completed order.
//List<Person>

var p119 = people.Where(x => x.Orders.Count(o => o.Status == "Completed") == 1).ToList();
//people.Where(x => x.Orders.Where(o => o.Status == "Completed").Count() == 1).ToList();

//3) Orders with total quantity > 3
//Return orders where the sum of item quantities is greater than 3.
//List<Order>
var p120 = orders.Where(x => x.OrderItems.Sum(ot => ot.Quantity) > 3).ToList();

//4) People who ordered a specific product

//Return people who have ever ordered product "Laptop".

//List<Person>

var p121 = people.Where(x => x.Orders.Any(o => o.OrderItems.Any(x => x.Product.Name == "Laptop"))).ToList();

//5) Total revenue per person (completed orders)

//Return total revenue per person.

//List<(string FullName, decimal Revenue)>
var p122 = people.Select(x =>
{
    var FullName = x.FullName;
    var Revenue = x.Orders.Where(o => o.Status == "Completed").Sum(y => y.OrderItems.Sum(ot => ot.Quantity * ot.Product.Price));
    return (FullName, Revenue);
}).ToList();

//6) Orders containing more than 2 items
//Return orders that have more than 2 order items (not quantity, item count).
//List<Order>
var p123 = orders.Where(o => o.OrderItems.Count() > 2).ToList();

var productsOrderedMoreThanOnce = products
    .Where(p =>
        orders
            .SelectMany(o => o.OrderItems
                .Where(i => i.ProductId == p.Id)
                .Select(i => o.Id))
            .Distinct()
            .Count() > 1
    )
    .ToList();

var p124 = people.Where(x => x.Orders.Where(o => o.Status == "Completed").SelectMany(o => o.OrderItems).SelectMany(y => y.Product.ProductCategories)
.Select(x => x.CategoryId).Distinct().Count() == 1).ToList();

var p125 = people.Select(x =>
{
    var FullName = x.FullName;
    var FOD = x.Orders.OrderBy(x => x.OrderDate).FirstOrDefault()?.OrderDate;
    var w = x.Orders.Min(x => x.OrderDate);
    return (FullName, FOD);
}).ToList();

var p128 = orders.Where(x => x.Status == "Completed").GroupBy(x => new { x.OrderDate.Year, x.OrderDate.Month }).Select(x =>
{
    var Year =x.Key.Year;
    var Month = x.Key.Month;
    var Revenue = x.SelectMany(x => x.OrderItems).Sum(x => x.Quantity * x.Product.Price);
    return (Year, Month, Revenue);

}).ToList();

var p126 = orders.Where(x => x.OrderItems.Any() && x.OrderItems.Average(y => y.Product.Price) > 500).ToList();

//1) Top customer per country
//For each country, return the person who spent the most total money
//(only completed orders).
//List<(string Country, Person Person, decimal TotalSpent)>

var p127 = people.GroupBy(x => x.Country).Select(x =>
{
    var Country = x.Key;
    var topPerson = x.Select(p =>
    {
        var person = p;
        var TotalSpent = p.Orders.Where(o=>o.Status == "Completed").Sum(o=>o.OrderItems.Sum(oi=>oi.Quantity* oi.Product.Price));
        return (person, TotalSpent);
    }).FirstOrDefault();

    return (Country, topPerson.person, topPerson.TotalSpent);
});
//2) Revenue per month per category
//Group completed orders by Year + Month + Category and calculate revenue.
//List<(int Year, int Month, string Category, decimal Revenue)>

var p129 = orders.Where(x => x.Status == "Completed").SelectMany(x => x.OrderItems.SelectMany(oi => oi.Product.ProductCategories.Select(pc => new
{
    Year = x.OrderDate.Year,
    Month = x.OrderDate.Month,
    Category = pc.Category.Name,
    Revenue = oi.Quantity * oi.Product.Price
}))).GroupBy(x => new { x.Year, x.Month, x.Category }).Select(x =>
{
    var Year = x.Key.Year;
    var Month = x.Key.Month;
    var Category = x.Key.Category;
    var Revenue = x.Sum(x => x.Revenue);
    return (Year, Month, Category, Revenue);
}).ToList();
 
//3) Products bought by inactive users only

//Return products that were never bought by active users.
//List<Product>
var p130 = products.Where(x => people.Where(p => !p.IsActive).SelectMany(p => p.Orders).SelectMany(o => o.OrderItems).Where(oi => oi.ProductId == x.Id).Any()).ToList();

//4) Average order value per person
//For each person, return their average order value (only completed orders).
//List<(string FullName, decimal AvgOrderValue)>
var p131 = people.Select(x =>
{
    var FullName = x.FullName;
    var AvgOrderValue = x.Orders.Any() ? x.Orders.Where(x => x.Status == "Completed").Average(x => x.OrderItems.Sum(o => o.Product.Price * o.Quantity)) : 0;
    return (FullName, AvgOrderValue);
}).ToList();

//5) Orders with mixed price ranges
//Return orders that contain:
//at least one product < 100
//and at least one product > 1000
//List<Order>
var p132 = orders.Where(x => x.OrderItems.Count(ot => ot.Product.Price < 100) >= 1 && x.OrderItems.Count(ot => ot.Product.Price > 1000) >= 1).ToList();
var p133 = orders.Where(x => x.OrderItems.Any(y => y.Product.Price < 100) && x.OrderItems.Any(y => y.Product.Price > 1000)).ToList();

//var p134 = people.Select(x =>
//{
//    var Person = x;
//    var Category = x.Orders
//}).ToList();

//6) Category dominance per person

//Return people who spent more than 70% of their total spending in one category.

//List<(Person Person, string Category, decimal Percentage)>

//7) Most frequent buyer

//Return the person who placed the most completed orders.

//(Person Person, int OrderCount)

var p135 = people.Select(x =>
{
    var Person = x;
    var OrderCount = x.Orders.Where(o => o.Status == "Completed").Count();
    return (Person, OrderCount);
}).MaxBy(x => x.OrderCount);
//OrderByDescending(x => x.OrderCount).FirstOrDefault();