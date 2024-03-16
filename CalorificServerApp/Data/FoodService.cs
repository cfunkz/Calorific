@page "/"
@using CalorificServerApp.Data
@inject FoodService foodService

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-12">
            @if (userRegistred == "no")
            {
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Login</h5>
                        <div class="mb-3">
                            <input type="text" placeholder="Enter Name" @bind="Name" class="form-control" />
                        </div>
                        <button @onclick="FindUser" class="btn btn-primary">Login</button>
                        <button @onclick="GoToRegister" class="btn btn-secondary">Register</button>
                    </div>
                </div>
            }
            else if (userRegistred == "yes")
            {
                <div class="text-center">
                    <h3>Hello, @loggedInUsername !</h3>
                    <hr />
                     <h3>Todays Summary</h3>
                 </div>
                <div class="container mt-4">
                    <div class="row">
                        <!-- Calorie Goal Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-primary text-white">
                                    Calorie Goal <strong>(kcal)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalCalories</span> / <span class="text-danger">@caloriesNeeded</span>
                                    </h5>
                                    <p class="card-text">Keep track of your calorie intake!</p>
                                </div>
                            </div>
                        </div>

                        <!-- Weight Goal Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-primary text-white">
                                    Current Weight <strong>(KG)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-info">@user.Weight</span>
                                    </h5>
                                    <p class="card-text">Achieve your weight goal.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Nutrient Intake Cards -->
                <div class="container mt-4">
                    <div class="row">
                        <!-- Fat Intake Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-success text-white">
                                    Fat <strong>(g)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalFat</span>
                                    </h5>
                                </div>
                            </div>
                        </div>

                        <!-- Sodium Intake Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-success text-white">
                                    Sodium <strong>(mg)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalSodium</span>
                                    </h5>
                                </div>
                            </div>
                        </div>

                        <!-- Carbohydrates Intake Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-success text-white">
                                    Carbs <strong>(g)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalCarbohydrates</span>
                                    </h5>
                                </div>
                            </div>
                        </div>

                        <!-- Fiber Intake Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-success text-white">
                                    Fiber <strong>(g)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalFiber</span>
                                    </h5>
                                </div>
                            </div>
                        </div>

                        <!-- Sugar Intake Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-success text-white">
                                    Sugar <strong>(g)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalSugars</span>
                                    </h5>
                                </div>
                            </div>
                        </div>

                        <!-- Sugar Intake Card -->
                        <div class="col">
                            <div class="card">
                                <div class="card-header text-center bg-success text-white">
                                    Protein <strong>(g)</strong>
                                </div>
                                <div class="card-body text-center">
                                    <h5 class="card-title">
                                        <span class="text-success">@CalculateTodaysNutrients().totalProtein</span>
                                    </h5>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <!-- Display Logs -->
                <div class="table-responsive">
                    <table class="table table-striped table-bordered mt-4">
                        <thead class="thead-dark">
                            <tr>
                                <th scope="col">Date</th>
                                <th scope="col">Food Name</th>
                                <th scope="col">Grams</th>
                                <th scope="col">Calories <strong>(kcal)</strong></th>
                                <th scope="col">Fat <strong>(g)</strong></th>
                                <th scope="col">Sodium <strong>(mg)</strong></th>
                                <th scope="col">Carbohydrates <strong>(g)</strong></th>
                                <th scope="col">Fiber <strong>(g)</strong></th>
                                <th scope="col">Sugars <strong>(g)</strong></th>
                                <th scope="col">Protein <strong>(g)</strong></th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var entry in logs)
                            {
                                <tr>
                                    <td>@entry.Date.ToString("yyyy-MM-dd HH:mm")</td>
                                    <td>@entry.FoodName</td>
                                    <td>@entry.Grams</td>
                                    <td>@entry.Calories</td>
                                    <td>@entry.Fat</td>
                                    <td>@entry.Sodium</td>
                                    <td>@entry.Carbohydrates</td>
                                    <td>@entry.Fiber</td>
                                    <td>@entry.Sugars</td>
                                    <td>@entry.Protein</td>
                                </tr>
                            }
                        </tbody>
                    </table>
                </div>

                <!-- CALORIE INTAKE ENTRY INPUT -->
                <div class="card mb-4">
                    <div class="card-body">
                        <h4 class="card-title">Add New Entry</h4>

                        <div class="form-row">
                            <div class="mb-3">
                                <input type="date" @bind="newEntry.Date" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <select @bind="newEntry.FoodName" class="form-select">
                                    @foreach (var food in foodItems)
                                    {
                                        <option value="@food.Name">@food.Name</option>
                                    }
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="grams" class="form-label">Grams:</label>
                                <input type="text" @bind="newEntry.Grams" class="form-control" maxlength="5" required />
                            </div>
                        </div>

                        <button style="margin-top:10px;" class="btn btn-primary" @onclick="AddLog">Add Log</button>
                    </div>
                </div>

                <!-- Add New Food -->
                <div class="card mb-4 mx-auto">
                    <div class="card-body">
                        <h4 class="card-title">Add New Food</h4>

                        <div class="form-row">
                            <div class="mb-3">
                                <label for="foodname">Food Name</label>
                                <input type="text" id="foodname" @bind="foodname" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodcalories">Calories (per 100g)</label>
                                <input type="text" id="foodcalories" @bind="foodcalories" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodcalories">Fats (per 100g)</label>
                                <input type="text" id="foodfats" @bind="foodfats" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodsodium">Sodium (per 100g)</label>
                                <input type="text" id="foodsodium" @bind="foodsodium" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodcarbohydrates">Carbohydrates (per 100g)</label>
                                <input type="text" id="foodcarbohydrates" @bind="foodcarbohydrates" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodfiber">Fiber (per 100g)</label>
                                <input type="text" id="foodfiber" @bind="foodfiber" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodsugars">Sugars (per 100g)</label>
                                <input type="text" id="foodsugars" @bind="foodsugars" class="form-control" required />
                            </div>
                            <div class="mb-3">
                                <label for="foodprotein">Protein (per 100g)</label>
                                <input type="text" id="foodprotein" @bind="foodprotein" class="form-control" required />
                            </div>
                        </div>

                        <button style="margin-top:10px;" @onclick="AddFoodItem" class="btn btn-primary">Save Food</button>
                    </div>
                </div>
            }
            else if (userRegistred == "error")
            {
                <div class="alert alert-danger" role="alert">
                    User not found. Please check the entered name.
                    <button @onclick="GoToLogin" class="btn btn-secondary">Back To Login</button>
                </div>
            }
            else
            {
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Register</h5>
                        <div class="mb-3">
                            <input type="text" placeholder="Enter Name" @bind="Name" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <select id="gender" @bind="Gender" class="form-select">
                                <option value="Male">Male</option>
                                <option value="Female">Female</option>
                            </select>
                        </div>
                        <div class="row mb-3">
                            <div class="col">
                                <label>Age (years)</label>
                                <input type="number" @bind="Age" class="form-control" />
                            </div>
                            <div class="col">
                                <label>Height (cm)</label>
                                <input type="number" @bind="Height" class="form-control" />
                            </div>
                            <div class="col">
                                <label>Weight (kg)</label>
                                <input type="number" @bind="Weight" class="form-control" />
                            </div>
                        </div>
                        <div class="mb-3">
                            <select id="amr" @bind="SelectedAmr" class="form-select">
                                <option value="1.2">Sedentary (None)</option>
                                <option value="1.375">Light Activity (Low)</option>
                                <option value="1.55">Moderate Activity (Medium)</option>
                                <option value="1.725">Active (Active)</option>
                                <option value="1.9">Very Active (Very Active)</option>
                            </select>
                        </div>
                        <div class="mb-3">
                            <select id="goal" @bind="SelectedGoal" class="form-select">
                                <option value="loseWeight">Loss</option>
                                <option value="gainWeight">Gain</option>
                                <option value="maintainWeight">Maintain</option>
                            </select>
                        </div>
                        <button @onclick="Register" class="btn btn-primary">Register</button>
                    </div>
                </div>
            }
        </div>
    </div>
</div>

@code {
    private string userRegistred = "no";
    private string loggedInUsername;
    private int caloriesNeeded;
    private string Name;
    private string Gender = "Male";
    private int Age;
    private int Height;
    private int Weight;
    private string SelectedAmr = "1.2";
    private string SelectedGoal = "maintainWeight";
    private User user;
    private List<Log> logs = new List<Log>();
    private Log newEntry = new Log { Date = DateTime.Now, FoodName = "Steak" };
    private List<Food> foodItems = new List<Food>(); // Initialize foodItems list

    public string foodname;
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

    private async void AddLog()
    {
        // get the food item details from the database
        var selectedFood = foodItems.FirstOrDefault(food => food.Name == newEntry.FoodName);
        if (selectedFood != null)
        {
            // Calculate calories and other nutrients based on grams
            double grams = newEntry.Grams;
            newEntry.Calories = (int)(selectedFood.Calories * (grams / 100.0));
            newEntry.Fat = (int)(selectedFood.Fat * (grams / 100.0));
            newEntry.Sodium = (int)(selectedFood.Sodium * (grams / 100.0));
            newEntry.Carbohydrates = (int)(selectedFood.Carbohydrates * (grams / 100.0));
            newEntry.Fiber = (int)(selectedFood.Fiber * (grams / 100.0));
            newEntry.Sugars = (int)(selectedFood.Sugars * (grams / 100.0));
            newEntry.Protein = (int)(selectedFood.Protein * (grams / 100.0));
        }

        // Add the log entry to the database
        await foodService.AddLog(newEntry, loggedInUsername);
        // Refresh the logs list
        logs = await foodService.GetLogsForUser(loggedInUsername);
        // Reset the newEntry
        newEntry = new Log { Date = DateTime.Now, FoodName = "Steak" };
    }

    private (int totalCalories, int totalFat, int totalSodium, int totalCarbohydrates, int totalFiber, int totalSugars, int totalProtein) CalculateTodaysNutrients()
    {
        // Get the logs for the current user and current day
        var todayLogs = logs.Where(log => log.Date.Date == DateTime.Now);
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

    private async void FindUser()
    {
        user = await foodService.GetUser(Name);
        if (user != null)
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
        await foodService.AddUser(Name, Gender, Age, Height, Weight, SelectedAmr, SelectedGoal);
        userRegistred = "yes";
        FindUser();
    }

    private async void AddFoodItem()
    {
        await foodService.AddFoodItem(foodname, foodcalories, foodfats, foodsodium, foodcarbohydrates, foodfiber, foodsugars, foodprotein);
        foodname = "Steak";
        foodcalories = 0;
        foodfats = 0;
        foodsodium = 0;
        foodcarbohydrates = 0;
        foodfiber= 0;
        foodsugars = 0;
        foodprotein = 0;
        foodItems = await foodService.GetFoodItems();
    }
}
