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
    /// Unit表配置
    /// </summary>
    internal class UnitConfiguration : EntityTypeConfiguration<Unit>
    {
        public UnitConfiguration()
        {
            ToTable("tbl_Unit");
            HasKey<Guid>(e => e.Id);
            Property(e => e.UnitName)
                .IsRequired()
                .HasMaxLength(100);
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
