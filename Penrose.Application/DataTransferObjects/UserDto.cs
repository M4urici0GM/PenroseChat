﻿using System;
using Penrose.Core.Entities;

namespace Penrose.Application.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public UserPropertiesDto Properties { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}