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
    /// 周边场景表配置
    /// </summary>
    internal class SceneConfiguration : EntityTypeConfiguration<Scene>
    {
        public SceneConfiguration()
        {
            ToTable("tbl_Scene");
            HasKey<Guid>(e => e.Id);
            Property(e => e.SceneCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.SceneName)
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
