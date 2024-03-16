using Microsoft.Data.Sqlite;

namespace CalorificServerApp.Data
{
    public class FoodService
    {
        private readonly string con = "Data Source=calorific.db";

        public async Task InitializeDatabase()
        {
            using (var connection = new SqliteConnection(con))
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
                        Time DATETIME,
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

        public async Task<List<Food>> GetFoodItems()
        {
            var foodItems = new List<Food>();

            using (var connection = new SqliteConnection(con))
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
            using (var connection = new SqliteConnection(con))
            {
                await connection.OpenAsync();

                var exec = connection.CreateCommand();
                exec.CommandText = "INSERT INTO FoodItems (Name, Calories, Fat, Sodium, Carbohydrates, Fiber, Sugars, Protein) VALUES (@Name, @Calories, @Fat, @Sodium, @Carbohydrates, @Fiber, @Sugars, @Protein)";
                exec.Parameters.AddWithValue("@Name", name);
                exec.Parameters.AddWithValue("@Calories", calories);
                exec.Parameters.AddWithValue("@Fat", fat);
                exec.Parameters.AddWithValue("@Sodium", sodium);
                exec.Parameters.AddWithValue("@Carbohydrates", carbohydrates);
                exec.Parameters.AddWithValue("@Fiber", fiber);
                exec.Parameters.AddWithValue("@Sugars", sugars);
                exec.Parameters.AddWithValue("@Protein", protein);

                await exec.ExecuteNonQueryAsync();
            }
        }

        public async Task AddUser(string name, string gender, int age, double height, double weight, string selectedAmr, string selectedGoal)
        {
            using (var connection = new SqliteConnection(con))
            {
                await connection.OpenAsync();

                var exec = connection.CreateCommand();
                exec.CommandText = "INSERT INTO Users (Name, Gender, Age, Height, Weight, SelectedAmr, SelectedGoal) VALUES (@Name, @Gender, @Age, @Height, @Weight, @SelectedAmr, @SelectedGoal)";
                exec.Parameters.AddWithValue("@Name", name);
                exec.Parameters.AddWithValue("@Gender", gender);
                exec.Parameters.AddWithValue("@Age", age);
                exec.Parameters.AddWithValue("@Height", height);
                exec.Parameters.AddWithValue("@Weight", weight);
                exec.Parameters.AddWithValue("@SelectedAmr", selectedAmr);
                exec.Parameters.AddWithValue("@SelectedGoal", selectedGoal);

                await exec.ExecuteNonQueryAsync();
            }
        }

        public async Task<User> GetUser(string name)
        {
            using (var connection = new SqliteConnection(con))
            {
                await connection.OpenAsync();

                var exec = connection.CreateCommand();
                exec.CommandText = "SELECT * FROM Users WHERE Name = @Name";
                exec.Parameters.AddWithValue("@Name", name);

                using (var reader = await exec.ExecuteReaderAsync())
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
                using (var connection = new SqliteConnection(con))
                {
                    await connection.OpenAsync();

                    var exec = connection.CreateCommand();
                    exec.CommandText = @"INSERT INTO Logs (Username, Date, FoodName, Grams, Calories, Fat, Sodium, Carbohydrates, Fiber, Sugars, Protein)
                                 VALUES (@Username, @Date, @FoodName, @Grams, @Calories, @Fat, @Sodium, @Carbohydrates, @Fiber, @Sugars, @Protein)";
                    exec.Parameters.AddWithValue("@Username", username);
                    exec.Parameters.AddWithValue("@Date", log.Date);
                    exec.Parameters.AddWithValue("@FoodName", log.FoodName);
                    exec.Parameters.AddWithValue("@Grams", log.Grams);
                    exec.Parameters.AddWithValue("@Calories", log.Calories);
                    exec.Parameters.AddWithValue("@Fat", log.Fat);
                    exec.Parameters.AddWithValue("@Sodium", log.Sodium);
                    exec.Parameters.AddWithValue("@Carbohydrates", log.Carbohydrates);
                    exec.Parameters.AddWithValue("@Fiber", log.Fiber);
                    exec.Parameters.AddWithValue("@Sugars", log.Sugars);
                    exec.Parameters.AddWithValue("@Protein", log.Protein);

                    await exec.ExecuteNonQueryAsync();
                }
        }

        public async Task<List<Log>> GetLogsForUser(string userName)
        {
            var logs = new List<Log>();

            using var connection = new SqliteConnection(con);
            await connection.OpenAsync();

            var exec = connection.CreateCommand();
            exec.CommandText = "SELECT * FROM Logs WHERE Username = @Username";
            exec.Parameters.AddWithValue("@Username", userName);

            var reader = await exec.ExecuteReaderAsync();

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
                Time = reader.GetDateTime(2),
                FoodName = reader.GetString(3),
                Grams = reader.GetInt32(4),
                Calories = reader.GetDouble(5),
                Fat = reader.GetDouble(6),
                Sodium = reader.GetDouble(7),
                Carbohydrates = reader.GetDouble(8),
                Fiber = reader.GetDouble(9),
                Sugars = reader.GetDouble(10),
                Protein = reader.GetDouble(11)
            };
        }
        public async Task PopulateFoodItems()
        {
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
    }
}
