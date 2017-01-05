using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    internal class PlanningApplyHeaderConfiguration : EntityTypeConfiguration<PlanningApplyHeader>
    {
        public PlanningApplyHeaderConfiguration()
        {
            ToTable("tbl_PlanningApplyHeader");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(150);
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
