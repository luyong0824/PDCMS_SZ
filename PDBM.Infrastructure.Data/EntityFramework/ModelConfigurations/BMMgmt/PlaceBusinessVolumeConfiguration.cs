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
    /// PlaceBusinessVolume表配置
    /// </summary>
    internal class PlaceBusinessVolumeConfiguration : EntityTypeConfiguration<PlaceBusinessVolume>
    {
        public PlaceBusinessVolumeConfiguration()
        {
            ToTable("tbl_PlaceBusinessVolume");
            HasKey<Guid>(e => e.Id);
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.G2BusinessVolumeId)
                .IsRequired();
            Property(e => e.D2BusinessVolumeId)
                .IsRequired();
            Property(e => e.G3BusinessVolumeId)
                .IsRequired();
            Property(e => e.G4BusinessVolumeId)
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
