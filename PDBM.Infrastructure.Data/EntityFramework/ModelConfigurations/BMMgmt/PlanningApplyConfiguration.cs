using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    internal class PlanningApplyConfiguration : EntityTypeConfiguration<PlanningApply>
    {
        public PlanningApplyConfiguration()
        {
            ToTable("tbl_PlanningApply");
            HasKey<Guid>(e => e.Id);
            Property(e => e.PlanningCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlanningName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.ReseauId)
                .IsRequired();
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Importance)
                .IsRequired();
            Property(e => e.DetailedAddress)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.Issued)
                .IsRequired();
            Property(e => e.PlanningUserId)
                .IsRequired();
            Property(e => e.PlanningAdvice)
                .IsRequired();
            Property(e => e.DoState)
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
