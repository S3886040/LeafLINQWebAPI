﻿namespace LeafLINQWebAPI.DTOs;

public class TokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string SessionId { get; set; }
}
