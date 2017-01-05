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
    /// ProjectTask表配置
    /// </summary>
    internal class ProjectTaskConfiguration : EntityTypeConfiguration<ProjectTask>
    {
        public ProjectTaskConfiguration()
        {
            ToTable("tbl_ProjectTask");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ProjectType)
                .IsRequired();
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.AreaManagerId)
                .IsRequired();
            Property(e => e.GeneralDesignId)
                .IsRequired();
            Property(e => e.DesignRealName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.DesignDate)
                .IsRequired();
            Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ProjectProgress)
                .IsRequired();
            Property(e => e.ProgressMemos)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.ProjectDate)
                .IsRequired();
            Property(e => e.ProjectBeginDate)
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
