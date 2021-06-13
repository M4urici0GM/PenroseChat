using System;

namespace Penrose.Application.DataTransferObjects
{
    public class UserPropertiesDto
    {
        public string PhotoUrl { get; set; }
        public string Status { get; set; }
        public DateTime LastSeen { get; set; }
    }
}