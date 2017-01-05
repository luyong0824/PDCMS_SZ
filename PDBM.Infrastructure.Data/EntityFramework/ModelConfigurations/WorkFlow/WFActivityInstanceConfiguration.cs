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
    /// WFActivityInstance表配置
    /// </summary>
    internal class WFActivityInstanceConfiguration : EntityTypeConfiguration<WFActivityInstance>
    {
        public WFActivityInstanceConfiguration()
        {
            ToTable("tbl_WFActivityInstance");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFProcessInstanceId)
                .IsRequired();
            Property(e => e.WFActivityInstanceName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.WFActivityInstanceState)
                .IsRequired();
            Property(e => e.WFActivityInstanceFlow)
                .IsRequired();
            Property(e => e.WFActivityInstanceResult)
                .IsRequired();
            Property(e => e.WFActivityOperate)
                .IsRequired();
            Property(e => e.WFActivityEditorId)
                .IsOptional();
            Property(e => e.IsMustEdit)
                .IsRequired();
            Property(e => e.WFActivityOrder)
                .IsRequired();
            Property(e => e.SerialId)
                .IsRequired();
            Property(e => e.RowId)
                .IsRequired();
            Property(e => e.Timelimit)
                .IsRequired();
            Property(e => e.UserId)
                .IsRequired();
            Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(200);
            Property(e => e.ReceivedDate)
                .IsRequired();
            Property(e => e.OperateDate)
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
