using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 站点服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlaceService : IDistributedService
    {
        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>站点维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceMaintObject GetPlaceById(Guid id);

        /// <summary>
        /// 根据条件获取分页站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="profession">专业</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="state">状态</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlacesPage(int pageIndex, int pageSize, int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state);

        /// <summary>
        /// 根据条件获取分页站点列表，用于选择站点
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlacesPageBySelect(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId);

        /// <summary>
        /// 修改站点
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void UpdatePlace(PlaceMaintObject placeMaintObject);

        /// <summary>
        /// 根据站点Id获取站点信息及参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceInfoObject GetPlaceInfoById(Guid id);

        /// <summary>
        /// 根据条件获取分页资源导入站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="propertyRight">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <param name="state">状态</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetResourcePlacesPage(int pageIndex, int pageSize, string groupPlaceCode, string placeName, int profession, Guid placeCategoryId,
            Guid areaId, Guid reseauId, int propertyRight, int importance, int telecomShare, int mobileShare, int unicomShare, int state);

        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>站点维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceMaintObject GetPlaceImportById(Guid id);

        /// <summary>
        /// 根据条件获取分页基站导入站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="state">状态</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlaceImportsPage(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid placeCategoryId, Guid placeOwner,
            Guid areaId, Guid reseauId, int importance, int state, Guid companyId);

        /// <summary>
        /// 修改站点
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlaceImport(PlaceMaintObject placeMaintObject);

        /// <summary>
        /// 根据站点Id获取逻辑号
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>站点维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceMaintObject GetLogicalNumberById(Guid id);

        /// <summary>
        /// 根据条件获取分页基站站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="g2Mark">2G</param>
        /// <param name="d2Mark">2D</param>
        /// <param name="g3Mark">3G</param>
        /// <param name="g4Mark">4G</param>
        /// <param name="g2Number">2G逻辑号</param>
        /// <param name="d2Number">2D逻辑号</param>
        /// <param name="g3Number">3G逻辑号</param>
        /// <param name="g4Number">4G逻辑号</param>
        /// <param name="allMark">全部</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetLogicalNumbersPage(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark);

        /// <summary>
        /// 修改逻辑号
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveLogicalNumber(PlaceMaintObject placeMaintObject);

        /// <summary>
        /// 根据条件获取基站列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <param name="placeName">站点名称</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlacesMobile(int pageIndex, int pageSize, string professionListSql, string placeName, Guid companyId);

        /// <summary>
        /// 根据条件获取基站列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <returns>分页站点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlacesPageMobile(int pageIndex, int pageSize, string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId);

        /// <summary>
        /// 更新站点方位(移动端)
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlacePositionMobile(PlaceMaintObject placeMaintObject);

        /// <summary>
        /// 站点修改(移动端)
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlaceMobile(PlaceMaintObject placeMaintObject);
    }
}
