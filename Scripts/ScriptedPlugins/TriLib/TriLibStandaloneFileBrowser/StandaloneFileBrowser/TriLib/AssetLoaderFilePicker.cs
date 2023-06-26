#pragma warning disable 618

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriLibCore.SFB;
using TriLibCore.General;
using TriLibCore.Mappers;
using TriLibCore.Utils;
using UnityEngine;
using Unity.VisualScripting;
using System.IO;
using TriLibCore.Extensions;
using TriLibCore.Samples;
using UnityEditor;
using Newtonsoft.Json;

namespace TriLibCore
{
    /// <summary>Represents an Asset Loader which loads files using a platform-specific file picker.</summary>
    public class AssetLoaderFilePicker : MonoBehaviour
    {
        private IList<ItemWithStream> _items;
        private string _modelExtension;
        private Action<AssetLoaderContext> _onLoad;
        private Action<AssetLoaderContext> _onMaterialsLoad;
        private Action<AssetLoaderContext, float> _onProgress;
        private Action<IContextualizedError> _onError;
        private Action<bool> _onBeginLoad;
        private GameObject _wrapperGameObject;
        private AssetLoaderOptions _assetLoaderOptions;
        private bool _haltTask;

        /// <summary>Creates the Asset Loader File Picker Singleton instance.</summary>
        /// <returns>The created AssetLoaderFilePicker.</returns>
        public static AssetLoaderFilePicker Create()
        {
            var gameObject = new GameObject("AssetLoaderFilePicker");
            var assetLoaderFilePicker = gameObject.AddComponent<AssetLoaderFilePicker>();
            return assetLoaderFilePicker;
        }

        /// <summary>Loads a Model from the OS file picker asynchronously, or synchronously when the OS doesn't support Threads.</summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="onLoad">The Method to call on the Main Thread when the Model is loaded but resources may still pending.</param>
        /// <param name="onMaterialsLoad">The Method to call on the Main Thread when the Model and resources are loaded.</param>
        /// <param name="onProgress">The Method to call when the Model loading progress changes.</param>
        /// <param name="onBeginLoad">The Method to call when the model begins to load.</param>
        /// <param name="onError">The Method to call on the Main Thread when any error occurs.</param>
        /// <param name="wrapperGameObject">The Game Object that will be the parent of the loaded Game Object. Can be null.</param>
        /// <param name="assetLoaderOptions">The options to use when loading the Model.</param>
        /// <param name="haltTask">Turn on this field to avoid loading the model immediately and chain the Tasks.</param>
        public void LoadModelFromFilePickerAsync(string title, Action<AssetLoaderContext> onLoad, Action<AssetLoaderContext> onMaterialsLoad, Action<AssetLoaderContext, float> onProgress, Action<bool> onBeginLoad, Action<IContextualizedError> onError, GameObject wrapperGameObject, AssetLoaderOptions assetLoaderOptions, bool haltTask = false)
        {
            _onLoad = onLoad;
            _onMaterialsLoad = onMaterialsLoad;
            _onProgress = onProgress;
            _onError = onError;
            _onBeginLoad = onBeginLoad;
            _wrapperGameObject = wrapperGameObject;
            _assetLoaderOptions = assetLoaderOptions;
            _haltTask = haltTask;
            try
            {
				StandaloneFileBrowser.OpenFilePanelAsync(title, null, GetExtensions(), true, OnItemsWithStreamSelected);
            }
            catch (Exception)
            {
                Dispatcher.InvokeAsync(DestroyMe);
                throw;
            }
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }

        private void HandleFileLoading()
        {
            StartCoroutine(DoHandleFileLoading());

        }

        private void NewHandleFileLoading()
        {
            StartCoroutine(NewDoHandleFileLoading());

        }


        private void Start()
        {
            IocContainer_InstanceMgr.GetInstance().RegistInstance<AssetLoaderFilePicker>(this);
          
        }
        /// <summary>
        /// 从本地下载文件
        /// </summary>
        public void ComeLocalDownStream(Stream stre,string filename)
        {
            Debug.Log("从本地下载文件="+filename);
            _onLoad = OnLoad;
            _onMaterialsLoad = OnMaterialsLoad; 
            _onProgress = OnProgress;
            _onError = OnError;
            _modelExtension = "fbx";
            AssetLoader.LoadModelFromStream(stre, filename, _modelExtension, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), _haltTask);
        }


        private void OnBeginLoad(bool filesSelected)
        {
           
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
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>()._progressText.text = $"Progress: {progress:P}";
            if (progress==1)
            {
                IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>()._progressText.text = "";
            }
        }

        void TestSam(GameObject go)
        {
            MeshRenderer[] child = go.transform.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < child.Length; i++)
            {
                string[] str = child[i].name.Split('_');
                int count = str.Length - 1;
                if (str[str.Length - 1].Substring(0, 1) == "N")
                {
                    count--;
                }
                else if (str[str.Length - 1].Substring(0, 1) == "M")
                {
                    count--;
                }

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

                        Color al = mt[j].GetColor("_Color");
                        mt[j].shader = Shader.Find("Universal Render Pipeline/Lit");
                        mt[j].SetColor("_BaseColor", al);

                        if (str.Length > 1 && j < 4)
                        {
                            IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().SetMap(mt[j], str[j + 1], false);
                            foreach (var item in IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().allModelShaderData)
                            {
                                string[] Pars = item.Split(",");
                                if (Pars[0] == str[0])
                                {
                                    mt[j].SetFloat("_Metallic", float.Parse(Pars[1].Split(':')[1]));
                                    mt[j].SetFloat("_Smoothness", float.Parse(Pars[2].Split(':')[1]));
                                    Debug.Log("相等的数据-other:" + str[0]);
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
                    IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().SetMap(mt[0], str[str.Length - 1], true);
                }
                child[i].materials = mt;
            }

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
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>()._progressText.text = "";
        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>()._progressText.text = "";

            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>().OnLoad(assetLoaderContext); 
            Debug.Log("File-Name=" + assetLoaderContext.RootGameObject.name);
           
        }

        /// <summary>
        /// 将模型数据保存到本地
        /// </summary>
        /// <param name="filePath"></param>
        void SaveModeData(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            //string savePath = Path.Combine(Application.persistentDataPath, Path.GetFileName(filePath));
            string savePath = UnityEngine.Application.persistentDataPath + "/" + Path.GetFileName(filePath);

            File.WriteAllBytes(savePath, fileBytes);
            PlayerPrefs.SetString("NewSate", savePath);
        }

        private IEnumerator DoHandleFileLoading()
        {
            var hasFiles = _items != null && _items.Count > 0 && _items[0].HasData;
            _onBeginLoad?.Invoke(hasFiles);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (!hasFiles)
            {
                DestroyMe();
                yield break;
            }
            var modelFileWithStream = FindModelFile();
            var modelFilename = modelFileWithStream.Name;

            string[] PathNames = modelFilename.Split('/');
            if (PathNames[PathNames.Length - 1].Split('.')[1] == "prt")
            {
                Debug.Log("模型格式为prt");
                IocContainer_InstanceMgr.GetInstance().GetInstance<CallMaxLoadProE>().Call3DMax(modelFilename);
            }
            else
            {
                SaveModeData(modelFilename);
                string name = Path.GetFileName(modelFilename);

                ///插入数据到本地数据库
               // IocContainer_InstanceMgr.GetInstance().GetInstance<SQLiteDemo>().Insert((name.Split('.')[0]), modelFilename, "model");


                //RequestGameAddModel person = new RequestGameAddModel { UserId = ModelData.GetInstance().userId, Name = (name.Split('.')[0]),Path= modelFilename };
                //string json = JsonConvert.SerializeObject(person);
                //string str = "3+" + json;
                //IocContainer.GetInstance().GetInstance<UDPClient>().SocketSend(str);

                //IocContainer.GetInstance().GetInstance<UDPClient>().SocketSend("2,InsertModel,"+ ModelData.GetInstance().userId+","+ (name.Split('.')[0]) +","+modelFilename);
                Debug.Log("成功后的=" + modelFilename);
                string parentPath = Path.GetDirectoryName(modelFilename); // 获取Assets文件夹上级目录路径
                string path = parentPath;
                path = path.Replace("\\", "/");
                //设置模型所在的文件夹的文件夹路径
                IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().folderPath = path;
                IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().LoadModelAllMap();

                var modelStream = modelFileWithStream.OpenStream();
                if (_assetLoaderOptions == null)
                {
                    _assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                }
                _assetLoaderOptions.TextureMapper = ScriptableObject.CreateInstance<FilePickerTextureMapper>();
                _assetLoaderOptions.ExternalDataMapper = ScriptableObject.CreateInstance<FilePickerExternalDataMapper>();
                _modelExtension = modelFilename != null ? FileUtils.GetFileExtension(modelFilename, false) : null;
                if (_modelExtension == "zip")
                {
                    Debug.Log("Time=" + modelStream);
                    if (modelStream != null)
                    {
                        AssetLoaderZip.LoadModelFromZipStream(modelStream, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), null, false, modelFilename);
                    }
                    else
                    {
                        AssetLoaderZip.LoadModelFromZipFile(modelFilename, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), null);
                    }
                }
                else
                {
                    Debug.Log("Time11=" + modelStream + ",,,,modelFilename=" + modelFilename + ",,,,_modelExtension=" + _modelExtension);
                    if (modelStream != null)
                    {
                        AssetLoader.LoadModelFromStream(modelStream, modelFilename, _modelExtension, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), _haltTask);
                    }
                    else
                    {
                        AssetLoader.LoadModelFromFile(modelFilename, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), _haltTask);
                    }
                }
                //DestroyMe();
            }

           
        }


        private IEnumerator NewDoHandleFileLoading()
        {
           
            yield return new WaitForEndOfFrame();
          
            var modelFileWithStream = FindModelFile();
            var modelFilename = modelFileWithStream.Name;
            var modelStream = modelFileWithStream.OpenStream();
            if (_assetLoaderOptions == null)
            {
                _assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            }
            _assetLoaderOptions.TextureMapper = ScriptableObject.CreateInstance<FilePickerTextureMapper>();
            _assetLoaderOptions.ExternalDataMapper = ScriptableObject.CreateInstance<FilePickerExternalDataMapper>();
            _modelExtension = modelFilename != null ? FileUtils.GetFileExtension(modelFilename, false) : null;
            if (_modelExtension == "zip")
            {
                if (modelStream != null)
                {
                    AssetLoaderZip.LoadModelFromZipStream(modelStream, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), null, false, modelFilename);
                }
                else
                {
                    AssetLoaderZip.LoadModelFromZipFile(modelFilename, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), null);
                }
            }
            else
            {
                if (modelStream != null)
                {
                    AssetLoader.LoadModelFromStream(modelStream, modelFilename, _modelExtension, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), _haltTask);
                }
                else
                {
                    AssetLoader.LoadModelFromFile(modelFilename, _onLoad, _onMaterialsLoad, _onProgress, _onError, _wrapperGameObject, _assetLoaderOptions, CustomDataHelper.CreateCustomDataDictionaryWithData(_items), _haltTask);
                }
            }
            DestroyMe();
        }


        private static ExtensionFilter[] GetExtensions()
        {
            var extensions = Readers.Extensions;
            var extensionFilters = new List<ExtensionFilter>();
            var subExtensions = new List<string>();
            for (var i = 0; i < extensions.Count; i++)
            {
                var extension = extensions[i];
                extensionFilters.Add(new ExtensionFilter(null, extension));
                subExtensions.Add(extension);
            }

            subExtensions.Add("zip");
            subExtensions.Add("prt");
            extensionFilters.Add(new ExtensionFilter(null, new[] { "zip" }));
            extensionFilters.Add(new ExtensionFilter("All Files", new[] { "*" }));
            extensionFilters.Insert(0, new ExtensionFilter("Accepted Files", subExtensions.ToArray()));
            return extensionFilters.ToArray();
        }
        
        private ItemWithStream FindModelFile()
        {
            if (_items.Count == 1)
            {
                return _items.First();
            }
            var extensions = Readers.Extensions;
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                if (item.Name == null)
                {
                    continue;
                }

                var extension = FileUtils.GetFileExtension(item.Name, false);
                if (extensions.Contains(extension))
                {
                    return item;
                }
            }

            return null;
        }


        private ItemWithStream NewFindModelFile()
        {
            if (_items.Count == 1)
            {
                return _items.First();
            }
            var extensions = Readers.Extensions;
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                if (item.Name == null)
                {
                    continue;
                }

                var extension = FileUtils.GetFileExtension(item.Name, false);
                if (extensions.Contains(extension))
                {
                    return item;
                }
            }

            return null;
        }


        private void OnItemsWithStreamSelected(IList<ItemWithStream> itemsWithStream)
        {
			if (itemsWithStream != null)
            {
                _items = itemsWithStream;
                Debug.Log("数字个数="+_items.Count);
                Dispatcher.InvokeAsync(HandleFileLoading);
            } else {
                DestroyMe();
            }    
        }
    }
}
