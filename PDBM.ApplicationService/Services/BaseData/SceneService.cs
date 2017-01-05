using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 周边场景应用层服务
    /// </summary>
    public class SceneService : DataService, ISceneService
    {
        private readonly IRepository<Scene> sceneRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Purchase> purchaseRepository;

        public SceneService(IRepositoryContext context,
            IRepository<Scene> sceneRepository,
            IRepository<Place> placeRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Purchase> purchaseRepository)
            : base(context)
        {
            this.sceneRepository = sceneRepository;
            this.placeRepository = placeRepository;
            this.addressingRepository = addressingRepository;
            this.purchaseRepository = purchaseRepository;
        }

        /// <summary>
        /// 根据周边场景Id获取周边场景
        /// </summary>
        /// <param name="id">周边场景Id</param>
        /// <returns>周边场景维护对象</returns>
        public SceneMaintObject GetSceneById(Guid id)
        {
            Scene scene = sceneRepository.FindByKey(id);
            if (scene != null)
            {
                SceneMaintObject sceneMaintObject = MapperHelper.Map<Scene, SceneMaintObject>(scene);
                return sceneMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的周边场景在系统中不存在");
            }
        }

        /// <summary>
        /// 获取周边场景列表
        /// </summary>
        /// <returns>周边场景维护对象列表</returns>
        public IList<SceneMaintObject> GetScenes()
        {
            IList<SceneMaintObject> sceneMaintObjects = new List<SceneMaintObject>();
            IEnumerable<Scene> scenes = sceneRepository.FindAll(null, "SceneCode");
            if (scenes != null)
            {
                foreach (var scene in scenes)
                {
                    sceneMaintObjects.Add(MapperHelper.Map<Scene, SceneMaintObject>(scene));
                }
            }
            return sceneMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的周边场景列表
        /// </summary>
        /// <returns>周边场景选择对象列表</returns>
        public IList<SceneSelectObject> GetUsedScenes()
        {
            IList<SceneSelectObject> sceneSelectObjects = new List<SceneSelectObject>();
            IEnumerable<Scene> scenes = sceneRepository.FindAll(Specification<Scene>.Eval(entity => entity.State == State.使用), "SceneCode");
            if (scenes != null)
            {
                foreach (var scene in scenes)
                {
                    sceneSelectObjects.Add(MapperHelper.Map<Scene, SceneSelectObject>(scene));
                }
            }
            return sceneSelectObjects;
        }

        /// <summary>
        /// 新增或者修改周边场景
        /// </summary>
        /// <param name="sceneMaintObject">要新增或者修改的周边场景维护对象</param>
        public void AddOrUpdateScene(SceneMaintObject sceneMaintObject)
        {
            if (sceneMaintObject.Id == Guid.Empty)
            {
                Scene scene = AggregateFactory.CreateScene(sceneMaintObject.SceneCode, sceneMaintObject.SceneName,
                    sceneMaintObject.Remarks, (State)sceneMaintObject.State, sceneMaintObject.CreateUserId);
                sceneRepository.Add(scene);
            }
            else
            {
                Scene scene = sceneRepository.FindByKey(sceneMaintObject.Id);
                if (scene != null)
                {
                    scene.Modify(sceneMaintObject.SceneCode, sceneMaintObject.SceneName, sceneMaintObject.Remarks,
                        (State)sceneMaintObject.State, sceneMaintObject.ModifyUserId);
                    sceneRepository.Update(scene);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_SceneCode"))
                {
                    throw new ApplicationFault("周边场景编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_SceneName"))
                {
                    throw new ApplicationFault("周边场景名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除周边场景
        /// </summary>
        /// <param name="sceneMaintObjects">要删除的周边场景维护对象列表</param>
        public void RemoveScenes(IList<SceneMaintObject> sceneMaintObjects)
        {
            foreach (SceneMaintObject sceneMaintObject in sceneMaintObjects)
            {
                Scene scene = sceneRepository.FindByKey(sceneMaintObject.Id);
                if (scene != null)
                {
                    sceneRepository.Remove(scene);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
