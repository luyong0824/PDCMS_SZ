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
    /// 站点类型表配置
    /// </summary>
    internal class PlaceCategoryConfiguration : EntityTypeConfiguration<PlaceCategory>
    {
        public PlaceCategoryConfiguration()
        {
            ToTable("tbl_PlaceCategory");
            HasKey<Guid>(e => e.Id);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.PlaceCategoryCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlaceCategoryName)
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
