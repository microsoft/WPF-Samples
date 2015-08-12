//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: DLL Entrypoint for TestCodec
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

HINSTANCE g_DllInstance;
TestCodecsRegistryManager rvm;

extern "C"
STDAPI DllRegisterServer()
{    
    rvm.Register();
    return S_OK;
}

extern "C"
STDAPI DllUnregisterServer()
{ 
    rvm.Unregister();
    return S_OK;
}

extern "C"
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, void **ppv)
{
    HRESULT result = E_INVALIDARG; 

    if (NULL != ppv)
    {
        IClassFactory *classFactory = NULL;
        if (CLSID_YCbCrPixelFormatConverter == rclsid)
        {
            result = S_OK;
            classFactory = new TestClassFactory<YCbCrPixelFormatConverter>();
        }
        else if (CLSID_CmykPixelFormatConverter == rclsid)
        {
            result = S_OK;
            classFactory = new TestClassFactory<CmykPixelFormatConverter>();
        }
        else if (CLSID_AitDecoder == rclsid)
        {
            result = S_OK;
            classFactory = new TestClassFactory<AitDecoder>();
        }
        else if (CLSID_AitEncoder == rclsid)
        {
            result = S_OK;
            classFactory = new TestClassFactory<AitEncoder>();
        }        
        else
        {
            result = E_NOINTERFACE;
        }

        if (SUCCEEDED(result))
        {
            if (NULL != classFactory)
            {
                result = classFactory->QueryInterface(riid, ppv);
            }
            else
            {
                result = E_OUTOFMEMORY;
            }
        }
    }
    return result;    
}

extern "C"
BOOL DllMain(HINSTANCE hinstDLL, ULONG fdwReason, LPVOID *lpvReserved)
{
    BOOL result = TRUE;

    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
        g_DllInstance = hinstDLL;
        DisableThreadLibraryCalls(hinstDLL);
        break;

    case DLL_PROCESS_DETACH:
        break;
    }

    return result;
}