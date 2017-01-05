using PDBM.Domain.Models.BaseData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// Customer表配置
    /// </summary>
    internal class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("tbl_Customer");
            HasKey<Guid>(e => e.Id);
            Property(e => e.CustomerType)
                .IsRequired();
            Property(e => e.CustomerCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.CustomerFullName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.CustomerUserId)
                .IsOptional();
            Property(e => e.ContactMan)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ContactTel)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ContactAddr)
                .IsRequired()
                .HasMaxLength(150);
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
