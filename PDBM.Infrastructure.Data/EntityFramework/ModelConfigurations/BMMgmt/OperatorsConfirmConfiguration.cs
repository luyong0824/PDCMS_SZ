using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// OperatorsConfirm表配置
    /// </summary>
    internal class OperatorsConfirmConfiguration : EntityTypeConfiguration<OperatorsConfirm>
    {
        public OperatorsConfirmConfiguration()
        {
            ToTable("tbl_OperatorsConfirm");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.OrderState)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
