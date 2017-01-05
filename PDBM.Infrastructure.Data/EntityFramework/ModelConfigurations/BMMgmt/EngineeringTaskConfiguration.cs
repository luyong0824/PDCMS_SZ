using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// EngineeringTask表配置
    /// </summary>
    internal class EngineeringTaskConfiguration : EntityTypeConfiguration<EngineeringTask>
    {
        public EngineeringTaskConfiguration()
        {
            ToTable("tbl_EngineeringTask");
            HasKey<Guid>(e => e.Id);
            Property(e => e.TaskModel)
                .IsRequired();
            Property(e => e.ProjectTaskId)
                .IsRequired();
            Property(e => e.DesignCustomerId)
                .IsRequired();
            Property(e => e.ConstructionCustomerId)
                .IsRequired();
            Property(e => e.SupervisionCustomerId)
                .IsRequired();
            Property(e => e.ProjectManagerId)
                .IsRequired();
            Property(e => e.DesignMemos)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.DesignRealName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.DesignDate)
                .IsRequired();
            Property(e => e.DesignState)
                .IsRequired();
            Property(e => e.EngineeringProgress)
                .IsRequired();
            Property(e => e.ProgressMemos)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.ProgressDate)
                .IsRequired();
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
