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
    }
}
