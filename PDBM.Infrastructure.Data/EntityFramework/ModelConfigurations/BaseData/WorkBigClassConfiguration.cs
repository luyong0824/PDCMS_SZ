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
    /// WorkBigClass表配置
    /// </summary>
    internal class WorkBigClassConfiguration : EntityTypeConfiguration<WorkBigClass>
    {
        public WorkBigClassConfiguration()
        {
            ToTable("tbl_WorkBigClass");
            HasKey<Guid>(e => e.Id);
            Property(e => e.BigClassCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.BigClassName)
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
