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
    /// DutyUser表配置
    /// </summary>
    internal class DutyUserConfiguration : EntityTypeConfiguration<DutyUser>
    {
        public DutyUserConfiguration()
        {
            ToTable("tbl_DutyUser");
            HasKey<Guid>(e => e.Id);
            Property(e => e.Duty)
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
