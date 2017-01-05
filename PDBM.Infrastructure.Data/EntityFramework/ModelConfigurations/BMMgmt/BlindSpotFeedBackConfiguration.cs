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
    /// BlindSpotFeedBack表配置
    /// </summary>
    internal class BlindSpotFeedBackConfiguration : EntityTypeConfiguration<BlindSpotFeedBack>
    {
        public BlindSpotFeedBackConfiguration()
        {
            ToTable("tbl_BlindSpotFeedBack");
            HasKey<Guid>(e => e.Id);
            Property(e => e.PlaceName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.AreaId)
                .IsRequired();
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.FeedBackContent)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.DoUserId)
                .IsRequired();
            Property(e => e.DoState)
                .IsRequired();
            Property(e => e.FeedBackResult)
                .IsRequired()
                .HasMaxLength(500);
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
