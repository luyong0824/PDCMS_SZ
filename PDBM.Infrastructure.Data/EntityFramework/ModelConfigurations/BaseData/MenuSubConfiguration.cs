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
    /// MenuSub表配置
    /// </summary>
    internal class MenuSubConfiguration : EntityTypeConfiguration<MenuSub>
    {
        public MenuSubConfiguration()
        {
            ToTable("tbl_MenuSub");
            HasKey<Guid>(e => e.Id);
            Property(e => e.MenuId)
                .IsRequired();
            Property(e => e.MenuSubName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.IndexId)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
