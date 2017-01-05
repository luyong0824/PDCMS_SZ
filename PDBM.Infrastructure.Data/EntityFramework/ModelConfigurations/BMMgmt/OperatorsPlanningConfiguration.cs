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
    /// OperatorsPlanning表配置
    /// </summary>
    internal class OperatorsPlanningConfiguration : EntityTypeConfiguration<OperatorsPlanning>
    {
        public OperatorsPlanningConfiguration()
        {
            ToTable("tbl_OperatorsPlanning");
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
            Property(e => e.AreaId)
                .IsRequired();
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.AntennaHeight)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.PoleNumber)
                .IsRequired();
            Property(e => e.CabinetNumber)
                .IsRequired();
            Property(e => e.Urgency)
                .IsRequired();
            Property(e => e.Solved)
                .IsRequired();
            Property(e => e.ToShared)
                .IsRequired();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.CompanyId)
                .IsRequired();
            Property(e => e.PlanningId)
                .IsOptional();
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
