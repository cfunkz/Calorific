using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CalorificServerApp.Data
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Sodium { get; set; }
        public double Carbohydrates { get; set; }
        public double Fiber { get; set; }
        public double Sugars { get; set; }
        public double Protein { get; set; }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string SelectedAmr { get; set; }
        public string SelectedGoal { get; set; }
    }
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string FoodName { get; set; }
        public int Grams { get; set; }
        public double Calories { get; set; }
        public double Fat { get; set; }
        public double Sodium { get; set; }
        public double Carbohydrates { get; set; }
        public double Fiber { get; set; }
        public double Sugars { get; set; }
        public double Protein { get; set; }
    }
}