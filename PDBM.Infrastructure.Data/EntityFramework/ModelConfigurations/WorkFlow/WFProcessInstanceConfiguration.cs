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
    /// WFProcessInstance表配置
    /// </summary>
    internal class WFProcessInstanceConfiguration : EntityTypeConfiguration<WFProcessInstance>
    {
        public WFProcessInstanceConfiguration()
        {
            ToTable("tbl_WFProcessInstance");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFProcessId)
                .IsRequired();
            Property(e => e.EntityId)
                .IsRequired();
            Property(e => e.WFProcessInstanceCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.WFProcessInstanceName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.WFProcessInstanceState)
                .IsRequired();
            Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(200);
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
            Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}
