﻿using Ninject.Modules;
using SchoolSystem.Web.Services;
using SchoolSystem.Web.Services.Contracts;

namespace SchoolSystem.WebForms.App_Start.Bindings
{
    public class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUserRolesDataService>().To<UserRolesDataService>();
        }
    }
}