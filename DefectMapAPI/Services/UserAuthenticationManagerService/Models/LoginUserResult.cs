﻿using DefectMapAPI.Models;

namespace DefectMapAPI.Services.UserAuthenticationManagerService.Models
{
    public class LoginUserResult
    {
        public bool Successful { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
