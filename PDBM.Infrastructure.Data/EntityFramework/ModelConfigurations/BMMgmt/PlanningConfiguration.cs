using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// Planning表配置
    /// </summary>
    internal class PlanningConfiguration : EntityTypeConfiguration<Planning>
    {
        public PlanningConfiguration()
        {
            ToTable("tbl_Planning");
            HasKey<Guid>(e => e.Id);
            Property(e => e.PlanningCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlanningName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.PlaceCategoryId)
                .IsRequired();
            Property(e => e.ReseauId)
                .IsRequired();
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.DetailedAddress)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.ProposedNetwork)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.OptionalAddress)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.Importance)
                .IsRequired();
            Property(e => e.PlaceOwner)
                .IsRequired();
            Property(e => e.Issued)
                .IsRequired();
            Property(e => e.AddressingState)
                .IsRequired();
            Property(e => e.AddressingUserId)
                .IsOptional();
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.AddressingUserDate)
                .IsRequired();
            Property(e => e.AddressingDate)
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
