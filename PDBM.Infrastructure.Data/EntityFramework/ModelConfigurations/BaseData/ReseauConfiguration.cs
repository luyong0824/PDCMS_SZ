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
    /// Reseau表配置
    /// </summary>
    internal class ReseauConfiguration : EntityTypeConfiguration<Reseau>
    {
        public ReseauConfiguration()
        {
            ToTable("tbl_Reseau");
            HasKey<Guid>(e => e.Id);
            Property(e => e.AreaId)
                .IsRequired();
            Property(e => e.ReseauCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ReseauName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.ReseauManagerId)
                .IsOptional();
            Property(e => e.PlanningManagerId)
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
