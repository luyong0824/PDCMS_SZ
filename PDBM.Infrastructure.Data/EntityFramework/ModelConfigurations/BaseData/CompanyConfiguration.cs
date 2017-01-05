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
    /// Company表配置
    /// </summary>
    internal class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            ToTable("tbl_Company");
            HasKey<Guid>(e => e.Id);
            Property(e => e.CompanyCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.CompanyFullName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.ApplyCodePrefix)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            Property(e => e.CompanyNature)
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
