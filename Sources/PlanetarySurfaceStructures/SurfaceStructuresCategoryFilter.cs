﻿using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using KSP.Localization;

namespace PlanetarySurfaceStructures
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class SurfaceStructuresCategoryFilter : MonoBehaviour
    {
        //create and the icons
        private Texture2D icon_surface_structures;
        private Texture2D icon_category_ls;

        private Texture2D icon_filter_pods;
        private Texture2D icon_filter_engine;
        private Texture2D icon_filter_fuel;
        private Texture2D icon_filter_payload;
        private Texture2D icon_filter_construction;
        private Texture2D icon_filter_coupling;
        private Texture2D icon_filter_electrical;
        private Texture2D icon_filter_ground;
        private Texture2D icon_filter_utility;
        private Texture2D icon_filter_science;
        private Texture2D icon_filter_thermal;
        private Texture2D icon_filter_cargo;

        internal string iconName = "PlanetaryBaseSystemsEditor";
        internal bool filter = true;

        //set to false when an icon could not be loaded
        private bool isValid = true;

        //a dictionary storing all the categories of the parts
        private Dictionary<string, PartCategories> partCategories;

        //stores wheter life support is available
        private bool lifeSupportAvailable = false;

        //stores wheter the Community Category Kit is availables
        private bool CCKavailable = false;

        //The name of the category for the KPBS filter
        private string functionFilterName = Localizer.GetStringByTag("#LOC_KPBS.categoryfilter.function.name");//"Planetary Surface Structures";

        //The name of the function filter
        private string filterName = "#autoLOC_453547";

        //The instance of this filter
        public static SurfaceStructuresCategoryFilter Instance;

        /// <summary>
        /// Called when the script instance is being loaded
        /// </summary>
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            DontDestroyOnLoad(this);

            InitializeIcons();

            //search for Community Category Kit
            int numAssemblies = AssemblyLoader.loadedAssemblies.Count;
            for (int i = 0; i < numAssemblies; i++)
            {
                if (AssemblyLoader.loadedAssemblies[i].name.Equals("CCK"))
                {
                    CCKavailable = true;
                    break;
                }
            }

            //save all the categories from the parts of this mod
            List<AvailablePart> all_parts = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith("KKAOSS"));
            partCategories = new Dictionary<string, PartCategories>();
            int num = all_parts.Count;
            for (int i = 0; i < num; i++)
            {
                try
                {
                    partCategories.Add(all_parts[i].name, all_parts[i].category);
                }
                catch (ArgumentException e)
                {
                    Debug.LogError("[KPBS] Error in filter: " + e.Message);
                    Debug.LogError("[KPBS] Part: " + all_parts[i].name);
                }
            }

            //save whether life support parts are available
            lifeSupportAvailable = (all_parts.FindAll(ap => ap.name.StartsWith("KKAOSS.LS")).Count > 0);

            //load the icons
            try
            {
                string errors = string.Empty;

                //load the icons
                icon_filter_pods = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_pods", false);
                icon_filter_engine = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_engine", false);
                icon_filter_fuel = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_fueltank", false);
                icon_filter_payload = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_payload", false);
                icon_filter_construction = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_construction", false);
                icon_filter_coupling = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_coupling", false);
                icon_filter_electrical = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_electrical", false);
                icon_filter_ground = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_ground", false);
                icon_filter_utility = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_utility", false);
                icon_filter_science = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_science", false);
                icon_filter_thermal = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_thermal", false);
                icon_filter_cargo = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/filter_cargo", false);

                icon_surface_structures = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/KPBSicon", false);
                icon_category_ls = GameDatabase.Instance.GetTexture("PlanetaryBaseInc/BaseSystem/Icons/KPBSCategoryLifeSupport", false);


                if (icon_surface_structures == null)
                {
                    errors += "KPBSicon.png ";
                    isValid = false;
                }
                if (icon_category_ls == null)
                {
                    errors += "KPBSCategoryLifeSupport.png ";
                    isValid = false;
                }
                if (icon_filter_pods == null)
                {
                    errors += "ilter_pods.png ";
                    isValid = false;
                }
                if (icon_filter_fuel == null)
                {
                    errors += "filter_fueltank.png ";
                    isValid = false;
                }
                if (icon_filter_electrical == null)
                {
                    errors += "filter_electrical.png ";
                    isValid = false;
                }
                if (icon_filter_thermal == null)
                {
                    errors += "filter_thermal.png ";
                    isValid = false;
                }
                if (icon_filter_science == null)
                {
                    errors += "filter_science.png ";
                    isValid = false;
                }
                if (icon_filter_engine == null)
                {
                    errors += "filter_engine.png ";
                    isValid = false;
                }
                if (icon_filter_ground == null)
                {
                    errors += "filter_ground.png ";
                    isValid = false;
                }
                if (icon_filter_coupling == null)
                {
                    errors += "filter_coupling.png ";
                    isValid = false;
                }
                if (icon_filter_payload == null)
                {
                    errors += "filter_payload.png ";
                    isValid = false;
                }
                if (icon_filter_construction == null)
                {
                    errors += "filter_construction.png ";
                    isValid = false;
                }
                if (icon_filter_utility == null)
                {
                    errors += "filter_utility.png ";
                    isValid = false;
                }

                if (!isValid)
                {
                    Debug.LogError("[KPBS] ERROR loading: " + errors);
                }

            }
            catch (Exception e)
            {
                Debug.LogError("[KPBS] ERROR EXC loading Images " + e.Message);
                isValid = false;
            }

            GameEvents.onGUIEditorToolbarReady.Add(KPBSMainFilter);
            GameEvents.OnGameSettingsApplied.Add(updateFilterSettings);
        }

        /// <summary>
        /// Initializes the icons, called from the Awake() method.  Texture2D vars have
        /// to be initialized inside either an Awake, Start, or OnGUI
        /// </summary>
        void InitializeIcons()
        {
            icon_surface_structures = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_category_ls = new Texture2D(32, 32, TextureFormat.ARGB32, false);

            icon_filter_pods = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_engine = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_fuel = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_payload = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_construction = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_coupling = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_electrical = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_ground = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_utility = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_science = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_thermal = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            icon_filter_cargo = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        }
        /// <summary>
        /// Removes all listeners from the GameEvents when Class is destroyed
        /// </summary>
        protected void OnDestroy()
        {
            GameEvents.onGUIEditorToolbarReady.Remove(KPBSMainFilter);
            GameEvents.OnGameSettingsApplied.Remove(updateFilterSettings);
        }

        /// <summary>
        /// Update the settings from the filters
        /// </summary>
        public void updateFilterSettings()
        {
            Debug.Log("[KPBS] updateFilterSettings");
            GameEvents.onGUIEditorToolbarReady.Remove(KPBSFunctionFilter);

            if (HighLogic.CurrentGame != null)
            {
                RemoveFunctionFilter();
                AddPartCategories();

                if (HighLogic.CurrentGame.Parameters.CustomParams<KPBSSettings>().functionFilter)
                {
                    RemovePartCategories();
                    GameEvents.onGUIEditorToolbarReady.Add(KPBSFunctionFilter);
                }
            }
        }

        /// <summary>
        /// Remove the fuction filte category
        /// </summary>
        private void RemoveFunctionFilter()
        {
            if (PartCategorizer.Instance != null)
            {
                PartCategorizer.Category Filter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == filterName);
                if (Filter != null)
                {
                    PartCategorizer.Category subFilter = Filter.subcategories.Find(f => f.button.categoryName == functionFilterName);
                    if (subFilter != null)
                    {
                        subFilter.DeleteSubcategory();
                    }
                }
            }
        }

        /// <summary>
        /// Remove the fuction filte category
        /// </summary>
        private void AddFunctionFilter()
        {
            if (PartCategorizer.Instance != null)
            {
                PartCategorizer.Category Filter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == filterName);
                if (Filter != null)
                {
                    PartCategorizer.Category subFilter = Filter.subcategories.Find(f => f.button.categoryName == functionFilterName);
                    if (subFilter != null)
                    {
                        subFilter.DeleteSubcategory();
                    }
                }
            }
        }

        /// <summary>
        /// Add the stored categories to all the parts of KPBS
        /// </summary>
        private void AddPartCategories()
        {
            if (partCategories != null)
            {
                List<AvailablePart> parts = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith("KKAOSS"));
                for (int i = 0; i < parts.Count; i++)
                {
                    if (partCategories.ContainsKey(parts[i].name))
                    {
                        parts[i].category = partCategories[parts[i].name];
                    }
                }
            }
        }

        /// <summary>
        /// Remove the categories from all parts of KPBS
        /// </summary>
        private void RemovePartCategories()
        {
            List<AvailablePart> parts = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith("KKAOSS"));
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].category = PartCategories.none;
            }
        }

        /// <summary>
        /// ilter the parts by their mod
        /// </summary>
        /// <param name="part">the part to test</param>
        /// <returns>true part is from KPBS, else false</returns>
        private bool filter_KKAOSS(AvailablePart part)
        {
            return part.name.StartsWith("KKAOSS");
        }

        /// <summary>
        /// Filter the parts by their mod and life support
        /// </summary>
        /// <param name="part">the part to test</param>
        /// <returns>true part is for live support from KPBS, else false</returns>
        private bool filter_KKAOSS_LS(AvailablePart part)
        {
            return (part.name.StartsWith("KKAOSS.LS") || part.name.Equals("KKAOSS.Greenhouse.g"));
        }

        /// <summary>
        /// Filter the parts by their mod without life support
        /// </summary>
        /// <param name="part">the part to test</param>
        /// <returns>true part is from KKAOSS but not for life support, else false</returns>
        private bool filter_KKAOSS_NO_LS(AvailablePart part)
        {
            return part.name.StartsWith("KKAOSS") && !part.name.StartsWith("KKAOSS.LS") && !part.name.Equals("KKAOSS.Greenhouse.g");
        }

        /// <summary>
        /// Filter the parts by their categorys
        /// </summary>
        /// <param name="part">the part to test</param>
        /// <param name="category">the category to check for</param>
        /// <returns>true when categories match, else false</returns>
        private bool filterCategories(AvailablePart part, PartCategories category)
        {
            return (part.name.StartsWith("KKAOSS") && (partCategories[part.name] == category));
        }

        /// <summary>
        /// Add the function filter to the filter
        /// </summary>
        private void KPBSFunctionFilter()
        {
            if (!isValid)
            {
                Debug.LogError("[KPBS] invalid");
                return;
            }

            RUI.Icons.Selectable.Icon filterIconSurfaceStructures = new RUI.Icons.Selectable.Icon("KKAOSS_icon_lifeSupport", icon_surface_structures, icon_surface_structures, true);

            if (filterIconSurfaceStructures == null)
            {
                Debug.LogError("[KPBS] ERROR filterIconSurfaceStructures cannot be loaded");
                return;
            }

            //Find the function filter
            PartCategorizer.Category functionFilter = PartCategorizer.Instance.filters.Find(f => f.button.categorydisplayName == filterName);

            //Add a new subcategory to the function filter
            PartCategorizer.AddCustomSubcategoryFilter(functionFilter, functionFilterName, functionFilterName, filterIconSurfaceStructures, p => filter_KKAOSS(p));
        }

        /// <summary>
        /// The function to add the modules of this mod to a separate category.
        /// </summary>
        private void KPBSMainFilter()
        {
            if (!isValid)
            {
                Debug.LogError("[KPBS] invalid");
                return;
            }

            //if the configuration is null
            if (KPBSConfiguration.Instance() == null)
            {
                Debug.LogError("[KPBS] ERROR Configuration Instance is null");
                return;
            }

            //-----------------own category-----------------
            if (KPBSConfiguration.Instance().ShowModFilter)
            {
                //create the icon for the filter
                RUI.Icons.Selectable.Icon filterIconSurfaceStructures = new RUI.Icons.Selectable.Icon("KKAOSS_icon_KPSS", icon_surface_structures, icon_surface_structures, true);

                //icons for KPSS's own category
                RUI.Icons.Selectable.Icon ic_pods = new RUI.Icons.Selectable.Icon("KKAOSS_filter_pods", icon_filter_pods, icon_filter_pods, true);
                RUI.Icons.Selectable.Icon ic_fuels = new RUI.Icons.Selectable.Icon("KKAOSS_filter_fuel", icon_filter_fuel, icon_filter_fuel, true);
                RUI.Icons.Selectable.Icon ic_engine = new RUI.Icons.Selectable.Icon("KKAOSS_filter_fuel", icon_filter_engine, icon_filter_engine, true);
                RUI.Icons.Selectable.Icon ic_structural = new RUI.Icons.Selectable.Icon("KKAOSS_filter_fuel", icon_filter_construction, icon_filter_construction, true);
                RUI.Icons.Selectable.Icon ic_payload = new RUI.Icons.Selectable.Icon("KKAOSS_filter_fuel", icon_filter_payload, icon_filter_payload, true);
                RUI.Icons.Selectable.Icon ic_utility = new RUI.Icons.Selectable.Icon("KKAOSS_filter_fuel", icon_filter_utility, icon_filter_utility, true);
                RUI.Icons.Selectable.Icon ic_coupling = new RUI.Icons.Selectable.Icon("KKAOSS_filter_coupling", icon_filter_coupling, icon_filter_coupling, true);
                RUI.Icons.Selectable.Icon ic_ground = new RUI.Icons.Selectable.Icon("KKAOSS_filter_ground", icon_filter_ground, icon_filter_ground, true);
                RUI.Icons.Selectable.Icon ic_thermal = new RUI.Icons.Selectable.Icon("KKAOSS_filter_thermal", icon_filter_thermal, icon_filter_thermal, true);
                RUI.Icons.Selectable.Icon ic_electrical = new RUI.Icons.Selectable.Icon("KKAOSS_filter_electrical", icon_filter_electrical, icon_filter_electrical, true);
                RUI.Icons.Selectable.Icon ic_cargo = new RUI.Icons.Selectable.Icon("KKAOSS_filter_carog", icon_filter_cargo, icon_filter_cargo, true);
                RUI.Icons.Selectable.Icon ic_science = new RUI.Icons.Selectable.Icon("KKAOSS_filter_fuel", icon_filter_science, icon_filter_science, true);
                RUI.Icons.Selectable.Icon ic_lifeSupport = new RUI.Icons.Selectable.Icon("KKAOSS_icon_KPSS", icon_category_ls, icon_category_ls, true);


                //add KPBS to the categories
                PartCategorizer.Category surfaceStructureFilter = PartCategorizer.AddCustomFilter("Planetary Surface Structures", functionFilterName, filterIconSurfaceStructures, new Color(0.63f, 0.85f, 0.63f));

                //add subcategories to the KPSS category you just added
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Pods", Localizer.GetStringByTag("#autoLOC_453549"), ic_pods, p => filterCategories(p, PartCategories.Pods));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Fuel Tank", Localizer.GetStringByTag("#autoLOC_453552"), ic_fuels, p => filterCategories(p, PartCategories.FuelTank));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Engines", Localizer.GetStringByTag("#autoLOC_453555"), ic_engine, p => filterCategories(p, PartCategories.Propulsion));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Structural", Localizer.GetStringByTag("#autoLOC_453561"), ic_structural, p => filterCategories(p, PartCategories.Structural));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Coupling", Localizer.GetStringByTag("#autoLOC_453564"), ic_coupling, p => filterCategories(p, PartCategories.Coupling));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Payload", Localizer.GetStringByTag("#autoLOC_453567"), ic_payload, p => filterCategories(p, PartCategories.Payload));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Ground", Localizer.GetStringByTag("#autoLOC_453573"), ic_ground, p => filterCategories(p, PartCategories.Ground));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Thermal", Localizer.GetStringByTag("#autoLOC_453576"), ic_thermal, p => filterCategories(p, PartCategories.Thermal));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Electrical", Localizer.GetStringByTag("#autoLOC_453579"), ic_electrical, p => filterCategories(p, PartCategories.Electrical));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Science", Localizer.GetStringByTag("#autoLOC_453585"), ic_science, p => filterCategories(p, PartCategories.Science));
                PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Cargo", Localizer.GetStringByTag("#autoLOC_8320001"), ic_cargo, p => filterCategories(p, PartCategories.Cargo));

                if (lifeSupportAvailable)
                {
                    PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Utility", Localizer.GetStringByTag("#autoLOC_453588"), ic_utility, p => (filterCategories(p, PartCategories.Utility) && !filter_KKAOSS_LS(p)));
                    PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Life Support", Localizer.GetStringByTag("#LOC_KPBS.categoryfilter.category.lifesupport"), ic_lifeSupport, p => filter_KKAOSS_LS(p));
                }
                else
                {
                    PartCategorizer.AddCustomSubcategoryFilter(surfaceStructureFilter, "Utility", Localizer.GetStringByTag("#autoLOC_453588"), ic_utility, p => (filterCategories(p, PartCategories.Utility)));
                }
            }
            //-----------------end own category-----------------

            //------------subcategory for life support---------

            //when the community category kit is not available, add own category for life support
            if (!CCKavailable)
            {
                //only continue when there are parts for life support
                if (lifeSupportAvailable)
                {
                    RUI.Icons.Selectable.Icon filterIconLifeSupport = new RUI.Icons.Selectable.Icon("KKAOSS_icon_KPSS", icon_category_ls, icon_category_ls, true);

                    if (filterIconLifeSupport == null)
                    {
                        Debug.Log("[KPBS] ERROR filterIconLifeSupport cannot be loaded");
                        return;
                    }

                    //Find the function filter
                    PartCategorizer.Category functionFilter = PartCategorizer.Instance.filters.Find(f => f.button.categorydisplayName == filterName);
                    PartCategorizer.AddCustomSubcategoryFilter(functionFilter, "Life Support", Localizer.GetStringByTag("#LOC_KPBS.categoryfilter.category.lifesupport"), filterIconLifeSupport, p => filter_KKAOSS_LS(p));

                    //add the greenhouse the the LS mods when other ls mods were found
                    List<AvailablePart> greenhouses = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.Equals("KKAOSS.Greenhouse.g"));
                    for (int i = 0; i < greenhouses.Count; i++)
                    {
                        greenhouses[i].category = PartCategories.none;
                    }
                }
            }
            else if (lifeSupportAvailable)
            {
                List<AvailablePart> greenhouses = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.Equals("KKAOSS.Greenhouse.g"));
                for (int i = 0; i < greenhouses.Count; i++)
                {
                    greenhouses[i].category = PartCategories.none;
                }
            }
            //---------end subcategory for life support-------
        }
    }
}