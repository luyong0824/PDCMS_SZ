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
    /// PlaceDesign表配置
    /// </summary>
    internal class PlaceDesignConfiguration : EntityTypeConfiguration<PlaceDesign>
    {
        public PlaceDesignConfiguration()
        {
            ToTable("tbl_PlaceDesign");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.PropertyType)
                .IsRequired();
            Property(e => e.DesignCustomerId)
                .IsOptional();
            Property(e => e.DesignUserId)
                .IsOptional();
            Property(e => e.SupervisorCustomerId)
                .IsOptional();
            Property(e => e.SupervisorUserId)
                .IsOptional();
            Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ProjectName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.ProjectMoney)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.ProjectIsApply)
                .IsRequired();
            Property(e => e.ProjectApplyDate)
                .IsRequired();
            Property(e => e.ProjectIsDoApply)
                .IsRequired();
            Property(e => e.ProjectDoApplyDate)
                .IsRequired();
            Property(e => e.GroupPlaceCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.TowerMark)
                .IsRequired();
            Property(e => e.TowerBaseMark)
                .IsRequired();
            Property(e => e.MachineRoomMark)
                .IsRequired();
            Property(e => e.ExternalElectricPowerMark)
                .IsRequired();
            Property(e => e.EquipmentInstallMark)
                .IsRequired();
            Property(e => e.AddressExplorMark)
                .IsRequired();
            Property(e => e.FoundationTestMark)
                .IsRequired();
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
