using PDBM.Domain.Models.BaseData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// WorkSmallClass表配置
    /// </summary>
    internal class WorkSmallClassConfiguration : EntityTypeConfiguration<WorkSmallClass>
    {
        public WorkSmallClassConfiguration()
        {
            ToTable("tbl_WorkSmallClass");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WorkBigClassId)
                .IsRequired();
            Property(e => e.SmallClassCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.SmallClassName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.State)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.ModifyUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
            Property(e => e.ModifyDate)
                .IsRequired();
        }
    }
}
