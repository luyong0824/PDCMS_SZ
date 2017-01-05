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
    /// CodeSeed表配置
    /// </summary>
    internal class CodeSeedConfiguration : EntityTypeConfiguration<CodeSeed>
    {
        public CodeSeedConfiguration()
        {
            ToTable("tbl_CodeSeed");
            HasKey<Guid>(e => e.Id);
            Property(e => e.EntityName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            Property(e => e.Digit)
                .IsRequired();
            Property(e => e.Prefix)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            Property(e => e.Seed)
                .IsRequired();
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
