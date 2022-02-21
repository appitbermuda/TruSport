using System;
using Microsoft.WindowsAzure.MobileServices;
//using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace TruSport.Model
{
    [Table("UserTypes")]
    public class UserTypes
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        public string Name { get; set; }

    }

    public class UserType
    {
        string id;
        string name;

        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
