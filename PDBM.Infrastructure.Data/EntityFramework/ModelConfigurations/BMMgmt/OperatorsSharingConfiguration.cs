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
    /// OperatorsSharing表配置
    /// </summary>
    internal class OperatorsSharingConfiguration : EntityTypeConfiguration<OperatorsSharing>
    {
        public OperatorsSharingConfiguration()
        {
            ToTable("tbl_OperatorsSharing");
            HasKey<Guid>(e => e.Id);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.PlaceCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.PowerUsed)
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
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.CompanyId)
                .IsRequired();
            Property(e => e.RemodelingId)
                .IsOptional();
            Property(e => e.OperatorsPlanningDemandId)
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
