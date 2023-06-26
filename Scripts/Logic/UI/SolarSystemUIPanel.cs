using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using UnityEngine.SceneManagement;
using GameFramework.Event;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using TMPro;
using Unity.VisualScripting;

namespace Flower
{
    /// <summary>
    /// 太阳系场景UI控制
    /// </summary>
    public class SolarSystemUIPanel : UGuiFormEx
    {
        [Header("本UI界面预制体中的控件")]
        [Tooltip("返回到主界面的Home按钮")]
        public Button btn_Home;
        [Tooltip("太阳系中的主要天体下拉列表")]
        public TMP_Dropdown tMP_Dropdown_CelestialBodies;
        [Tooltip("太阳系中的天体中文名称")]
        public Text txt_Planet_ChineseName;

        [Header("要打开显示的UI界面预制体在Assets中的路径数组")]
        [Tooltip("要打开显示的UI界面预制体在Assets中的路径数组")]
        public string[] array_Str_PathOfToOpenUIPrefabInAssets;


        [Tooltip("要打开显示的UI界面预制体所在的分组数组")]
        public string[] array_Str_GroupOfToOpenUIPrefabInAssets;

        [Header("集合")]
        [Tooltip("太阳系中的主要天体字典")]
        public Dictionary<int, string> dict_CelestialBodies = new Dictionary<int, string>();
        [Tooltip("太阳系中的主要天体中文名和英文名对应的字典")]
        public Dictionary<string, string> dict_CelestialBodies_ChineseName_EnglishName = new Dictionary<string, string>();

        public static SolarSystemUIPanel instance = null;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            btn_Home.onClick.AddListener(OnHomeButtonClick);
            tMP_Dropdown_CelestialBodies.value = 0;
            // 将太阳系中的主要天体下拉列表的选项加入到太阳系中的主要天体字典中
            for (int i = 0; i < tMP_Dropdown_CelestialBodies.options.Count; i++)
            {
                dict_CelestialBodies.Add(i, tMP_Dropdown_CelestialBodies.options[i].text);
            }
            tMP_Dropdown_CelestialBodies.onValueChanged.AddListener(OnTMP_Dropdown_CelestialBodiesValueChanged);
            //添加太阳系中的主要天体中文名和英文名对应的键值对:
            dict_CelestialBodies_ChineseName_EnglishName.Add("水星", "Mercury");
            dict_CelestialBodies_ChineseName_EnglishName.Add("金星", "Venus");
            dict_CelestialBodies_ChineseName_EnglishName.Add("地球", "Earth");
            dict_CelestialBodies_ChineseName_EnglishName.Add("太阳", "Sun");
            dict_CelestialBodies_ChineseName_EnglishName.Add("火星", "Mars");
            dict_CelestialBodies_ChineseName_EnglishName.Add("木星", "Jupiter");
            dict_CelestialBodies_ChineseName_EnglishName.Add("土星", "Saturn");
            dict_CelestialBodies_ChineseName_EnglishName.Add("天王星", "Uranus");
            dict_CelestialBodies_ChineseName_EnglishName.Add("海王星", "Neptune");
            dict_CelestialBodies_ChineseName_EnglishName.Add("冥王星", "Pluto");
            dict_CelestialBodies_ChineseName_EnglishName.Add("月球", "Moon");
        }

        private void OnHomeButtonClick()
        {
            //GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            SceneManager.LoadSceneAsync("Main");
            Debug.Log("点击了Home按钮,返回到主界面Main场景了");
        }

        private void OnTMP_Dropdown_CelestialBodiesValueChanged(int index)
        {
            Debug.Log("选中了太阳系中的主要天体下拉列表中的第 " + (index + 1) + " 个选项：" + tMP_Dropdown_CelestialBodies.options[index].text);
            string str_Dict_CelestialBodies_ChineseNameValue = dict_CelestialBodies[index];
            Debug.Log("选中了第 " + (index + 1) + " 个选项,中文名为：" + str_Dict_CelestialBodies_ChineseNameValue);
            string str_Dict_CelestialBodies_EnglishNameValue = dict_CelestialBodies_ChineseName_EnglishName[str_Dict_CelestialBodies_ChineseNameValue];
            Debug.Log("选中了第 " + (index + 1) + " 个选项,英文名为：" + str_Dict_CelestialBodies_EnglishNameValue);
            UIManager.instance.SwitchToDetailedPlanetView(str_Dict_CelestialBodies_EnglishNameValue);
            txt_Planet_ChineseName.text = tMP_Dropdown_CelestialBodies.options[index].text;
        }
    }
}



