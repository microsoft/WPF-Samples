//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration of AitEncoder
//----------------------------------------------------------------------------------------
#include "precomp.hpp"
static const GUID CLSID_AitEncoder             = { 0xEEC50239, 0x14BB, 0x4F70, { 0xAD, 0x2A, 0xB7, 0x12, 0x62, 0x19, 0xA6, 0xDC } };
//----------------------------------------------------------------------------------------
// UTILITY FUNCTIONS
//----------------------------------------------------------------------------------------

static ULARGE_INTEGER lastBlockPos = { 0 };

static HRESULT BeginBlock(IStream *stream, LPCSTR name, UINT frameNum)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    // Remember where we are
    if (SUCCEEDED(result))
    {
        LARGE_INTEGER zero = { 0 };
        result = stream->Seek(zero, STREAM_SEEK_CUR, &lastBlockPos);
    }

    // The number of bytes is stored as zero for now. The function EndBlock will write the
    // correct value later.
    UINT numBytes = 0;

    // Output the name
    if (SUCCEEDED(result))
    {
        result = OutputValues(stream, name, 4);                        
    }

    // Output the frame number
    if (SUCCEEDED(result))
    {
        result = OutputValue(stream, frameNum);
    }

    // Output the size
    if (SUCCEEDED(result))
    {
        result = OutputValue(stream, numBytes);
    }

    return result;
}

static HRESULT EndBlock(IStream *stream)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    // Remember where we are
    ULARGE_INTEGER curPos = { 0 };

    if (SUCCEEDED(result))
    {
        LARGE_INTEGER zero = { 0 };
        result = stream->Seek(zero, STREAM_SEEK_CUR, &curPos);
    }

    // Move to where the size of the last block is stored
    if (SUCCEEDED(result))
    {
        LARGE_INTEGER newPos;
        newPos.QuadPart = static_cast<LONGLONG>(lastBlockPos.QuadPart) + 2*sizeof(UINT);

        result = stream->Seek(newPos, STREAM_SEEK_SET, NULL);
    }

    // Store the size
    if (SUCCEEDED(result))
    {
        UINT size = static_cast<UINT>(curPos.QuadPart - lastBlockPos.QuadPart) - 3*sizeof(UINT);

        result = OutputValue(stream, size);
    }

    // Move back to where we should be
    if (SUCCEEDED(result))
    {
        LARGE_INTEGER cp;
        cp.QuadPart = static_cast<LONGLONG>(curPos.QuadPart);

        result = stream->Seek(cp, STREAM_SEEK_SET, NULL);
    }

    return result;
}

static HRESULT OutputBitmapSource(IStream *stream, LPCSTR name, UINT frameNum,
                                  double dpiX, double dpiY,
                                  IWICBitmapSource *source)
{
    HRESULT result = E_INVALIDARG;

    if (stream && source)
    {
        UINT width = 0, height = 0;
        UINT stride = 0;
        WICPixelFormatGUID pixelFormat;
        BYTE *pixels = NULL;

        result = S_OK;

        // Retrieve the dimensions of the source
        if (SUCCEEDED(result))
        {
            result = source->GetSize(&width, &height);
        }

        // Optionally retrieve the resolution
        if (SUCCEEDED(result) && (dpiX == 0 || dpiY == 0))
        {
            result = source->GetResolution(&dpiX, &dpiY);
        }

        // Get the pixel format
        if (SUCCEEDED(result))
        {
            result = source->GetPixelFormat(&pixelFormat);
        }

        // Get the stride
        if (SUCCEEDED(result))
        {
            stride = GetScanlineStride(width, pixelFormat);

            if (stride < 1)
            {
                result = E_UNEXPECTED;
            }
        }

        // Allocate the pixels
        if (SUCCEEDED(result))
        {
            pixels = new BYTE[stride * height];

            if (NULL == pixels)
            {
                result = E_OUTOFMEMORY;
            }
        }

        // Get the pixels
        if (SUCCEEDED(result))
        {
            WICRect rct;
            rct.X = 0;
            rct.Y = 0;
            rct.Width = width;
            rct.Height = height;

            result = source->CopyPixels(&rct, stride, stride * height, pixels);
        }

        // Output everything
        if (SUCCEEDED(result))
        {
            BeginBlock(stream, name, frameNum);
            OutputValue(stream, width);
            OutputValue(stream, height);
            OutputValue(stream, dpiX);
            OutputValue(stream, dpiY);
            OutputValue(stream, stride);
            OutputValue(stream, pixelFormat);
            OutputValues(stream, pixels, stride*height);
            result = EndBlock(stream);
        }

        if (pixels)
        {
            delete[] pixels;
        }
    }

    return result;
}

static HRESULT OutputBitmapPalette(IStream *stream, UINT frameNum, IWICPalette *palette)
{
    HRESULT result = E_INVALIDARG;

    if (stream && palette)
    {
        UINT colorCount = 0;
        WICColor *colors = NULL;

        result = S_OK;

        if (SUCCEEDED(result))
        {
            result = palette->GetColorCount(&colorCount);
        }

        if (colorCount > 0)
        {

            if (SUCCEEDED(result))
            {
                colors = new WICColor[colorCount];

                if (NULL == colors)
                {
                    result = E_OUTOFMEMORY;
                }
            }

            if (SUCCEEDED(result))
            {
                UINT cActualColors = 0;
                result = palette->GetColors(colorCount, colors, &cActualColors);
            }

            if (SUCCEEDED(result))
            {
                BeginBlock(stream, "PAL", frameNum);
                OutputValue(stream, colorCount);
                OutputValues(stream, colors, colorCount);
                result = EndBlock(stream);
            }

            if (colors)
            {
                delete[] colors;
            }
        }
    }

    return result;
}

static HRESULT OutputColorContext(IStream *stream, UINT frameNum, UINT colorContextCount, IWICColorContext **colorContext)
{
    HRESULT result = E_INVALIDARG;
    if (stream && colorContext)
    {
        BeginBlock(stream, "COL", frameNum);
        for(int i = 0; i < colorContextCount; i++)
        {
            UINT numBytes = 0;
            BYTE *bytes = NULL;
    
            result = S_OK;

            if (SUCCEEDED(result))
            {
                result = colorContext[i]->GetProfileBytes(0, NULL, &numBytes);
            }

            if (numBytes > 0)
            {
                if (SUCCEEDED(result))
                {
                    bytes = new BYTE[numBytes];
    
                    if (NULL == bytes)
                    {
                        result = E_OUTOFMEMORY;
                    }
                }

                if (SUCCEEDED(result))
                {
                    result = colorContext[i]->GetProfileBytes(numBytes, bytes, &numBytes);
                }

                if (SUCCEEDED(result))
                {                    
                    OutputValue(stream, numBytes);
                    OutputValues(stream, bytes, numBytes);                    
                }

                if (bytes)
                {
                    delete[] bytes;
                }
            }
       }
       result = EndBlock(stream);
    }
    return result;
}


//----------------------------------------------------------------------------------------
// FRAME ENCODE
//----------------------------------------------------------------------------------------

AitFrameEncode::AitFrameEncode(IWICImagingFactory *pIFactory, IStream *pIStream, UINT num)
    : BaseFrameEncode(pIFactory, pIStream, num)
{
   
}

AitFrameEncode::~AitFrameEncode()
{
}

STDMETHODIMP AitFrameEncode::Commit()
{
    HRESULT result = E_UNEXPECTED;
    result = BaseFrameEncode::Commit();

    if (SUCCEEDED(result))
    {
        result = OutputBitmapSource(stream, "FRA", frameNumber, destDpiX, destDpiY, destSource);
    }

    if (SUCCEEDED(result) && (NULL != destPalette))
    {
        result = OutputBitmapPalette(stream, frameNumber, destPalette);
    }

    if (SUCCEEDED(result) && (colorContexts.size() != 0))
    {
        IWICColorContext** ppIColorContext = new IWICColorContext*[colorContexts.size()];
        for (int i = 0; i < (UINT)colorContexts.size(); i++)
        {
            ppIColorContext[i] = colorContexts[i];            
        }  
        result = OutputColorContext(stream, frameNumber, (UINT)colorContexts.size(), ppIColorContext);
        delete[] ppIColorContext;
    }

    if (SUCCEEDED(result) && (NULL != destThumbnail))
    {
        result = OutputBitmapSource(stream, "THU", frameNumber, 0, 0, destThumbnail);
    }

    if (SUCCEEDED(result) && (NULL != destPreview))
    {
        result = OutputBitmapSource(stream, "PRE", frameNumber, 0, 0, destPreview);
    }

    return result;
}

STDMETHODIMP AitFrameEncode::GetMetadataQueryWriter(IWICMetadataQueryWriter **ppIMetadataQueryWriter)
{
    return E_NOTIMPL;
}


//----------------------------------------------------------------------------------------
// ENCODER
//----------------------------------------------------------------------------------------

AitEncoder::AitEncoder()
    : BaseEncoder(CLSID_AitEncoder, CLSID_AitContainer)
{
}

AitEncoder::~AitEncoder()
{  
}


STDMETHODIMP AitEncoder::Initialize(IStream *pIStream, WICBitmapEncoderCacheOption cacheOption)
{
    HRESULT result = E_INVALIDARG;

    if (pIStream)
    {
        result = S_OK;

        stream = pIStream;

        if (SUCCEEDED(result))
        {
            result = VerifyFactory();
        }

        // Remember the position of the header for later reference
        if (SUCCEEDED(result))
        {
            LARGE_INTEGER zero = { 0 };
            result = stream->Seek(zero, STREAM_SEEK_CUR, &headerPos);
        }

        // Write the header with a placeholder for the number of frames
        if (SUCCEEDED(result))
        {
            BeginBlock(stream, "AIT", 0xFFFFFFFF);            
            OutputValue(stream, (UINT)0);
            result = EndBlock(stream);
        }
    }    
    return result;
}

BaseFrameEncode* AitEncoder::CreateNewEncoderFrame(
        /* [in] */  IWICImagingFactory *pIFactory,
        /* [in] */ IStream *pIStream, 
        /* [in] */ UINT num)
{
    return new AitFrameEncode(factory, pIStream, frames.size());
};
STDMETHODIMP AitEncoder::Commit()
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        ULARGE_INTEGER curPos = { 0 };

        result = S_OK;

        // Go back and insert the number of frames
        
        // Remember this position
        if (SUCCEEDED(result))
        {
            LARGE_INTEGER zero = { 0 };
            result = stream->Seek(zero, STREAM_SEEK_CUR, NULL);
        }

        // Seek to the numFrames member of the header
        if (SUCCEEDED(result))
        {
            LARGE_INTEGER pos;
            pos.QuadPart = static_cast<LONGLONG>(headerPos.QuadPart) + 3*sizeof(UINT);

            result = stream->Seek(pos, STREAM_SEEK_SET, NULL);
        }

        // Write the proper value
        if (SUCCEEDED(result))
        {
            UINT numFrames = frames.size();

            result = OutputValue(stream, numFrames);
        }

        // Seek back to where we were
        if (SUCCEEDED(result))
        {
            LARGE_INTEGER pos;
            pos.QuadPart = static_cast<LONGLONG>(curPos.QuadPart);

            result = stream->Seek(pos, STREAM_SEEK_SET, NULL);
        }

        if (SUCCEEDED(result) && (NULL != destPalette))
        {
            result = OutputBitmapPalette(stream, 0xFFFFFFFF, destPalette);
        }

        if (SUCCEEDED(result) && (UINT)colorContexts.size() != 0)
        {
            IWICColorContext** ppIColorContext = new IWICColorContext*[colorContexts.size()];
            for (int i = 0; i < (UINT)colorContexts.size(); i++)
            {
                ppIColorContext[i] = colorContexts[i];            
            }  
            result = OutputColorContext(stream, 0xFFFFFFFF, colorContexts.size(), (IWICColorContext**)ppIColorContext);
            delete[] ppIColorContext;
        }

        if (SUCCEEDED(result) && (NULL != destThumbnail))
        {
            result = OutputBitmapSource(stream, "THU", 0xFFFFFFFF, 0, 0, destThumbnail);
        }

        if (SUCCEEDED(result) && (NULL != destPreview))
        {
            result = OutputBitmapSource(stream, "PRE", 0xFFFFFFFF, 0, 0, destPreview);
        }
    }

    return result;
}

STDMETHODIMP AitEncoder::GetMetadataQueryWriter(IWICMetadataQueryWriter **ppIMetadataQueryWriter)
{
    return E_NOTIMPL;
}
