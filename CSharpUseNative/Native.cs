using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssetBundleOperation.CSharpUseNative
{
    public static class Native
    {
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr AtriGetLastError();
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LoadAssetBundleByPath(string path,bool fastread,ref long selfindex);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnLoadAssetBundle(IntPtr ptr);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SaveAssetBundle(IntPtr ptr,string path,bool fastcall);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCallBackProgress();
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetCallBackInfo();
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetUnSerializeValue(IntPtr assetbundle, long FileId, string Path);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetTypeTree(IntPtr assetbundle, long FileId);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPtrValue(IntPtr src, IntPtr dest,int len);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPtrValue(string src, IntPtr dest, int len);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool GetMonoObject(IntPtr assetbundle,ref long FileId);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long ObjectInfoYield(IntPtr assetbundle, long FileId);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetResourcesPath(IntPtr assetbundle, int index);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SetResourcesPath(IntPtr assetbundle,string newpath, int index);
        [DllImport("AssetBundleOperationNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetResourcesCount(IntPtr assetbundle);
        public static string GetLastError()
        {
            return Marshal.PtrToStringAnsi(AtriGetLastError());
        }
    }
}
