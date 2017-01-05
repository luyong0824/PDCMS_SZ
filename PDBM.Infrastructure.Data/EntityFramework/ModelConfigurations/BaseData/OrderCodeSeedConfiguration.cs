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
    /// OrderCodeSeed表配置
    /// </summary>
    internal class OrderCodeSeedConfiguration : EntityTypeConfiguration<OrderCodeSeed>
    {
        public OrderCodeSeedConfiguration()
        {
            ToTable("tbl_OrderCodeSeed");
            HasKey<Guid>(e => e.Id);
            Property(e => e.Seed)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(5);
        }
    }
}
