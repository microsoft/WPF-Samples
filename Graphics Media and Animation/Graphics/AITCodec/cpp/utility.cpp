//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of Utility/Helper functions
//----------------------------------------------------------------------------------------

#include "precomp.hpp"

//----------------------------------------------------------------------------------------
// MISC UTILITY FUNCTIONS
//----------------------------------------------------------------------------------------
UINT GetScanlineStride(UINT width, REFWICPixelFormatGUID pixelFormat)
{
    UINT bpp = 0;

    if ((GUID_WICPixelFormat1bppIndexed == pixelFormat) ||
        (GUID_WICPixelFormatBlackWhite == pixelFormat))
    {
        bpp = 1;
    }
    else if ((GUID_WICPixelFormat2bppIndexed == pixelFormat) ||
             (GUID_WICPixelFormat2bppGray == pixelFormat))
    {
        bpp = 2;
    }
    else if ((GUID_WICPixelFormat4bppIndexed == pixelFormat) ||
             (GUID_WICPixelFormat4bppGray == pixelFormat))
    {
        bpp = 4;
    }
    else if ((GUID_WICPixelFormat8bppIndexed == pixelFormat) ||
             (GUID_WICPixelFormat8bppGray == pixelFormat))
    {
        bpp = 8;
    }
    else if ((GUID_WICPixelFormat16bppBGR555 == pixelFormat) ||
             (GUID_WICPixelFormat16bppBGR565 == pixelFormat) ||
             (GUID_WICPixelFormat16bppGray == pixelFormat)   ||
             (GUID_WICPixelFormat16bppGrayFixedPoint == pixelFormat))
    {
        bpp = 16;
    }
    else if ((GUID_WICPixelFormat24bppBGR == pixelFormat) ||
             (GUID_WICPixelFormat24bppRGB == pixelFormat))
    {
        bpp = 24;
    }
    else if ((GUID_WICPixelFormat32bppBGR == pixelFormat)       ||
             (GUID_WICPixelFormat32bppBGRA == pixelFormat)      ||
             (GUID_WICPixelFormat32bppPBGRA == pixelFormat)     ||
             (GUID_WICPixelFormat32bppGrayFloat == pixelFormat) ||
             (GUID_WICPixelFormat32bppBGR101010 == pixelFormat) ||
             (GUID_WICPixelFormat32bppCMYK == pixelFormat))
    {
        bpp = 32;
    }
    else if ((GUID_WICPixelFormat48bppRGBFixedPoint == pixelFormat) ||
             (GUID_WICPixelFormat48bppRGB == pixelFormat))
    {
        bpp = 48;
    }
    else if ((GUID_WICPixelFormat64bppRGBA == pixelFormat) ||
             (GUID_WICPixelFormat64bppPRGBA == pixelFormat))
    {
        bpp = 64;
    }
    else if ((GUID_WICPixelFormat96bppRGBFixedPoint == pixelFormat))
    {
        bpp = 96;
    }
    else if ((GUID_WICPixelFormat128bppRGBAFloat == pixelFormat)  ||
             (GUID_WICPixelFormat128bppPRGBAFloat == pixelFormat) ||
             (GUID_WICPixelFormat128bppRGBFloat == pixelFormat))
    {
        bpp = 128;
    }	

    return (bpp * width + 7)/8;
};


