using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    internal class DelayApplyConfiguration : EntityTypeConfiguration<DelayApply>
    {
        public DelayApplyConfiguration()
        {
            ToTable("tbl_DelayApply");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ConstructionTaskId)
                .IsRequired();
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ConstructionProgress)
                .IsRequired();
            Property(e => e.DelayDays)
                .IsRequired();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.OrderState)
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
