//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------
//  Description: Declaration and Definition of TestUnknown
//----------------------------------------------------------------------------------------
#pragma once


class UnknownImpl
{
private:
    int numReferences;

public:
    UnknownImpl()
        : numReferences(0)
    {}

    ULONG STDMETHODCALLTYPE AddRef()
    {
        return ++numReferences;
    }

    ULONG STDMETHODCALLTYPE Release()
    {
        ULONG result;

        if (numReferences > 0)
        {
            --numReferences;
            result = numReferences;
        }
        else
        {
            result = numReferences = 0;
        }

        return result;
    }
};
