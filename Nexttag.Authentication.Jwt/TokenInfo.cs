﻿using System;

namespace Nexttag.Authentication.Jwt
{
    /// <summary>
    /// Objetos para montagem do token
    /// </summary>
    public class TokenInfo
    {
        public string Id { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string Application { get; set; }
        public string[] Roles { get; set; }
        public string[] Permissions { get; set; }
    }
}