﻿using IMDB.Domain.Enums;
using System.Text.Json.Serialization;

namespace IMDB.Application.DTOs.User;

public class UserDetailsDto
{
    public long? Id { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role Role { get; set; }
}