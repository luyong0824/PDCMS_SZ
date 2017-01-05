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
    /// 会计主体表配置
    /// </summary>
    internal class AccountingEntityConfiguration : EntityTypeConfiguration<AccountingEntity>
    {
        public AccountingEntityConfiguration()
        {
            ToTable("tbl_AccountingEntity");
            HasKey<Guid>(e => e.Id);
            Property(e => e.AccountingEntityCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.AccountingEntityName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
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
