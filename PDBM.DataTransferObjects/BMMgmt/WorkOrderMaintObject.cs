using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 派工单维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkOrderMaintObject
    {
        /// <summary>
        /// 派工单Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 工单小类Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid WorkSmallClassId
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系人
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string SceneContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string SceneContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 要求派工日期
        /// </summary>
        [DataMember, ProtoMember(7)]
        public DateTime RequireSendDate
        {
            get;
            set;
        }

        /// <summary>
        /// 派工时长
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int Days
        {
            get;
            set;
        }

        /// <summary>
        /// 代维单位
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 代维联系人Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid CustomerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 代维电话
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string MaintainContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 工作内容
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string WorkContent
        {
            get;
            set;
        }

        /// <summary>
        /// 用人要求
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string HumanRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 用车要求
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string CarRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 材料要求
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string MaterialRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        [DataMember, ProtoMember(17)]
        public DateTime WorkBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(小时)
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int BeginHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int BeginMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        [DataMember, ProtoMember(20)]
        public DateTime WorkEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(小时)
        /// </summary>
        [DataMember, ProtoMember(21)]
        public int EndHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(22)]
        public int EndMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        [DataMember, ProtoMember(23)]
        public int IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记人
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid? RegisterUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记日期
        /// </summary>
        [DataMember, ProtoMember(25)]
        public DateTime RegisterDate
        {
            get;
            set;
        }

        /// <summary>
        /// 执行情况
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string ExecuteSituation
        {
            get;
            set;
        }

        /// <summary>
        /// 材料消耗
        /// </summary>
        [DataMember, ProtoMember(27)]
        public string MaterialConsumption
        {
            get;
            set;
        }

        /// <summary>
        /// 人员数量
        /// </summary>
        [DataMember, ProtoMember(28)]
        public string PersonnelNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember, ProtoMember(29)]
        public string CarType
        {
            get;
            set;
        }

        /// <summary>
        /// 派工单审批状态
        /// </summary>
        [DataMember, ProtoMember(30)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(31)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(32)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(33)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(34)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工单大类Id
        /// </summary>
        [DataMember, ProtoMember(35)]
        public Guid WorkBigClassId
        {
            get;
            set;
        }

        /// <summary>
        /// 工单大类Id
        /// </summary>
        [DataMember, ProtoMember(36)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(37)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 附件Id列表
        /// </summary>
        [DataMember, ProtoMember(38)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 派工日期
        /// </summary>
        [DataMember, ProtoMember(39)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 要求派工日期
        /// </summary>
        [DataMember, ProtoMember(40)]
        public string RequireSendDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(41)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 隐患上报Id
        /// </summary>
        [DataMember, ProtoMember(42)]
        public Guid WorkApplyId
        {
            get;
            set;
        }

        /// <summary>
        /// 单位分类
        /// </summary>
        [DataMember, ProtoMember(43)]
        public int CustomerType
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(44)]
        public string PlaceName
        {
            get;
            set;
        }
    }
}
