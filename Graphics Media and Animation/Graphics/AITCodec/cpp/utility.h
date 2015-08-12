//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------
//  Description: Declaration of Utility/Helper functions
//----------------------------------------------------------------------------------------
#pragma once

//----------------------------------------------------------------------------------------
// MISC UTILITY FUNCTIONS
//----------------------------------------------------------------------------------------
UINT GetScanlineStride(UINT width, REFWICPixelFormatGUID pixelFormat);

//----------------------------------------------------------------------------------------
// STREAM UTILITY FUNCTIONS
//----------------------------------------------------------------------------------------
template< typename T >
static HRESULT InputValue(IStream *stream, T &val)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    ULONG numRead = 0;

    if (SUCCEEDED(result))
    {
        result = stream->Read(&val, sizeof(T), &numRead);
    }

    if (SUCCEEDED(result))
    {
        result = (sizeof(T) == numRead) ? S_OK : E_UNEXPECTED;
    }

    return result;
}

template< typename T >
static HRESULT InputValues(IStream *stream, T ptr, UINT count)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    ULONG numRead = 0;
    ULONG numToRead = sizeof(*ptr) * count;

    if (SUCCEEDED(result))
    {
        result = stream->Read(ptr, numToRead, &numRead);
    }

    if (SUCCEEDED(result))
    {
        result = (numToRead == numRead) ? S_OK : E_UNEXPECTED;
    }

    return result;
}

template< typename T >
static HRESULT OutputValue(IStream *stream, T val)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    ULONG numWritten = 0;

    if (SUCCEEDED(result))
    {
        result = stream->Write(&val, sizeof(T), &numWritten);
    }

    if (SUCCEEDED(result))
    {
        result = (sizeof(T) == numWritten) ? S_OK : E_UNEXPECTED;
    }

    return result;
}

template< typename T >
static HRESULT OutputValues(IStream *stream, T ptr, UINT count)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    ULONG numWritten = 0;
    ULONG numToWritten = sizeof(*ptr) * count;

    if (SUCCEEDED(result))
    {
        result = stream->Write(ptr, numToWritten, &numWritten);
    }

    if (SUCCEEDED(result))
    {
        result = (numToWritten == numWritten) ? S_OK : E_UNEXPECTED;
    }

    return result;
}
