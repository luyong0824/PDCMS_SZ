using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// BusinessVolume表配置
    /// </summary>
    internal class BusinessVolumeConfiguration : EntityTypeConfiguration<BusinessVolume>
    {
        public BusinessVolumeConfiguration()
        {
            ToTable("tbl_BusinessVolume");
            HasKey<Guid>(e => e.Id);
            Property(e => e.LogicalType)
                .IsRequired();
            Property(e => e.LogicalNumber)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.TrafficVolumes)
                .IsRequired()
                .HasPrecision(18, 3);
            Property(e => e.BusinessVolumes)
                .IsRequired()
                .HasPrecision(18, 3);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.CompanyId)
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
