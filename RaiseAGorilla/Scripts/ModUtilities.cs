﻿using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace RaiseAGorilla.Scripts
{
    internal class ModUtilities
    {
        #region Asset Utilities
        public static async Task<AssetBundle> LoadFromStream(string name)
        {
            Stream assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();
            var request = AssetBundle.LoadFromStreamAsync(assetStream);
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleCreateRequest;
                taskCompletionSource.SetResult(outRequest.assetBundle);
            };
            return await taskCompletionSource.Task;
        }

        public static async Task<AssetBundle> LoadFromStream(Stream assetStream)
        {
            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();
            var request = AssetBundle.LoadFromStreamAsync(assetStream);
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleCreateRequest;
                taskCompletionSource.SetResult(outRequest.assetBundle);
            };
            return await taskCompletionSource.Task;
        }

        public static async Task<AssetBundle> LoadFromFile(string path)
        {
            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();
            var request = AssetBundle.LoadFromFileAsync(path);
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleCreateRequest;
                taskCompletionSource.SetResult(outRequest.assetBundle);
            };
            return await taskCompletionSource.Task;
        }

        public static async Task<T> LoadAsset<T>(AssetBundle bundle, string name) where T : UnityEngine.Object
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            var request = bundle.LoadAssetAsync<T>(name);
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleRequest;
                if (outRequest.asset == null)
                {
                    taskCompletionSource.SetResult(null);
                    return;
                }

                taskCompletionSource.SetResult(outRequest.asset as T);
            };
            return await taskCompletionSource.Task;
        }

        public static async Task<T[]> LoadAllAssets<T>(AssetBundle bundle) where T : UnityEngine.Object
        {
            var taskCompletionSource = new TaskCompletionSource<T[]>();
            var request = bundle.LoadAllAssetsAsync<T>();
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleRequest;
                if (outRequest.allAssets == null)
                {
                    taskCompletionSource.SetResult(null);
                    return;
                }

                taskCompletionSource.SetResult(outRequest.allAssets as T[]);
            };
            return await taskCompletionSource.Task;
        }

        public static async Task<UnityEngine.Object[]> LoadAllAssets(AssetBundle bundle)
        {
            var taskCompletionSource = new TaskCompletionSource<UnityEngine.Object[]>();
            var request = bundle.LoadAllAssetsAsync();
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleRequest;
                if (outRequest.allAssets == null)
                {
                    taskCompletionSource.SetResult(null);
                    return;
                }

                taskCompletionSource.SetResult(outRequest.allAssets);
            };
            return await taskCompletionSource.Task;
        }

        public static async Task<T[]> LoadAssetWithSubAssetsAsync<T>(AssetBundle bundle, string name) where T : UnityEngine.Object
        {
            var taskCompletionSource = new TaskCompletionSource<T[]>();
            var request = bundle.LoadAssetWithSubAssetsAsync<T>(name);
            request.completed += operation =>
            {
                var outRequest = operation as AssetBundleRequest;
                if (outRequest.asset == null)
                {
                    taskCompletionSource.SetResult(null);
                    return;
                }

                taskCompletionSource.SetResult(outRequest.allAssets as T[]);
            };
            return await taskCompletionSource.Task;
        }
        #endregion
    }
}
