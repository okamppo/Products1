using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace Products1
{
    [DataTable("DeviceUser")]
	public class DeviceUser
	{
        [JsonProperty(PropertyName = "id")]        
        public int Id { get; set; }

        [JsonProperty]
        public int DeviceUserID {get; set;}

        [JsonProperty]
        public string NickName {get; set;}

        //[JsonIgnore]
		public string FirstName {get; set;}

        //[JsonIgnore]
        public string LastName {get; set;}

        [JsonProperty]
		public string Password {get; set;}

        [Version]
        public string Version { get; set; }
    }
}

