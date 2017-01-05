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
    /// WorkApply表配置
    /// </summary>
    internal class WorkApplyConfiguration : EntityTypeConfiguration<WorkApply>
    {
        public WorkApplyConfiguration()
        {
            ToTable("tbl_WorkApply");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.CustomerId)
                .IsRequired();
            Property(e => e.ReseauId)
                .IsRequired();
            Property(e => e.ReseauManagerId)
                .IsRequired();
            Property(e => e.ApplyReason)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.IsSoved)
                .IsRequired();
            Property(e => e.WorkOrderId)
                .IsOptional();
            Property(e => e.OrderState)
                .IsRequired();
            Property(e => e.SceneContactMan)
                .IsOptional()
                .HasMaxLength(50);
            Property(e => e.SceneContactTel)
                .IsOptional()
                .HasMaxLength(50);
            Property(e => e.ReturnReason)
                .IsOptional()
                .HasMaxLength(500);
            Property(e => e.ProjectCode)
                .IsOptional()
                .HasMaxLength(50);
            Property(e => e.IsProject)
                .IsOptional();
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
