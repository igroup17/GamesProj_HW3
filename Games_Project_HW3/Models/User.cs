//using HW1.DAL;
using System.Runtime.InteropServices;

namespace HW1.Models
{
    public class User
    {

        int id;
        string name;
        string email;
        string password;

        public User() { }
        public User(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

       

        
        public int Insert() 
        {
            Console.WriteLine($"Inserting User: {Id},{Name}, {Email}, {Password}");
            DBservices dbs = new DBservices();
            return dbs.InsertUser(this);
        }


        public int DeleteGameByUser(int gameID, int userID)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteGame(gameID, userID);
        }

        public User LogIn()
        {
            DBservices dbs = new DBservices();
            return dbs.LogInUserCheck(this); //returns User object or null if the details incorrect
        }

        public User UpdateUserRequest()
        {
            Console.WriteLine($"update User: {Id},{Name}, {Email}, {Password}");

            DBservices dbs = new DBservices();
            return dbs.UpdateUser(this);
        }
    }
}

