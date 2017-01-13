using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planetbase;
using PlanetbaseFramework;
using UnityEngine;

namespace PBMod
{
    public class PBMod : ModBase
    {
        public PBMod() : this("PBMod")
        {

        }

        public PBMod(string ModName) : base(ModName)
        {
        }

        public override void Init()
        {
            TypeList<ModuleType, ModuleTypeList> moduleList = TypeList<ModuleType, ModuleTypeList>.getInstance();
            moduleList.add(new ModuleTypeOxygenTank(this));
        }

        public override void Update()
        {
        }
    }

    class ModuleTypeOxygenTank : ModuleType
    {
        int mOxygenStorageCapacity;
        public ModuleTypeOxygenTank(PBMod mod)
        {
            this.mIcon = Utils.LoadPNG(mod, @"BuildingIcon.png");//Todo: add a method to do this automagically from the mod's folder
            this.mPowerGeneration = -1000;
            this.mExterior = false;
            this.mMinSize = 1;
            this.mMaxSize = 3;
            this.mExtraSize = -1;
            this.mHeight = 1f;
            this.mOxygenStorageCapacity = 400000;
            this.mRequiredStructure.set<ModuleTypeOxygenGenerator>();
            //this.mExteriorNavRadius = 3f;
            this.initStrings();
            this.mCost = new ResourceAmounts();
            this.mFlags = 16 + 32 + 2048;
            this.mLayoutType = LayoutType.Circular;

            
        }

        public override ResourceAmounts calculateCost(int sizeIndex)
        {
            ResourceAmounts resources = new ResourceAmounts();

            resources.add(TypeList<ResourceType, ResourceTypeList>.find<Metal>(), sizeIndex * 2);
            resources.add(TypeList<ResourceType, ResourceTypeList>.find<Bioplastic>(), sizeIndex * 3);

            return resources;
        }

        public override GameObject loadPrefab(int sizeIndex)
        {
            GameObject moduleObject = UnityEngine.Object.Instantiate<GameObject>(ResourceUtil.loadPrefab("Prefabs/Modules/PrefabBioDome" + (sizeIndex + 1)));

            moduleObject.calculateSmoothMeshRecursive(ModuleType.mMeshes);
            if (moduleObject.GetComponent<Collider>() != null)
            {
                Debug.LogWarning("COLLISION IN THE ROOT");
            }
            GameObject moduleObject2 = GameObject.Find(ModuleType.GroupName);
            if (moduleObject2 == null)
            {
                moduleObject2 = new GameObject();
                moduleObject2.name = ModuleType.GroupName;
            }
            moduleObject.transform.SetParent(moduleObject2.transform, false);
            moduleObject.SetActive(false);
            return moduleObject;
        }
    }
}
