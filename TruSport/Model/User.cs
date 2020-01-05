using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace TruSport.Model
{
    [Table("Users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserTypeID { get; set; }
        public bool isLoggedIn { get; set; }
    }

    //public class User
    //{
    //    string id;
    //    string firstName;
    //    string lastName;
    //    string email;
    //    string password;
    //    string userTypeID;
    //    bool isloggedIn;

    //    [PrimaryKey]
    //    [JsonProperty(PropertyName = "id")]
    //    public string ID
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    [JsonProperty(PropertyName = "userTypeID")]
    //    public string UserTypeID
    //    {
    //        get { return userTypeID; }
    //        set { userTypeID = value; }
    //    }

    //    [JsonProperty(PropertyName = "firstName")]
    //    public string FirstName
    //    {
    //        get { return firstName; }
    //        set { firstName = value; }
    //    }

    //    [JsonProperty(PropertyName = "lastName")]
    //    public string LastName
    //    {
    //        get { return lastName; }
    //        set { lastName = value; }
    //    }

    //    [JsonProperty(PropertyName = "email")]
    //    public string Email
    //    {
    //        get { return email; }
    //        set { email = value; }
    //    }

    //    [JsonProperty(PropertyName = "isLoggedIn")]
    //    public bool IsLoggedIn
    //    {
    //        get { return isloggedIn; }
    //        set { isloggedIn = value; }
    //    }

    //    [Version]
    //    public string Version { get; set; }
    //}

    public class UserListView
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserTypeID { get; set; }
        public string UserType { get; set; }
        public bool isLoggedIn { get; set; }
    }

    public class UserRequest
    {
        public string ID { get; set; }
        public string UserTypeID { get; set; }
        public string TeamID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public string ID { get; set; }
        public string UserTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TeamID { get; set; }
        public bool IsValidated { get; set; }
        public string Token { get; set; }

        [Ignore]
        public UserType UserType { get; set; }

        [Ignore]
        public Team Team { get; set; }
    }

    public class UserAuthentication
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class PasswordReset
    {
        public string Email { get; set; }
        public string TemporaryPassword { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPassword
    {
        public string Email { get; set; }
    }
}
