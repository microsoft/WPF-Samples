//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------
//  Description: Declaration and Definition of TestClassFactory
//----------------------------------------------------------------------------------------
#pragma once

#include "UnknownImpl.h"

template< typename T >
class TestClassFactory : public IClassFactory
{
public:
    // IUnknown Interface
    STDMETHOD(QueryInterface)(REFIID riid, void **ppv)
    {
        HRESULT result = E_INVALIDARG;

        if (ppv)
        {
            if (riid == IID_IUnknown)
            {
                *ppv = static_cast<IUnknown*>(this);
                AddRef();

                result = S_OK;
            }
            else if (riid == IID_IClassFactory)
            {
                *ppv = static_cast<IClassFactory*>(this);
                AddRef();

                result = S_OK;
            }
            else
            {
                result = E_NOINTERFACE;
            }
        }

        return result;
    }

    STDMETHOD_(ULONG, AddRef)()
    {
        return unknownImpl.AddRef();
    
    }

    STDMETHOD_(ULONG, Release)()
    {
        ULONG result = unknownImpl.Release();
        if (0 == result)
        {
            delete this;
        }
        return result;
    }

    // IClassFactory Interface
    STDMETHOD(CreateInstance)(IUnknown *pUnkOuter, REFIID riid, void **ppv)
    {
        HRESULT result = E_INVALIDARG;

        if (NULL != ppv)
        {
            T *obj = new T();

            if (NULL != obj)
            {
                result = obj->QueryInterface(riid, ppv);
            }
            else
            {
                *ppv = NULL;
                result = E_OUTOFMEMORY;
            }
        }

        return result;
    }

    STDMETHOD(LockServer)(BOOL fLock)
    {
        return CoLockObjectExternal(this, fLock, FALSE);
    }

private:
    UnknownImpl unknownImpl;
};

