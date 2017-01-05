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
    /// 建设进度维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ConstructionTaskEditorObject
    {
        /// <summary>
        /// 建设任务Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 建设进度
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int ConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔类型
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int TowerType
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔高度
        /// </summary>
        [DataMember, ProtoMember(7)]
        public decimal TowerHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 平台数量
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int PlatFormNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 机房类型
        /// </summary>
        [DataMember, ProtoMember(10)]
        public int MachineRoomType
        {
            get;
            set;
        }

        /// <summary>
        /// 机房面积
        /// </summary>
        [DataMember, ProtoMember(11)]
        public decimal MachineRoomArea
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int ExternalElectric
        {
            get;
            set;
        }

        /// <summary>
        /// 开关电源
        /// </summary>
        [DataMember, ProtoMember(13)]
        public decimal SwitchPower
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        [DataMember, ProtoMember(14)]
        public decimal Battery
        {
            get;
            set;
        }

        /// <summary>
        /// 空调
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int AirConditioner
        {
            get;
            set;
        }

        /// <summary>
        /// 消防
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int FireControl
        {
            get;
            set;
        }

        /// <summary>
        /// 附件列表Id
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人Id
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int Count
        {
            get;
            set;
        }
    }
}
