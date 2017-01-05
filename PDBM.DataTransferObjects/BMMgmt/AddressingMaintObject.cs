using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 寻址确认维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class AddressingMaintObject
    {
        /// <summary>
        /// 寻址确认Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 规划Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid PlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string PlanningName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(11)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(12)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int Importance
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(14)]
        public Guid PlaceOwner
        {
            get;
            set;
        }

        /// <summary>
        /// 拟建网络
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁部门Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid AddressingDepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁人
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string AddressingRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系人
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系电话
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(25)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        [DataMember, ProtoMember(26)]
        public Guid AreaManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 总设单位Id
        /// </summary>
        [DataMember, ProtoMember(27)]
        public Guid DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 流程步骤Id
        /// </summary>
        [DataMember, ProtoMember(28)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 分公司Id
        /// </summary>
        [DataMember, ProtoMember(29)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(30)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址状态
        /// </summary>
        [DataMember, ProtoMember(31)]
        public int AddressingState
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理名称
        /// </summary>
        [DataMember, ProtoMember(32)]
        public string AreaManagerText
        {
            get;
            set;
        }

        /// <summary>
        /// 总设单位名称
        /// </summary>
        [DataMember, ProtoMember(33)]
        public string DesignCustomerText
        {
            get;
            set;
        }
    }
}
