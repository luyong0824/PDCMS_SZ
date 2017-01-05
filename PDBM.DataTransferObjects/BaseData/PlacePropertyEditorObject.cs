using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 运营商使用情况
    /// </summary>
    [DataContract, ProtoContract]
    public class PlacePropertyEditorObject
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 父级Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 用电量
        /// </summary>
        [DataMember, ProtoMember(6)]
        public decimal PowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 安装是否完成
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 登记人Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 登记人名字
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 登记日期文本
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 分公司Id
        /// </summary>
        [DataMember, ProtoMember(17)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 任务Id
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid ConstructionTaskId
        {
            get;
            set;
        }
    }
}
