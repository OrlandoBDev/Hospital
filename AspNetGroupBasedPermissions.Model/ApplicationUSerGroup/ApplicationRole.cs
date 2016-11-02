﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetGroupBasedPermissions.Model.ApplicationUSerGroup
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }


        public ApplicationRole(string name, string description) : base(name)
        {
            this.Description = description;
        }
        public virtual string Description { get; set; }
    }
}