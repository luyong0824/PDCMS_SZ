using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    internal class ConstructionTaskConfiguration : EntityTypeConfiguration<ConstructionTask>
    {
        public ConstructionTaskConfiguration()
        {
            ToTable("tbl_ConstructionTask");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ConstructionMethod)
                .IsRequired();
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.ProjectId)
                .IsOptional();
            Property(e => e.ProjectManagerId)
                .IsRequired();
            Property(e => e.SupervisorCustomerId)
                .IsRequired();
            Property(e => e.SupervisorUserId)
                .IsRequired();
            Property(e => e.ConstructionProgress)
                .IsRequired();
            Property(e => e.ProgressMemos)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.RequestState)
                .IsRequired();
            Property(e => e.IsApply)
                .IsRequired();
            Property(e => e.IsFinishMobile)
                .IsRequired();
            Property(e => e.IsFinishTelecom)
                .IsRequired();
            Property(e => e.IsFinishUnicom)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
            Property(e => e.ModifyDate)
                .IsRequired();
        }
    }
}
