using PDBM.Domain.Models.BaseData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    internal class MaterialCategoryConfiguration : EntityTypeConfiguration<MaterialCategory>
    {
        public MaterialCategoryConfiguration()
        {
            ToTable("tbl_MaterialCategory");
            HasKey<Guid>(e => e.Id);
            Property(e => e.MaterialCategoryCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.MaterialCategoryName)
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
