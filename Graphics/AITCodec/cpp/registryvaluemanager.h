//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------
//  Description: Declaration and Definition of RegistryValueManager
//----------------------------------------------------------------------------------------
#pragma once


class RegistryValueManager
{
public:
    HRESULT Register()
    {
        return PopulateValues();
    }

    HRESULT Unregister()
    {
        HRESULT result = S_OK;
        Register();
        while (keys.size() > 0)
        {
            RegDeleteKeyA(HKEY_CLASSES_ROOT, keys.back());
            keys.pop_back();
        }

        return result;
    }

protected:
    virtual HRESULT PopulateValues() = 0;

    HRESULT Val(LPCSTR keyName, LPCSTR valueName, LPCSTR value)
    {
        return Val(keyName, valueName, REG_SZ, reinterpret_cast<const BYTE*>(value), strlen(value) + 1);
    }

    HRESULT Val(LPCSTR keyName, LPCSTR valueName, DWORD value)
    {
        return Val(keyName, valueName, REG_DWORD, reinterpret_cast<const BYTE*>(&value), sizeof(DWORD));
    }

    HRESULT Val(LPCSTR keyName, LPCSTR valueName, const BYTE *data, DWORD size)
    {
        return Val(keyName, valueName, REG_BINARY, data, size);
    }

private:
    HRESULT Val(LPCSTR keyName, LPCSTR valueName, DWORD type, const BYTE *data, DWORD size)
    {
        HRESULT result = S_OK;
        HKEY hkey;

        keys.push_back(keyName);
        long err = RegCreateKeyA(HKEY_CLASSES_ROOT, keyName, &hkey);
        if (ERROR_SUCCESS == err)
        {
            err = RegSetValueExA(hkey, valueName, 0, type, data, size);
            RegCloseKey(hkey);
        }
        else
        {
            result = E_FAIL;
        }
        return result;
    }
    std::vector<LPCSTR> keys;
};
