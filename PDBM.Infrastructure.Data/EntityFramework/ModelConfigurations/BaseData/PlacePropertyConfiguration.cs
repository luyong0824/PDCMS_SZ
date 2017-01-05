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
    /// tbl_PlaceProperty表配置
    /// </summary>
    internal class PlacePropertyConfiguration : EntityTypeConfiguration<PlaceProperty>
    {
        public PlacePropertyConfiguration()
        {
            ToTable("tbl_PlaceProperty");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.PropertyType)
                .IsRequired();
            Property(e => e.MobileShare)
                .IsRequired();
            Property(e => e.MobilePoleNumber)
                .IsRequired();
            Property(e => e.MobileCabinetNumber)
                .IsRequired();
            Property(e => e.MobilePowerUsed)
                .IsRequired();
            Property(e => e.MobileCreateUserId)
                .IsOptional();
            Property(e => e.MobileCreateDate)
                .IsRequired();
            Property(e => e.TelecomShare)
                .IsRequired();
            Property(e => e.TelecomPoleNumber)
                .IsRequired();
            Property(e => e.TelecomCabinetNumber)
                .IsRequired();
            Property(e => e.TelecomPowerUsed)
                .IsRequired();
            Property(e => e.TelecomCreateUserId)
                .IsOptional();
            Property(e => e.TelecomCreateDate)
                .IsRequired();
            Property(e => e.UnicomShare)
                .IsRequired();
            Property(e => e.UnicomPoleNumber)
                .IsRequired();
            Property(e => e.UnicomCabinetNumber)
                .IsRequired();
            Property(e => e.UnicomPowerUsed)
                .IsRequired();
            Property(e => e.UnicomCreateUserId)
                .IsOptional();
            Property(e => e.UnicomCreateDate)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
