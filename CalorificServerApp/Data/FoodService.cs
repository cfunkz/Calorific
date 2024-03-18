using Microsoft.EntityFrameworkCore;

namespace CalorificServerApp.Data
{
    public class AppDatabase : DbContext
    {
        public DbSet<Food> FoodItems { get; set; } // Represents table for Food items
        public DbSet<User> Users { get; set; } // Represents table for Users
        public DbSet<Log> Logs { get; set; } // Represents table for Logs

        public AppDatabase(DbContextOptions<AppDatabase> options) : base(options)
        {
        }

        // This function is for entity framework to setup the db connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=calorific.db"); // Set datasource
        }

        // This function is for entity framweork to configure the db model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>().HasKey(f => f.Id); // Set primary key
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Log>().HasKey(l => l.Id);
        }
    }

    public class FoodService
    {
        private readonly AppDatabase db; // readonly instance of database

        public FoodService(AppDatabase database) // FoodService class with AppDatabase instance as param
        {
            db = database; //Assign AppDatabase to db
        }

        public async Task<List<Food>> GetFoodItems()  // Function to get all food items from the db (using async for concurrency so multiple tasks can be run)
        {
            return await db.FoodItems.ToListAsync(); // Returning a list of food items
        }

        public async Task AddFoodItem(string name, double calories, double fat, double sodium, double carbohydrates, double fiber, double sugars, double protein)
        {
            // Adding a new food item to the FoodItems table
            db.FoodItems.Add(new Food
            {
                Name = name,
                Calories = calories,
                Fat = fat,
                Sodium = sodium,
                Carbohydrates = carbohydrates,
                Fiber = fiber,
                Sugars = sugars,
                Protein = protein
            });
            await db.SaveChangesAsync();
        }

        public async Task AddUser(string name, string gender, int age, double height, double weight, string selectedAmr, string selectedGoal)
        {
            // Adding a new user to the Users table
            db.Users.Add(new User
            {
                Name = name,
                Gender = gender,
                Age = age,
                Height = height,
                Weight = weight,
                SelectedAmr = selectedAmr,
                SelectedGoal = selectedGoal
            });
            await db.SaveChangesAsync();
        }

        public async Task<User> GetUser(string name)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Name == name); //Use firstordefault instead of find because searched by username
        }

        public async Task AddLog(Log log, string username)
        {
            // Adding a new log to the Logs table
            db.Logs.Add(new Log
            {
                Username = username,
                Date = log.Date,
                Time = log.Time,
                FoodName = log.FoodName,
                Grams = log.Grams,
                Calories = log.Calories,
                Fat = log.Fat,
                Sodium = log.Sodium,
                Carbohydrates = log.Carbohydrates,
                Fiber = log.Fiber,
                Sugars = log.Sugars,
                Protein = log.Protein
            });
            await db.SaveChangesAsync();
        }

        public async Task<List<Log>> GetLogsForUser(string username)
        {
            return await db.Logs.Where(l => l.Username == username).ToListAsync();
        }

        //Prepopulate with food items (values per 100g of item)
        public async Task PopulateFoodItems()
        {
            var foodItems = new List<Food>
            {
                new Food { Name = "Steak", Calories = 250, Fat = 26, Sodium = 65, Protein = 26 },
                new Food { Name = "Chicken", Calories = 165, Fat = 3.6, Sodium = 74, Protein = 31 },
                new Food { Name = "Cheeseburger", Calories = 250, Fat = 12, Sodium = 300, Carbohydrates = 33, Sugars = 6, Protein = 15 },
                new Food { Name = "Rice", Calories = 130, Fat = 0.3, Carbohydrates = 28, Protein = 2.7 },
                new Food { Name = "Pizza", Calories = 285, Fat = 12, Sodium = 683, Carbohydrates = 35, Fiber = 2.3, Sugars = 3.2, Protein = 12 },
                new Food { Name = "Fries", Calories = 365, Fat = 17, Sodium = 210, Carbohydrates = 63, Fiber = 6, Sugars = 0.6, Protein = 3.4 },
                new Food { Name = "Bread", Calories = 265, Fat = 2.9, Sodium = 498, Carbohydrates = 49, Fiber = 3, Sugars = 3, Protein = 8 },
                new Food { Name = "Pasta", Calories = 131, Fat = 1.1, Sodium = 4.5, Carbohydrates = 25, Fiber = 1.3, Sugars = 1.4, Protein = 5.2 },
                new Food { Name = "Cheese", Calories = 402, Fat = 33, Sodium = 621, Carbohydrates = 2.2, Protein = 25 }
            };

            db.FoodItems.AddRange(foodItems);
            await db.SaveChangesAsync();
        }
    }
}
