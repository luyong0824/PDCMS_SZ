using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.WorkFlow
{
    /// <summary>
    /// WFProcess表配置
    /// </summary>
    internal class WFProcessConfiguration : EntityTypeConfiguration<WFProcess>
    {
        public WFProcessConfiguration()
        {
            ToTable("tbl_WFProcess");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFCategoryId)
                .IsRequired();
            Property(e => e.WFProcessCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.WFProcessName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.IsApprovedByManager)
                .IsRequired();
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
