using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// RoleUser表配置
    /// </summary>
    internal class RoleUserConfiguration : EntityTypeConfiguration<RoleUser>
    {
        public RoleUserConfiguration()
        {
            ToTable("tbl_RoleUser");
            HasKey<Guid>(e => e.Id);
            Property(e => e.RoleId)
                .IsRequired();
            Property(e => e.UserId)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
