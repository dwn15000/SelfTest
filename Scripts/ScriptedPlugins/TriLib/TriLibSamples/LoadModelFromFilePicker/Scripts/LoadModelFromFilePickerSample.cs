#pragma warning disable 649
using TriLibCore.General;
using UnityEngine;
using TriLibCore.Extensions;
using UnityEngine.UI;
using System.IO;
using TriLibCore.Interfaces;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using DG.Tweening.Plugins.Core.PathCore;

namespace TriLibCore.Samples
{
    /// <summary>
    /// Represents a sample that loads a Model from a file-picker.
    /// </summary>
    public class LoadModelFromFilePickerSample : MonoBehaviour
    {
        /// <summary>
        /// 需要克隆的材质球
        /// </summary>
        public Material cloneMaterials;

        public AssetLoaderOptions Options;
        /// <summary>
        /// The last loaded GameObject.
        /// </summary>
        private GameObject _loadedGameObject;

        /// <summary>
        /// The load Model Button.
        /// </summary>
        [SerializeField]
        private Button _loadModelButton;

        /// <summary>
        /// The progress indicator Text;
        /// </summary>
        [SerializeField]
        public Text _progressText;

        /// <summary>
        /// Creates the AssetLoaderOptions instance and displays the Model file-picker.
        /// </summary>
        /// <remarks>
        /// You can create the AssetLoaderOptions by right clicking on the Assets Explorer and selecting "TriLib->Create->AssetLoaderOptions->Pre-Built AssetLoaderOptions".
        /// </remarks>
        public void LoadModel()
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            assetLoaderOptions.UseMaterialKeywords = true;
            assetLoaderOptions.ImportColors = true;
              var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
            assetLoaderFilePicker.LoadModelFromFilePickerAsync("选择一个模型", OnLoad, OnMaterialsLoad, OnProgress, OnBeginLoad, OnError, null, assetLoaderOptions);
        }


        public void LocalLoadModel()
        {
            AssetLoaderOptions options = AssetLoader.CreateDefaultLoaderOptions();
            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
           
            filePath = PlayerPrefs.GetString("NewSate", "");
            Debug.Log("filePath=" + filePath);
            //string[] Names = path.Split('/');
            //string name = (Names[Names.Length - 1].Split('.'))[0];
           // IocContainer.GetInstance().GetInstance<WWWTest>().DownStreamFile(filePath);
           
        }


        public void WaidCallLoadModel(string path)
        {
            
            AssetLoaderOptions options = AssetLoader.CreateDefaultLoaderOptions();
            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
            string[] Names = path.Split('/');
            string name = (Names[Names.Length - 1].Split('.'))[0];
            Debug.Log("WaidCallLoadModelWaidCallLoadModel:" + path+ ",,,,,name="+ name);
            IocContainer_InstanceMgr.GetInstance().GetInstance<WWWTest>().DownStreamFile(path,name);
        }

        /// <summary>
        /// 获取玩家所有模型数据
        /// </summary>
        public void GetAllModelData()
        {
            //RequestGetAllModelData person = new RequestGetAllModelData { UserId = ModelData.GetInstance().userId, ModelCount = 3 };
            //string json = JsonConvert.SerializeObject(person);
            //string str = "4+" + json;
            //IocContainer.GetInstance().GetInstance<UDPClient>().SocketSend(str);
        }

        private void Start()
        {
            IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
        }


        #region  新版写的加载模型文件的方法
        string filePath;
        /// <summary>
        /// 选择文件
        /// </summary>
        public void NewLoadModel()
        {
            // /filePath = UnityEditor.EditorUtility.OpenFilePanel("选择一个模型", "", "fbx,obj,glb,gltf");
            //PlayerPrefs.SetString("dwn",filePath);
            OpenFile();
        }


        public void OpenFile()
        {
            FileOpenDialog dialog = new FileOpenDialog();

            dialog.structSize = Marshal.SizeOf(dialog);

            //dialog.filter = "exe files\0*.exe\0All Files\0*.*\0\0";
            dialog.filter = "0All Files\0*.*\0\0";

            dialog.file = new string(new char[256]);

            dialog.maxFile = dialog.file.Length;

            dialog.fileTitle = new string(new char[64]);

            dialog.maxFileTitle = dialog.fileTitle.Length;

            dialog.initialDir = UnityEngine.Application.dataPath;  //默认路径

            dialog.title = "Open File Dialog";

            dialog.defExt = "exe";//显示文件的类型
                                  //注意一下项目不一定要全选 但是0x00000008项不要缺少
            dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

            if (DialogShow.GetOpenFileName(dialog))
            {
                Debug.Log(dialog.file);
                filePath = dialog.file;
                PlayerPrefs.SetString("dwn", filePath);
                NewLoadIsModel();
            }
        }

        /// <summary>
        /// 执行加载模型的
        /// </summary>
         void NewLoadIsModel()
        {
            AssetLoaderOptions options = AssetLoader.CreateDefaultLoaderOptions();
            //options.RotationAngles = new Vector3(-90.0f, 0.0f, 0.0f); // Rotate the model to fix Y-up/Z-up axis issues
            //AssetLoader.LoadModelFromFile(filePath, OnModelLoaded, OnMaterialsLoad, OnProgress);
        }
        //private void OnModelLoaded(AssetLoaderContext assetLoaderContext)
        //{         
        //    byte[] fileBytes = File.ReadAllBytes(filePath);
        //    //string savePath = Path.Combine(Application.persistentDataPath, Path.GetFileName(filePath));
        //    string savePath = UnityEngine.Application.persistentDataPath + "/" + Path.GetFileName(filePath);

        //    File.WriteAllBytes(savePath, fileBytes);

        //    Debug.Log("Model saved to: " + savePath);
           
        //    PlayerPrefs.SetString("NewSate", savePath);
           
        //    AssetLoader.LoadModelFromFile(savePath, OnLoad);
        //}

        #endregion
       

        /// <summary>
        /// Called when the the Model begins to load.
        /// </summary>
        /// <param name="filesSelected">Indicates if any file has been selected.</param>
        private void OnBeginLoad(bool filesSelected)
        {
            //_loadModelButton.interactable = !filesSelected;
           // _progressText.enabled = filesSelected;
        }

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            _progressText.text = $"Progress: {progress:P}";
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            if (assetLoaderContext.RootGameObject != null)
            {
                
                TestSam(assetLoaderContext.RootGameObject);
                Debug.Log("Model fully loaded.");
            }
            else
            {
                Debug.Log("Model could not be loaded.");
            }
            //_loadModelButton.interactable = true;
            _progressText.enabled = false;
        }


        void TestSam(GameObject go)
        {         
            MeshRenderer[] child = go.transform.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < child.Length; i++)
            {
                string[] str = child[i].name.Split('_');
                int count = str.Length-1;
                if (str[str.Length - 1].Substring(0, 1) == "N")
                {
                    count--;
                }
                else if (str[str.Length - 1].Substring(0, 1) == "M")
                {
                    count--;
                }
             
                //if (str[str.Length - 1] == "Nor")
                //{
                //    count--;
                //}

                Material[] mt = child[i].materials;
              
               
                if (count == 0)
                {
                    for (int j = 0; j < mt.Length; j++)
                    {

                        Color al = mt[j].GetColor("_Color");
                        mt[j].shader = Shader.Find("Universal Render Pipeline/Lit");
                        mt[j].SetColor("_BaseColor", al);
                    }
                }
                else
                {
                    
                    for (int j = 0; j < count; j++)
                    {
                        Material ma = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                       // Material ma = Instantiate(cloneMaterials);
                        mt[j] = ma;
                        //Debug.Log("材质球:"+ mt[j].name+",,,name="+ child[i].name);
                        //以下是原来的
                       // Color al = mt[j].GetColor("_Color");
                       // mt[j].shader = Shader.Find("Universal Render Pipeline/Lit");
                       // mt[j].SetColor("_BaseColor", al);

                        if (str.Length > 1 && j < 4)
                        {
                            IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().SetMap(ma, str[j + 1], false);
                            IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().SetMap(mt[j], str[j + 1], false);
                            foreach (var item in IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().allModelShaderData)
                            {
                                string[] Pars = item.Split(",");
                                //Debug.Log("第一个名字:"+str[0]+",,,StringName="+Pars[0]+",,item:"+item);
                                if (Pars[0] == str[0])
                                {
                                    mt[j].SetFloat("_Metallic", float.Parse(Pars[1].Split(':')[1]));
                                    mt[j].SetFloat("_Smoothness", float.Parse(Pars[2].Split(':')[1]));
                                    continue;
                                }
                            }
                        }
                    }
                }
                //Debug.Log("材质个数:"+mt.Length+",,,,名字长度:"+str.Length+",,,,实际个数:"+count+",,,name:"+ child[i].name);

#if UNITY_EDITOR
                if (count > 0 && mt.Length > count)
                {
                   
                    for (int k = count; k < mt.Length; k++)
                    {
                        mt[k] = null;
                    }
                }
#else
   
#endif



                if (str[str.Length - 1].Substring(0, 1) == "N")
                {
                    IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().SetMap(mt[0], str[str.Length - 1],true);
                }
                child[i].materials = mt;

            }           

        }


        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        public void OnLoad(AssetLoaderContext assetLoaderContext)
        {

           _progressText.text = "";


            if (_loadedGameObject != null)
            {
                Destroy(_loadedGameObject);
            }
            _loadedGameObject = assetLoaderContext.RootGameObject;
            Debug.Log("BasePath="+assetLoaderContext.BasePath);
            Debug.Log("Name=" + _loadedGameObject.name);
          
           
            PlayerPrefs.SetString("SateName", _loadedGameObject.name);


            IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().GetCurrSelectModelName = _loadedGameObject.name;

            _loadedGameObject.layer = 6;
            //这部分内容要改成事件的形式
            IocContainer_InstanceMgr.GetInstance().GetInstance<Rotate>().center = _loadedGameObject;
            GameObjNodeJudge(_loadedGameObject);
            TestSam(_loadedGameObject);
            //CreateLight(_loadedGameObject);
            if (_loadedGameObject != null)
            {
                ///一键合拢和拆解的
                //ModelFunction.Self.Show();
               
                Camera.main.FitToBounds(assetLoaderContext.RootGameObject, 2f);
            }
        }

        /// <summary>
        /// 创建灯光并朝向物体的正前方
        /// </summary>
        /// <param name="target"></param>
        void CreateLight(GameObject target)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light lightComp = lightObj.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            // 设置光源的位置和旋转方向
            lightObj.transform.position = new Vector3(0f, 0f, -1.5f); // 设置光源的位置
            lightObj.transform.rotation = Quaternion.LookRotation(target.transform.forward, Vector3.up); // 朝向物体的正前方
        }


        void WriteByteToFile(byte[] data, string name)
        {
            string path = UnityEngine.Application.persistentDataPath + "/" + name;
            PlayerPrefs.SetString("" + name, path);
            Debug.Log("保存到本地的地址="+path);
            File.WriteAllBytes(path, data);          
        }



        /// <summary>
        /// 给物体添加boxclider
        /// </summary>
        void AddBoxCliderToGameObj(GameObject go)
        {
            
            MeshRenderer[] child = go.transform.GetComponentsInChildren<MeshRenderer>();
          
            for (int i = 0; i < child.Length; i++)
            {
                BoxCollider b = child[i].gameObject.AddComponent<BoxCollider>();
                //child[i].gameObject.AddComponent<MouseOverObject>();
               
            }
            Debug.Log("数据data="+ Shader.Find("Universal Render Pipeline/Lit"));
        }


        /// <summary>
        /// 判断物体根节点的字节点是否有一个
        /// </summary>
        /// <param name="obj"></param>
        void GameObjNodeJudge(GameObject obj)
        {
            if (obj.transform.childCount == 1 && obj.transform.GetChild(0).GetComponent<MeshRenderer>()==null)
            {
                GameObject go = obj.transform.GetChild(0).gameObject;
                go.tag = "SateParent";
                //go.AddComponent<ObjectExplosion>();
                AddBoxCliderToGameObj(go);
                SelectModelAttribute.GetInstance().GetCurrObj = go;
                SelectModelAttribute.GetInstance().GetIsHaveParent = true;
                SelectModelAttribute.GetInstance().GetOperateLevel = 0;
                IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().GetCurrOperateModel = SelectModelAttribute.GetInstance();
                ModelData.GetInstance().AddOperateGameObj(go);

               // IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().targetObject = go;
               // IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().ParentName = _loadedGameObject.name;
               // IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().ShowAllChildParam(_loadedGameObject.name);

                //IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().gameObject.SetActive(true);
                AddMouseOverObject(go);
            }
            else
            {

                AddMouseOverObject(obj);
                //obj.AddComponent<ObjectExplosion>();
                AddBoxCliderToGameObj(obj);
                obj.tag = "SateParent";
                SelectModelAttribute.GetInstance().GetOperateLevel = 0;
                SelectModelAttribute.GetInstance().GetCurrObj = obj;
                SelectModelAttribute.GetInstance().GetIsHaveParent = true;
                IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().GetCurrOperateModel = SelectModelAttribute.GetInstance();
                ModelData.GetInstance().AddOperateGameObj(obj);

                //IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().targetObject = obj;
               // IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().ParentName = _loadedGameObject.name;
                //IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().ShowAllChildParam(_loadedGameObject.name);

               // IocContainer_InstanceMgr.GetInstance().GetInstance<ShowHierarchyUI>().gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 给每个物体组件添加鼠标移入等事件
        /// </summary>
        /// <param name="obj"></param>
        void AddMouseOverObject(GameObject obj)
        {
            MeshRenderer[] child = obj.transform.GetComponentsInChildren<MeshRenderer>();
            //for (int i = 0; i < child.Length; i++)
            //{
            //    if (child[i].gameObject.GetComponent<MouseOverObject>() == null)
            //    {
            //        child[i].gameObject.AddComponent<MouseOverObject>();
            //    }
            //}
        }
    }
}
