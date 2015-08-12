#include "stdafx.h"

using namespace System;
using namespace System::Windows;
using namespace System::Windows::Documents;
using namespace System::Threading;
using namespace System::Windows::Controls;
using namespace System::Windows::Media;

public ref class MyPageEventArgs : EventArgs
{
public:
  MyPageEventArgs();
  MyPageEventArgs(bool isOK);

  property bool IsOK;
};

public ref class WPFPage : public Grid
{
private:
  Label ^titleText, ^nameLabel, ^addressLabel, ^cityLabel, ^stateLabel, ^zipLabel;
  TextBox ^nameTextBox, ^addressTextBox, ^cityTextBox, ^stateTextBox, ^zipTextBox;
  Button ^okButton, ^cancelButton;
  String ^_name, ^_address, ^_city, ^_state, ^_zip;
  FontFamily ^_defaultFontFamily;
  void SetFontFamily(FontFamily^ newFont);
  FontStyle _defaultFontStyle;
  void SetFontStyle(FontStyle newFont);
  double _defaultFontSize;
  void SetFontSize(double newSize);
  FontWeight _defaultFontWeight;
  void SetFontWeight(FontWeight newWeight);
  Brush^ _defaultForeBrush;
  void SetForeBrush(Brush^ newForeBrush);

  Label ^ CreateLabel(int column, int row, String ^text);
  TextBox ^ CreateTextBox(int column, int row, int span);
  Button ^CreateButton(int column, int row, String ^text);
  void ButtonClicked(Object ^sender, RoutedEventArgs ^args);

public:
  delegate void ButtonClickHandler(Object ^, MyPageEventArgs ^);
  WPFPage();
  WPFPage(int height, int width);
  event ButtonClickHandler ^OnButtonClicked;
  //properties
  property FontFamily^ DefaultFontFamily
  {
    FontFamily^ get() {return _defaultFontFamily;}
    void set(FontFamily^ value) {SetFontFamily(value);}
  };
  property FontStyle DefaultFontStyle
  {
    FontStyle get() {return _defaultFontStyle;}
    void set(FontStyle value) {SetFontStyle(value);}
  };
  property double DefaultFontSize
  {
    double get() {return _defaultFontSize;}
    void set(double value) {SetFontSize(value);}
  };
  property FontWeight DefaultFontWeight
  {
    FontWeight get() {return _defaultFontWeight;}
    void set(FontWeight value) {SetFontWeight(value);}
  };

  property Brush^ DefaultForeBrush
  {
    Brush^ get() {return _defaultForeBrush;}
    void set(Brush^ value) {SetForeBrush(value);}
  };

  property String ^EnteredName;
  property String ^EnteredAddress;
  property String ^EnteredCity;
  property String ^EnteredState;
  property String ^EnteredZip;
};



