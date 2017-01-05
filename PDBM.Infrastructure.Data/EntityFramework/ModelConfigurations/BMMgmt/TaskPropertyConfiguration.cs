using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    internal class TaskPropertyConfiguration : EntityTypeConfiguration<TaskProperty>
    {
        public TaskPropertyConfiguration()
        {
            ToTable("tbl_TaskProperty");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ConstructionTaskId)
                .IsRequired();
            Property(e => e.TaskModel)
                .IsRequired();
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.ConstructionCustomerId)
                .IsRequired();
            Property(e => e.SupervisorCustomerId)
                .IsRequired();
            Property(e => e.ConstructionProgress)
                .IsRequired();
            Property(e => e.ProgressMemos)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.ProgressUserId)
                .IsOptional();
            Property(e => e.ProgressModifyDate)
                .IsRequired();
            Property(e => e.SubmitState)
                .IsRequired();
            Property(e => e.SubmitUserId)
                .IsOptional();
            Property(e => e.SubmitModifyDate)
                .IsRequired();
            Property(e => e.TimeLimit)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
