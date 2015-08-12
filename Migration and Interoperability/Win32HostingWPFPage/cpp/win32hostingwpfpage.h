#pragma once

#include "resource.h"
#include "WPFPage.h"
#include <vcclr.h>
#include <string.h>

//Radiobutton IDs
#define IDC_ORIGINALBACKGROUND 1
#define IDC_LIGHTGREENBACKGROUND 2
#define IDC_LIGHTSALMONBACKGROUND 3
#define IDC_ORIGINALFOREGROUND 4
#define IDC_REDFOREGROUND 5
#define IDC_YELLOWFOREGROUND 6
#define IDC_ORIGINALFONTFAMILY 7
#define IDC_TIMESNEWROMAN 8
#define IDC_WINGDINGS 9
#define IDC_ORIGINALFONTSIZE 10
#define IDC_TENPOINT 11
#define IDC_TWELVEPOINT 12
#define IDC_ORIGINALFONTSTYLE 13
#define IDC_ITALIC 14
#define IDC_ORIGINALFONTWEIGHT 15
#define IDC_BOLD 16
public ref class WPFPageHost
{
public:
  WPFPageHost();
  static WPFPage^ hostedPage;
  //initial property settings
  static System::Windows::Media::Brush^ initBackBrush;
  static System::Windows::Media::Brush^ initForeBrush;
  static System::Windows::Media::FontFamily^ initFontFamily;
  static System::Windows::FontStyle initFontStyle;
  static System::Windows::FontWeight initFontWeight;
  static double initFontSize;
};

RECT rect;
HWND GetHwnd(HWND parent, int x, int y, int width, int height);
HWND dataDisplayLabel,  nameLabel, addressLabel, cityLabel, stateLabel, zipLabel;
HWND controlDisplayLabel,  foregroundLabel, fontLabel, sizeLabel, styleLabel, weightLabel;
HWND backgroundGroup, originalBackgroundButton, lightGreenBackgroundButton, lightSalmonBackgroundButton;
HWND foregroundGroup, originalForegroundButton, redForegroundButton, yellowForegroundButton;
HWND fontFamilyGroup, originalFontFamily, timesFontFamily, wingdingsFontFamily;
HWND fontSizeGroup, originalFontSize, tenpointFontSize, twelvepointFontSize;
HWND fontStyleGroup, originalFontStyle, italicFontStyle;
HWND fontWeightGroup, originalFontWeight, boldFontWeight;
void WPFButtonClicked(Object ^sender, MyPageEventArgs ^args);
void CreateDataDisplay(HWND hWnd, int top, int left, int width);
void CreateRadioButtons(HWND hWnd);