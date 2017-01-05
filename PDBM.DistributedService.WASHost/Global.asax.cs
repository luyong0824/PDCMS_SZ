using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using PDBM.Infrastructure.Data.EntityFramework;
using PDBM.Infrastructure.Utils;

namespace PDBM.DistributedService.WASHost
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            LogHelper.InitConfigure();
            PDBMDbContextInitializer.Initialize();
        }
    }
}