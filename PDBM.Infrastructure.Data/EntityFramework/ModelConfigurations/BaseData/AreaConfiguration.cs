﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// Area表配置
    /// </summary>
    internal class AreaConfiguration : EntityTypeConfiguration<Area>
    {
        public AreaConfiguration()
        {
            ToTable("tbl_Area");
            HasKey<Guid>(e => e.Id);
            Property(e => e.AreaCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.AreaName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.AreaManagerId)
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