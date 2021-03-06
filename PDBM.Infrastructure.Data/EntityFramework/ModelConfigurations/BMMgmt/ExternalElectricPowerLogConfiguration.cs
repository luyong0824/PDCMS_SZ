﻿using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// ExternalElectricPowerLog表配置
    /// </summary>
    internal class ExternalElectricPowerLogConfiguration : EntityTypeConfiguration<ExternalElectricPowerLog>
    {
        public ExternalElectricPowerLogConfiguration()
        {
            ToTable("tbl_ExternalElectricPowerLog");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OperationType)
                .IsRequired();
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.PropertyType)
                .IsRequired();
            Property(e => e.ExternalElectric)
                .IsRequired();
            Property(e => e.BudgetPrice)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.CustomerId)
                .IsOptional();
            Property(e => e.CustomerUserId)
                .IsOptional();
            Property(e => e.TimeLimit)
                .IsRequired();
            Property(e => e.Memos)
                .IsRequired()
                .HasMaxLength(500);
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
