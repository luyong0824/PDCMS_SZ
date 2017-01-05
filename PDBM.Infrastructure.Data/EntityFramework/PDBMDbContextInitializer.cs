using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Utils;

namespace PDBM.Infrastructure.Data.EntityFramework
{
    /// <summary>
    /// PDBM数据库上下文初始化器
    /// </summary>
    public sealed class PDBMDbContextInitializer : DropCreateDatabaseIfModelChanges<PDBMDbContext>
    {
        protected override void Seed(PDBMDbContext context)
        {
            if (FileHelper.IsExistFile("PDBM_DB_Index.sql"))
            {
                context.Database.ExecuteSqlCommand(FileHelper.FileToString("PDBM_DB_Index.sql"));
            }
            if (FileHelper.IsExistFile("PDBM_DB_Init.sql"))
            {
                context.Database.ExecuteSqlCommand(FileHelper.FileToString("PDBM_DB_Init.sql"));
            }
            if (FileHelper.IsExistFile("PDBM_DB_Func.sql"))
            {
                context.Database.ExecuteSqlCommand(FileHelper.FileToString("PDBM_DB_Func.sql"));
            }
            base.Seed(context);
        }

        public static void Initialize()
        {
            //Database.SetInitializer<PDBMDbContext>(new PDBMDbContextInitializer());
            Database.SetInitializer<PDBMDbContext>(null);
        }
    }
}
