// Win32HostingWPFPage.cpp : Defines the entry point for the application.
//
using namespace System;
using namespace System::Windows;
using namespace System::Windows::Controls;
using namespace System::Windows::Media;
using namespace System::Runtime;

#include "stdafx.h"
#include "Win32HostingWPFPage.h"
#include <gcroot.h>

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
TCHAR szTitle[MAX_LOADSTRING];                    // The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name
HWND wpfHwnd; //The hwnd associated with the hosted WPF page

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
[System::STAThreadAttribute] //Needs to be an STA thread to play nicely with WPF
int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: Place code here.

    MSG msg;
    HACCEL hAccelTable;

    // Initialize global strings
    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInstance, IDC_WIN32HOSTINGWPFPAGE, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WIN32HOSTINGWPFPAGE));

    // Main message loop:
    while (GetMessage(&msg, NULL, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style         = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc   = WndProc;
    wcex.cbClsExtra    = 0;
    wcex.cbWndExtra    = 0;
    wcex.hInstance     = hInstance;
    wcex.hIcon         = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WIN32HOSTINGWPFPAGE));
    wcex.hCursor       = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_BTNSHADOW);
    wcex.lpszMenuName  = MAKEINTRESOURCE(IDC_WIN32HOSTINGWPFPAGE);
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm       = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    HWND hWnd;

    hInst = hInstance; // Store instance handle in our global variable

    hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

    if (!hWnd)
    {
        return FALSE;
    }

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND    - process the application menu
//  WM_PAINT    - Paint the main window
//  WM_DESTROY    - post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    int wmId, wmEvent;
    PAINTSTRUCT ps;
    HDC hdc;

    switch (message)
    {
      case WM_CREATE :
        GetClientRect(hWnd, &rect);
        wpfHwnd = GetHwnd(hWnd, rect.right-375, 0, 375, 250);
        CreateDataDisplay(hWnd, 275, rect.right-375, 375);
        CreateRadioButtons(hWnd);
      break;
      case WM_COMMAND:
        wmId    = LOWORD(wParam);
        wmEvent = HIWORD(wParam);

        switch (wmId)
        {
        //Menu selections
          case IDM_ABOUT:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
          break;
          case IDM_EXIT:
            DestroyWindow(hWnd);
          break;
          //RadioButtons
          case IDC_ORIGINALBACKGROUND :
            WPFPageHost::hostedPage->Background = WPFPageHost::initBackBrush;
          break;
          case IDC_LIGHTGREENBACKGROUND :
            WPFPageHost::hostedPage->Background = gcnew SolidColorBrush(Colors::LightGreen);
          break;
          case IDC_LIGHTSALMONBACKGROUND :
            WPFPageHost::hostedPage->Background = gcnew SolidColorBrush(Colors::LightSalmon);
          break;

          case IDC_ORIGINALFONTFAMILY :
            WPFPageHost::hostedPage->DefaultFontFamily = WPFPageHost::initFontFamily;
          break;
          case IDC_TIMESNEWROMAN :
            WPFPageHost::hostedPage->DefaultFontFamily = gcnew FontFamily("Times New Roman");
          break;
          case IDC_WINGDINGS:
            WPFPageHost::hostedPage->DefaultFontFamily = gcnew FontFamily("WingDings");
          break;
          case IDC_ORIGINALFONTSTYLE :
            WPFPageHost::hostedPage->DefaultFontStyle = WPFPageHost::initFontStyle;
          break;
          case IDC_ITALIC :
            WPFPageHost::hostedPage->DefaultFontStyle = System::Windows::FontStyles::Italic;
          break;
          case IDC_ORIGINALFONTSIZE :
            WPFPageHost::hostedPage->DefaultFontSize = WPFPageHost::initFontSize;
          break;
          case IDC_TENPOINT :
            WPFPageHost::hostedPage->DefaultFontSize = 10;
          break;
          case IDC_TWELVEPOINT :
            WPFPageHost::hostedPage->DefaultFontSize = 12;
          break;
          case IDC_ORIGINALFONTWEIGHT :
            WPFPageHost::hostedPage->DefaultFontWeight = WPFPageHost::initFontWeight;
          break;
          case IDC_BOLD :
            WPFPageHost::hostedPage->DefaultFontWeight = FontWeights::Bold;
          break;
          case IDC_ORIGINALFOREGROUND :
            WPFPageHost::hostedPage->DefaultForeBrush = WPFPageHost::initForeBrush;
          break;
          case IDC_REDFOREGROUND :
            WPFPageHost::hostedPage->DefaultForeBrush = gcnew SolidColorBrush(Colors::Red);
          break;
          case IDC_YELLOWFOREGROUND :
            WPFPageHost::hostedPage->DefaultForeBrush = gcnew SolidColorBrush(Colors::Yellow);
          break;
          default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
      break;
      case WM_PAINT:
        hdc = BeginPaint(hWnd, &ps);
        // TODO: Add any drawing code here...
        EndPaint(hWnd, &ps);
      break;
      case WM_DESTROY:
        PostQuitMessage(0);
      break;
      default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
      case WM_INITDIALOG:
        return (INT_PTR)TRUE;

      case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
      break;
    }
    return (INT_PTR)FALSE;
}

WPFPageHost::WPFPageHost(){} 
HWND GetHwnd(HWND parent, int x, int y, int width, int height)
{
    System::Windows::Interop::HwndSourceParameters^ sourceParams = gcnew System::Windows::Interop::HwndSourceParameters(
    "hi" // NAME
    );
    sourceParams->PositionX = x;
    sourceParams->PositionY = y;
    sourceParams->Height = height;
    sourceParams->Width = width;
    sourceParams->ParentWindow = IntPtr(parent);
    sourceParams->WindowStyle = WS_VISIBLE | WS_CHILD; // style
    System::Windows::Interop::HwndSource^ source = gcnew System::Windows::Interop::HwndSource(*sourceParams);
    WPFPage ^myPage = gcnew WPFPage(width, height);
    //Assign a reference to the WPF page and a set of UI properties to a set of static properties in a class
    //that is designed for that purpose.
    WPFPageHost::hostedPage = myPage;
    WPFPageHost::initBackBrush = myPage->Background;
    WPFPageHost::initFontFamily = myPage->DefaultFontFamily;
    WPFPageHost::initFontSize = myPage->DefaultFontSize;
    WPFPageHost::initFontStyle = myPage->DefaultFontStyle;
    WPFPageHost::initFontWeight = myPage->DefaultFontWeight;
    WPFPageHost::initForeBrush = myPage->DefaultForeBrush;
    myPage->OnButtonClicked += gcnew WPFPage::ButtonClickHandler(WPFButtonClicked);
    source->RootVisual = myPage;
    return (HWND) source->Handle.ToPointer();
}
void WPFButtonClicked(Object ^sender, MyPageEventArgs ^args)
{
    if(args->IsOK) //display data if OK button was clicked
    {
        WPFPage ^myPage = WPFPageHost::hostedPage;
        LPCWSTR userName = (LPCWSTR) InteropServices::Marshal::StringToHGlobalAuto("Name: " + myPage->EnteredName).ToPointer();
        SetWindowText(nameLabel, userName);
        LPCWSTR userAddress = (LPCWSTR) InteropServices::Marshal::StringToHGlobalAuto("Address: " + myPage->EnteredAddress).ToPointer();
        SetWindowText(addressLabel, userAddress);
        LPCWSTR userCity = (LPCWSTR) InteropServices::Marshal::StringToHGlobalAuto("City: " + myPage->EnteredCity).ToPointer();
        SetWindowText(cityLabel, userCity);
        LPCWSTR userState = (LPCWSTR) InteropServices::Marshal::StringToHGlobalAuto("State: " + myPage->EnteredState).ToPointer();
        SetWindowText(stateLabel, userState);
        LPCWSTR userZip = (LPCWSTR) InteropServices::Marshal::StringToHGlobalAuto("Zip: " + myPage->EnteredZip).ToPointer();
        SetWindowText(zipLabel, userZip);
    }
    else
    {
        SetWindowText(nameLabel, L"Name: ");
        SetWindowText(addressLabel, L"Address: ");
        SetWindowText(cityLabel, L"City: ");
        SetWindowText(stateLabel, L"State: ");
        SetWindowText(zipLabel, L"Zip: ");
    }
}

void CreateDataDisplay(HWND hWnd, int top, int left, int width)
{
    dataDisplayLabel = CreateWindowEx(0, L"static", L"Data From WPF Control",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      left, top+25,
                                      width, 25,
                                      hWnd,
                                      (HMENU) 1,
                                      hInst,
                                      NULL);

    nameLabel =        CreateWindowEx(0, L"static", L"Name: ",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      left, top+60,
                                      width, 25,
                                      hWnd,
                                     (HMENU) 1,
                                      hInst,
                                      NULL);

    addressLabel =     CreateWindowEx(0, L"static", L"Address: ",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      left, top+85,
                                      width, 25,
                                      hWnd,
                                      (HMENU) 1,
                                      hInst,
                                      NULL);
    cityLabel =        CreateWindowEx(0, L"static", L"City: ",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      left, top+110,
                                      width, 25,
                                      hWnd,
                                      (HMENU) 1,
                                      hInst,
                                      NULL);
  
    stateLabel =       CreateWindowEx(0, L"static", L"State: ",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      left, top+135,
                                      width, 25,
                                      hWnd,
                                      (HMENU) 1,
                                      hInst,
                                      NULL);
  
    zipLabel =         CreateWindowEx(0, L"static", L"Zip: ",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      left, top+160,
                                      width, 25,
                                      hWnd,
                                      (HMENU) 1,
                                      hInst,
                                      NULL);
  
}

void CreateRadioButtons(HWND hWnd)
{
    //Background color
    int top = 25;
    controlDisplayLabel = CreateWindowEx(0, L"static", L"ControlProperties",
                                      WS_CHILD | WS_VISIBLE | SS_LEFT,
                                      10, top,
                                      125, 25,
                                      hWnd,
                                      NULL,
                                      hInst,
                                      NULL);

    //Background radio buttons
    top += 35;
    backgroundGroup =  CreateWindowEx(0, L"button", L"Background Color",
                                      WS_CHILD | WS_VISIBLE | BS_GROUPBOX,
                                      10, top,
                                      175, 100,
                                      hWnd,
                                      (HMENU) IDC_ORIGINALBACKGROUND,
                                      hInst,
                                      NULL);
    top += 20;
    originalBackgroundButton = CreateWindowEx(0, L"button", L"Original",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_ORIGINALBACKGROUND,
                                      hInst,
                                      NULL);
    top += 20;
    lightGreenBackgroundButton = CreateWindowEx(0, L"button", L"Light Green",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_LIGHTGREENBACKGROUND,
                                      hInst,
                                      NULL);

    top += 20;
    lightSalmonBackgroundButton = CreateWindowEx(0, L"button", L"Light Salmon",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_LIGHTSALMONBACKGROUND,
                                      hInst,
                                      NULL);
    //Foreground color
    top += 45;
    foregroundGroup =  CreateWindowEx(0, L"button", L"Foreground Color",
                                      WS_CHILD | WS_VISIBLE | BS_GROUPBOX,
                                      10, top,
                                      175, 100,
                                      hWnd,
                                      NULL,
                                      hInst,
                                      NULL);
    top += 20;
    originalForegroundButton = CreateWindowEx(0, L"button", L"Original",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_ORIGINALFOREGROUND,
                                      hInst,
                                      NULL);
    top += 20;
    redForegroundButton = CreateWindowEx(0, L"button", L"Red",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_REDFOREGROUND,
                                      hInst,
                                      NULL);

    top += 20;
    yellowForegroundButton = CreateWindowEx(0, L"button", L"Yellow",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_YELLOWFOREGROUND,
                                      hInst,
                                      NULL);

    //Font family
    top += 45;
    fontFamilyGroup =  CreateWindowEx(0, L"button", L"Font Family",
                                      WS_CHILD | WS_VISIBLE | BS_GROUPBOX,
                                      10, top,
                                      175, 100,
                                      hWnd,
                                      NULL,
                                      hInst,
                                      NULL);
    top += 20;
    originalFontFamily = CreateWindowEx(0, L"button", L"Original",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      125, 25,
                                      hWnd,
                                      (HMENU) IDC_ORIGINALFONTFAMILY,
                                      hInst,
                                      NULL);
    top += 20;
    timesFontFamily = CreateWindowEx(0, L"button", L"Times New Roman",
                                      WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                      15, top,
                                      140, 25,
                                      hWnd,
                                      (HMENU) IDC_TIMESNEWROMAN,
                                      hInst,
                                      NULL);

    top += 20;
    wingdingsFontFamily = CreateWindowEx(0, L"button", L"WingDings",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_WINGDINGS,
                                     hInst,
                                     NULL);

    //Font size
    top += 45;
    fontSizeGroup =   CreateWindowEx(0, L"button", L"Font Size",
                                     WS_CHILD | WS_VISIBLE | BS_GROUPBOX,
                                     10, top,
                                     175, 100,
                                     hWnd,
                                     NULL,
                                     hInst,
                                     NULL);
    top += 20;
    originalFontSize = CreateWindowEx(0, L"button", L"Original",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_ORIGINALFONTSIZE,
                                     hInst,
                                     NULL);
    top += 20;
    tenpointFontSize = CreateWindowEx(0, L"button", L"10",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_TENPOINT,
                                     hInst,
                                     NULL);

    top += 20;
    twelvepointFontSize = CreateWindowEx(0, L"button", L"12",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_TWELVEPOINT,
                                     hInst,
                                     NULL);

    //Font style
    top += 45;
    fontStyleGroup = CreateWindowEx(0, L"button", L"Font Style",
                                     WS_CHILD | WS_VISIBLE | BS_GROUPBOX,
                                     10, top,
                                     175, 70,
                                     hWnd,
                                     NULL,
                                     hInst,
                                     NULL);
    top += 20;
    originalFontStyle = CreateWindowEx(0, L"button", L"Original",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_ORIGINALFONTSTYLE,
                                     hInst,
                                     NULL);
    top += 20;
    italicFontStyle = CreateWindowEx(0, L"button", L"Italic",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_ITALIC,
                                     hInst,
                                     NULL);

    //Font weight
    top += 45;
    fontWeightGroup = CreateWindowEx(0, L"button", L"Font Weight",
                                     WS_CHILD | WS_VISIBLE | BS_GROUPBOX,
                                     10, top,
                                     175, 70,
                                     hWnd,
                                     NULL,
                                     hInst,
                                     NULL);
    top += 20;
    originalFontWeight = CreateWindowEx(0, L"button", L"Original",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_ORIGINALFONTWEIGHT,
                                     hInst,
                                     NULL);
    top += 20;
    boldFontWeight =  CreateWindowEx(0, L"button", L"Bold",
                                     WS_CHILD | WS_VISIBLE | BS_AUTORADIOBUTTON,
                                     15, top,
                                     125, 25,
                                     hWnd,
                                     (HMENU) IDC_BOLD,
                                     hInst,
                                     NULL);
}