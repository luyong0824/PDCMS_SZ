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
    /// MaterialSpec表配置
    /// </summary>
    internal class MaterialSpecConfiguration : EntityTypeConfiguration<MaterialSpec>
    {
        public MaterialSpecConfiguration()
        {
            ToTable("tbl_MaterialSpec");
            HasKey<Guid>(e => e.Id);
            Property(e => e.MaterialSpecCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.MaterialSpecName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.MaterialId)
                .IsRequired();
            Property(e => e.UnitId)
                .IsRequired();
            Property(e => e.Price)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.CustomerId)
                .IsOptional();
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
