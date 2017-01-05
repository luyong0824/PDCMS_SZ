using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    internal class NoticeConfiguration : EntityTypeConfiguration<Notice>
    {
        public NoticeConfiguration()
        {
            ToTable("tbl_Notice");
            HasKey<Guid>(e => e.Id);
            Property(e => e.NoticeType)
                .IsRequired();
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.NoticeContent)
                .IsRequired()
            .HasMaxLength(150);
            Property(e => e.ReceiveUserId)
                .IsRequired();
            Property(e => e.NoticeState)
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
