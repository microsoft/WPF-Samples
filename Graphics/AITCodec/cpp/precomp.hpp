//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------
//  Description: Collection of precompiled headers for the TestCodec
//----------------------------------------------------------------------------------------

#pragma once

#include <windows.h>
#include <shellapi.h>
//#include <tchar.h>
#include <strsafe.h>
#include <winerror.h>
#include <comip.h>
#include <assert.h>
#include <vector>
#include <map>

#include <algorithm>

#include <typeinfo>

#include <wincodec.h>
#include <wincodecsdk.h>

#include "Utility.h"

#include "RegistryValueManager.h"
#include "AitCodecRegistryManager.h"
#include "ClassFactory.h"
#include "BaseDecoder.h"
#include "BaseEncoder.h"
#include "BasePixelFormatConverter.h"

#include "AitDecoder.h"
#include "AitEncoder.h"
#include "YCbCrPixelFormatConverter.h"
#include "CMYKPixelFormatConverter.h"


