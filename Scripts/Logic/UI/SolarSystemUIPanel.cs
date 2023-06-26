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
    /// ̫��ϵ����UI����
    /// </summary>
    public class SolarSystemUIPanel : UGuiFormEx
    {
        [Header("��UI����Ԥ�����еĿؼ�")]
        [Tooltip("���ص��������Home��ť")]
        public Button btn_Home;
        [Tooltip("̫��ϵ�е���Ҫ���������б�")]
        public TMP_Dropdown tMP_Dropdown_CelestialBodies;
        [Tooltip("̫��ϵ�е�������������")]
        public Text txt_Planet_ChineseName;

        [Header("Ҫ����ʾ��UI����Ԥ������Assets�е�·������")]
        [Tooltip("Ҫ����ʾ��UI����Ԥ������Assets�е�·������")]
        public string[] array_Str_PathOfToOpenUIPrefabInAssets;


        [Tooltip("Ҫ����ʾ��UI����Ԥ�������ڵķ�������")]
        public string[] array_Str_GroupOfToOpenUIPrefabInAssets;

        [Header("����")]
        [Tooltip("̫��ϵ�е���Ҫ�����ֵ�")]
        public Dictionary<int, string> dict_CelestialBodies = new Dictionary<int, string>();
        [Tooltip("̫��ϵ�е���Ҫ������������Ӣ������Ӧ���ֵ�")]
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
            // ��̫��ϵ�е���Ҫ���������б��ѡ����뵽̫��ϵ�е���Ҫ�����ֵ���
            for (int i = 0; i < tMP_Dropdown_CelestialBodies.options.Count; i++)
            {
                dict_CelestialBodies.Add(i, tMP_Dropdown_CelestialBodies.options[i].text);
            }
            tMP_Dropdown_CelestialBodies.onValueChanged.AddListener(OnTMP_Dropdown_CelestialBodiesValueChanged);
            //���̫��ϵ�е���Ҫ������������Ӣ������Ӧ�ļ�ֵ��:
            dict_CelestialBodies_ChineseName_EnglishName.Add("ˮ��", "Mercury");
            dict_CelestialBodies_ChineseName_EnglishName.Add("����", "Venus");
            dict_CelestialBodies_ChineseName_EnglishName.Add("����", "Earth");
            dict_CelestialBodies_ChineseName_EnglishName.Add("̫��", "Sun");
            dict_CelestialBodies_ChineseName_EnglishName.Add("����", "Mars");
            dict_CelestialBodies_ChineseName_EnglishName.Add("ľ��", "Jupiter");
            dict_CelestialBodies_ChineseName_EnglishName.Add("����", "Saturn");
            dict_CelestialBodies_ChineseName_EnglishName.Add("������", "Uranus");
            dict_CelestialBodies_ChineseName_EnglishName.Add("������", "Neptune");
            dict_CelestialBodies_ChineseName_EnglishName.Add("ڤ����", "Pluto");
            dict_CelestialBodies_ChineseName_EnglishName.Add("����", "Moon");
        }

        private void OnHomeButtonClick()
        {
            //GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            SceneManager.LoadSceneAsync("Main");
            Debug.Log("�����Home��ť,���ص�������Main������");
        }

        private void OnTMP_Dropdown_CelestialBodiesValueChanged(int index)
        {
            Debug.Log("ѡ����̫��ϵ�е���Ҫ���������б��еĵ� " + (index + 1) + " ��ѡ�" + tMP_Dropdown_CelestialBodies.options[index].text);
            string str_Dict_CelestialBodies_ChineseNameValue = dict_CelestialBodies[index];
            Debug.Log("ѡ���˵� " + (index + 1) + " ��ѡ��,������Ϊ��" + str_Dict_CelestialBodies_ChineseNameValue);
            string str_Dict_CelestialBodies_EnglishNameValue = dict_CelestialBodies_ChineseName_EnglishName[str_Dict_CelestialBodies_ChineseNameValue];
            Debug.Log("ѡ���˵� " + (index + 1) + " ��ѡ��,Ӣ����Ϊ��" + str_Dict_CelestialBodies_EnglishNameValue);
            UIManager.instance.SwitchToDetailedPlanetView(str_Dict_CelestialBodies_EnglishNameValue);
            txt_Planet_ChineseName.text = tMP_Dropdown_CelestialBodies.options[index].text;
        }
    }
}



