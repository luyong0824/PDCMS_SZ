using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// 项目表配置
    /// </summary>
    internal class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            ToTable("tbl_Project");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ProjectName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.ProjectFullName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.ProjectCategory)
                .IsRequired();
            Property(e => e.AccountingEntityId)
                .IsOptional();
            Property(e => e.ManagerUserId)
                .IsOptional();
            Property(e => e.ResponsibleUserId)
                .IsOptional();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.ResponsibleUserId)
                .IsOptional();
            Property(e => e.State)
                .IsRequired();
            Property(e => e.ProfessionList)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            Property(e => e.BudgetPrice)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.ProjectApplyDate)
                .IsRequired();
            Property(e => e.ProjectDoApplyDate)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.ModifyUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
            Property(e => e.ModifyDate)
                .IsRequired();
            Property(e => e.FinishDate)
                .IsRequired();
        }
    }
}
