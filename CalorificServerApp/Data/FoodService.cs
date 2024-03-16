using Microsoft.Data.Sqlite;

namespace CalorificServerApp.Data
{
    public class FoodService
    {
        private readonly string _connectionString = "Data Source=calorific.db";

        public async Task InitializeDatabaseAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var new_tables = connection.CreateCommand();
                new_tables.CommandText = @"
                    CREATE TABLE IF NOT EXISTS FoodItems (
                        Name TEXT PRIMARY KEY,
                        Calories REAL,
                        Fat REAL,
                        Sodium REAL,
                        Carbohydrates REAL,
                        Fiber REAL,
                        Sugars REAL,
                        Protein REAL
                    );

                    CREATE TABLE IF NOT EXISTS Users (
                        Name TEXT PRIMARY KEY,
                        Gender TEXT,
                        Age INTEGER,
                        Height REAL,
                        Weight REAL,
                        SelectedAmr TEXT,
                        SelectedGoal TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Logs (
                        Username TEXT,
                        Date TEXT,
                        FoodName TEXT,
                        Grams INTEGER,
                        Calories REAL,
                        Fat REAL,
                        Sodium REAL,
                        Carbohydrates REAL,
                        Fiber REAL,
                        Sugars REAL,
                        Protein REAL
                    );
                ";

                await new_tables.ExecuteNonQueryAsync();
            }
        }

        public async Task PrepopulateFoodItemsAsync()
        {
            // Get existing food items from the database
            var existing = await GetFoodItemsAsync();

            // Check if food items already exist in the database
            if (existing.Count > 0)
            {
                return;
            }
            // Add food items to the database
            await AddFoodItem("Steak", 250, 26, 65, 0, 0, 0, 26);
            await AddFoodItem("Chicken", 165, 3.6, 74, 0, 0, 0, 31);
            await AddFoodItem("Cheeseburger", 250, 12, 300, 33, 0, 6, 15);
            await AddFoodItem("Rice", 130, 0.3, 0, 28, 0, 0, 2.7);
            await AddFoodItem("Pizza", 285, 12, 683, 35, 2.3, 3.2, 12);
            await AddFoodItem("Fries", 365, 17, 210, 63, 6, 0.6, 3.4);
            await AddFoodItem("Bread", 265, 2.9, 498, 49, 3, 3, 8);
            await AddFoodItem("Pasta", 131, 1.1, 4.5, 25, 1.3, 1.4, 5.2);
            await AddFoodItem("Cheese", 402, 33, 621, 2.2, 0, 0, 25);
        }

        public async Task<List<Food>> GetFoodItems()
        {
            var foodItems = new List<Food>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var exec = connection.CreateCommand();
                exec.CommandText = "SELECT * FROM FoodItems";

                using (var reader = await exec.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        foodItems.Add(ReadFoodItem(reader));
                    }
                }
            }

            return foodItems;
        }

        private Food ReadFoodItem(SqliteDataReader reader)
        {
            return new Food
            {
                Name = reader.GetString(0),
                Calories = reader.GetDouble(1),
                Fat = reader.GetDouble(2),
                Sodium = reader.GetDouble(3),
                Carbohydrates = reader.GetDouble(4),
                Fiber = reader.GetDouble(5),
                Sugars = reader.GetDouble(6),
                Protein = reader.GetDouble(7)
            };
        }

        public async Task AddFoodItem(string name, double calories, double fat, double sodium, double carbohydrates, double fiber, double sugars, double protein)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO FoodItems (Name, Calories, Fat, Sodium, Carbohydrates, Fiber, Sugars, Protein) VALUES (@Name, @Calories, @Fat, @Sodium, @Carbohydrates, @Fiber, @Sugars, @Protein)";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Calories", calories);
                command.Parameters.AddWithValue("@Fat", fat);
                command.Parameters.AddWithValue("@Sodium", sodium);
                command.Parameters.AddWithValue("@Carbohydrates", carbohydrates);
                command.Parameters.AddWithValue("@Fiber", fiber);
                command.Parameters.AddWithValue("@Sugars", sugars);
                command.Parameters.AddWithValue("@Protein", protein);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task AddUser(string name, string gender, int age, double height, double weight, string selectedAmr, string selectedGoal)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (Name, Gender, Age, Height, Weight, SelectedAmr, SelectedGoal) VALUES (@Name, @Gender, @Age, @Height, @Weight, @SelectedAmr, @SelectedGoal)";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Height", height);
                command.Parameters.AddWithValue("@Weight", weight);
                command.Parameters.AddWithValue("@SelectedAmr", selectedAmr);
                command.Parameters.AddWithValue("@SelectedGoal", selectedGoal);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<User> GetUser(string name)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users WHERE Name = @Name";
                command.Parameters.AddWithValue("@Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Name = reader.GetString(0),
                            Gender = reader.GetString(1),
                            Age = reader.GetInt32(2),
                            Height = reader.GetDouble(3),
                            Weight = reader.GetDouble(4),
                            SelectedAmr = reader.GetString(5),
                            SelectedGoal = reader.GetString(6)
                        };
                    }
                }
            }

            return null;
        }

        public async Task AddLog(Log log, string username)
        {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Logs (Username, Date, FoodName, Grams, Calories, Fat, Sodium, Carbohydrates, Fiber, Sugars, Protein)
                                 VALUES (@Username, @Date, @FoodName, @Grams, @Calories, @Fat, @Sodium, @Carbohydrates, @Fiber, @Sugars, @Protein)";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Date", log.Date);
                    command.Parameters.AddWithValue("@FoodName", log.FoodName);
                    command.Parameters.AddWithValue("@Grams", log.Grams);
                    command.Parameters.AddWithValue("@Calories", log.Calories);
                    command.Parameters.AddWithValue("@Fat", log.Fat);
                    command.Parameters.AddWithValue("@Sodium", log.Sodium);
                    command.Parameters.AddWithValue("@Carbohydrates", log.Carbohydrates);
                    command.Parameters.AddWithValue("@Fiber", log.Fiber);
                    command.Parameters.AddWithValue("@Sugars", log.Sugars);
                    command.Parameters.AddWithValue("@Protein", log.Protein);

                    await command.ExecuteNonQueryAsync();
                }
        }

        public async Task<List<Log>> GetLogsForUser(string userName)
        {
            var logs = new List<Log>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Logs WHERE Username = @Username";
            command.Parameters.AddWithValue("@Username", userName);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(ReadLogItems(reader));
            }

            return logs;
        }
        private Log ReadLogItems(SqliteDataReader reader)
        {
            return new Log
            {
                Date = reader.GetDateTime(1),
                FoodName = reader.GetString(2),
                Grams = reader.GetInt32(3),
                Calories = reader.GetDouble(4),
                Fat = reader.GetDouble(5),
                Sodium = reader.GetDouble(6),
                Carbohydrates = reader.GetDouble(7),
                Fiber = reader.GetDouble(8),
                Sugars = reader.GetDouble(9),
                Protein = reader.GetDouble(10)
            };
        }
    }
}
