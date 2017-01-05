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
    /// WFActivity表配置
    /// </summary>
    internal class WFActivityConfiguration : EntityTypeConfiguration<WFActivity>
    {
        public WFActivityConfiguration()
        {
            ToTable("tbl_WFActivity");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFProcessId)
                .IsRequired();
            Property(e => e.WFActivityName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.WFActivityOperate)
                .IsRequired();
            Property(e => e.WFActivityEditorId)
                .IsOptional();
            Property(e => e.WFActivityOrder)
                .IsRequired();
            Property(e => e.SerialId)
                .IsRequired();
            Property(e => e.RowId)
                .IsRequired();
            Property(e => e.Timelimit)
                .IsRequired();
            Property(e => e.CompanyId)
                .IsRequired();
            Property(e => e.DepartmentId)
                .IsOptional();
            Property(e => e.UserId)
                .IsOptional();
            Property(e => e.PostId)
                .IsOptional();
            Property(e => e.IsNecessaryStep)
                .IsRequired();
            Property(e => e.IsMustEdit)
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
