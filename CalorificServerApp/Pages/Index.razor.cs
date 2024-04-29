using CalorificServerApp.Data;

namespace CalorificServerApp.Pages
{
    public partial class Index
    {
        private string userRegistred = "no"; //Default user registred = no
        private string loggedInUsername = ""; //Default logged in username variable
        private string Password = ""; // Default password variable
        private int caloriesNeeded; // calories needed variable
        private string Name = ""; // Username variable
        private DateTime Date = DateTime.Now; // Set Date to datetime now
        private string FoodName = "Steak"; // default first item for food log = steak
        private string Gender = "Male"; // Default gender for registering
        private int Age;
        private int Height;
        private int Weight;
        private string SelectedAmr = "1.2"; // Default amr goal
        private string SelectedGoal = "maintainWeight"; // Default weight gain/loss goal
        private User user; // user variable
        private List<Log> logs = new List<Log>(); // Logs list
        private Log newEntry = new Log { Date = DateTime.Now, FoodName = "Steak" }; // New log entry set to datetime now and foodname steak as default 
        private List<Food> foodItems = new List<Food>(); // Initialize foodItems list

        public string foodname = "";
        public double foodcalories;
        public double foodfats;
        public double foodsodium;
        public double foodcarbohydrates;
        public double foodfiber;
        public double foodsugars;
        public double foodprotein;

        protected override async Task OnInitializedAsync()
        {
            foodItems = await foodService.GetFoodItems();

        }

        private async void AddEntry()
        {
            // get the food item details from the database
            var selectedFood = foodItems.Find(food => food.Name == newEntry.FoodName);
            if (selectedFood != null && newEntry.Grams > 0)
            {
                // Calculate calories and other nutrients based on grams
                double grams = newEntry.Grams;
                newEntry.Date = newEntry.Date.Date + newEntry.Time.TimeOfDay;
                newEntry.Calories = (int)(selectedFood.Calories * (grams / 100.0));
                newEntry.Fat = (int)(selectedFood.Fat * (grams / 100.0));
                newEntry.Sodium = (int)(selectedFood.Sodium * (grams / 100.0));
                newEntry.Carbohydrates = (int)(selectedFood.Carbohydrates * (grams / 100.0));
                newEntry.Fiber = (int)(selectedFood.Fiber * (grams / 100.0));
                newEntry.Sugars = (int)(selectedFood.Sugars * (grams / 100.0));
                newEntry.Protein = (int)(selectedFood.Protein * (grams / 100.0));
            }
            else
            {
                userRegistred = "error4";
            }
            // Add the log entry to the database
            await foodService.AddLog(newEntry, loggedInUsername);
            // Refresh the logs list
            logs = await foodService.GetLogsForUser(loggedInUsername);
        }

        private (int totalCalories, int totalFat, int totalSodium, int totalCarbohydrates, int totalFiber, int totalSugars, int totalProtein) CalculateTodaysNutrients()
        {
            // Get the logs for the current user and current day
            var todayLogs = logs.Where(log => log.Date.Date == DateTime.Today);
            // Calculate total nutrients
            int totalCalories = (int)todayLogs.Sum(log => log.Calories);
            int totalFat = (int)todayLogs.Sum(log => log.Fat);
            int totalSodium = (int)todayLogs.Sum(log => log.Sodium);
            int totalCarbohydrates = (int)todayLogs.Sum(log => log.Carbohydrates);
            int totalFiber = (int)todayLogs.Sum(log => log.Fiber);
            int totalSugars = (int)todayLogs.Sum(log => log.Sugars);
            int totalProtein = (int)todayLogs.Sum(log => log.Protein);
            return (totalCalories, totalFat, totalSodium, totalCarbohydrates, totalFiber, totalSugars, totalProtein);
        }

        private async void LogIn()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Password)) //INPUT VALIDATION
            {
                userRegistred = "error";
                return;
            }

            user = await foodService.GetUser(Name);
            if (user != null && user.Password == Password)
            {
                userRegistred = "yes";
                loggedInUsername = Name;
                Calculate();
                logs = await foodService.GetLogsForUser(loggedInUsername);
            }
            else
            {
                userRegistred = "error";
            }
        }

        private void Calculate()
        {
            if (user != null)
            {
                double bmr;

                if (user.Gender == "Male")
                {
                    bmr = 66.47 + (13.75 * user.Weight) + (5.003 * user.Height) - (6.755 * user.Age); // BMR for male
                }
                else
                {
                    bmr = 655.1 + (9.563 * user.Weight) + (1.850 * user.Height) - (4.676 * user.Age); // BMR for female
                }

                caloriesNeeded = (int)(bmr * double.Parse(user.SelectedAmr) - 50); // BMR * AMR - 50

                if (user.SelectedGoal == "loseWeight")
                {
                    caloriesNeeded -= 250; // Lose weight
                }
                else if (user.SelectedGoal == "gainWeight")
                {
                    caloriesNeeded += 250; // Gain weight
                }
            }
        }

        private void GoToRegister()
        {
            userRegistred = "calculator";
        }
        

        private void GoToLogin()
        {
            userRegistred = "no";

        }

        private async void Register()
        {
            var user = await foodService.GetUser(Name); //Check existing user data
            if (user != null) // If existing user
            {
                userRegistred = "error2"; //Send to errror message number2
            }
            else if (Age <= 0 || Height <= 0 || Weight <= 0 || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Password)) //Input validation
            {
                userRegistred = "error3"; //Send to errror message number3
            }
            else
            {
                await foodService.AddUser(Name, Password, Gender, Age, Height, Weight, SelectedAmr, SelectedGoal);
                userRegistred = "yes"; //Set to user registred to "yes"
                LogIn();//Login
            }
        }

        private async void AddItem()
        {
            if (foodcalories > 0) 
            {
                await foodService.AddFoodItem(foodname, foodcalories, foodfats, foodsodium, foodcarbohydrates, foodfiber, foodsugars, foodprotein); //Add food item to db
                foodname = ""; //Reset the forms to default
                foodcalories = 0;
                foodfats = 0;
                foodsodium = 0;
                foodcarbohydrates = 0;
                foodfiber = 0;
                foodsugars = 0;
                foodprotein = 0;
                foodItems = await foodService.GetFoodItems(); // Refresh food items
            }
        }
    }
}
