#include "stdafx.h"
#include "WPFPage.h"

WPFPage::WPFPage() {}
WPFPage::WPFPage(int allottedWidth, int allotedHeight)
{
  array<ColumnDefinition ^> ^ columnDef = gcnew array<ColumnDefinition ^> (4);
  array<RowDefinition ^> ^ rowDef = gcnew array<RowDefinition ^> (6);

  this->Height = allotedHeight;
  this->Width = allottedWidth;
  this->Background = gcnew SolidColorBrush(Colors::LightGray);
  
  //Set up the Grid's row and column definitions
  for(int i=0; i<4; i++)
  {
    columnDef[i] = gcnew ColumnDefinition();
    columnDef[i]->Width = GridLength(1, GridUnitType::Auto);
    this->ColumnDefinitions->Add(columnDef[i]);
  }
  for(int i=0; i<6; i++)
  {
    rowDef[i] = gcnew RowDefinition();
    rowDef[i]->Height = GridLength(1, GridUnitType::Auto);
    this->RowDefinitions->Add(rowDef[i]);
  }
  //Add the title
  titleText = gcnew Label();
  titleText->Content = "Simple WPF Control";
  titleText->HorizontalAlignment = System::Windows::HorizontalAlignment::Center;
  titleText->Margin = Thickness(10, 5, 10, 0);
  titleText->FontWeight = FontWeights::Bold;
  titleText->FontSize = 14;
  Grid::SetColumn(titleText, 0);
  Grid::SetRow(titleText, 0);
  Grid::SetColumnSpan(titleText, 4);
  this->Children->Add(titleText);
  //Add the Name Label and TextBox
  nameLabel = CreateLabel(0, 1, "Name");
  this->Children->Add(nameLabel);
  nameTextBox = CreateTextBox(1, 1, 3);
  this->Children->Add(nameTextBox);
  //Add the Address Label and TextBox
  addressLabel = CreateLabel(0, 2, "Address");
  this->Children->Add(addressLabel);
  addressTextBox = CreateTextBox(1, 2, 3);
  this->Children->Add(addressTextBox);

  //Add the City Label and TextBox
  cityLabel = CreateLabel(0, 3, "City");
  this->Children->Add(cityLabel);
  cityTextBox = CreateTextBox(1, 3, 1);
  cityTextBox->Width = 100;
  this->Children->Add(cityTextBox);

  //Add the State Label and TextBox
  stateLabel = CreateLabel(2, 3, "State");
  this->Children->Add(stateLabel);
  stateTextBox = CreateTextBox(3, 3, 1);
  stateTextBox->Width = 50;
  this->Children->Add(stateTextBox);

  //Add the Zip Label and TextBox
  zipLabel = CreateLabel(0, 4, "Zip");
  this->Children->Add(zipLabel);
  zipTextBox = CreateTextBox(1, 4, 1);
  this->Children->Add(zipTextBox);
  //Add the Buttons and atttach event handlers
  okButton = CreateButton(0, 5, "OK");
  cancelButton = CreateButton(1, 5, "Cancel");
  this->Children->Add(okButton);
  this->Children->Add(cancelButton);
  okButton->Click += gcnew RoutedEventHandler(this, &WPFPage::ButtonClicked);
  cancelButton->Click += gcnew RoutedEventHandler(this, &WPFPage::ButtonClicked);
  //Set the default font properties
  DefaultFontFamily = nameLabel->FontFamily;
  DefaultFontStyle = nameLabel->FontStyle;
  DefaultFontSize = nameLabel->FontSize;
  DefaultFontWeight = nameLabel->FontWeight;
  DefaultForeBrush = nameLabel->Foreground;

}
Label ^WPFPage::CreateLabel(int column, int row, String ^ text)
{
  Label ^ newLabel = gcnew Label();
  newLabel->Content = text;
  newLabel->Margin = Thickness(10, 5, 10, 0);
  newLabel->FontWeight = FontWeights::Normal;
  newLabel->FontSize = 12;
  Grid::SetColumn(newLabel, column);
  Grid::SetRow(newLabel, row);
  return newLabel;
}
TextBox ^WPFPage::CreateTextBox(int column, int row, int span)
{
  TextBox ^newTextBox = gcnew TextBox();
  newTextBox->Margin = Thickness(10, 5, 10, 0);
  Grid::SetColumn(newTextBox, column);
  Grid::SetRow(newTextBox, row);
  Grid::SetColumnSpan(newTextBox, span);
  return newTextBox;
}
Button ^WPFPage::CreateButton(int column, int row, String ^text)
{
  Button ^newButton = gcnew Button();
  newButton->Content = text;
  newButton->Margin = Thickness(10, 10, 10, 10);
  newButton->Width = 60;
  Grid::SetColumn(newButton, column);
  Grid::SetRow(newButton, row);
  return newButton;
}
void WPFPage::ButtonClicked(Object ^sender, RoutedEventArgs ^args)
{

  //TODO: validate input data
  bool okClicked = true;
  if(sender == cancelButton)
    okClicked = false;
  EnteredName = nameTextBox->Text;
  EnteredAddress = addressTextBox->Text;
  EnteredCity = cityTextBox->Text;
  EnteredState = stateTextBox->Text;
  EnteredZip = zipTextBox->Text;
  OnButtonClicked(this, gcnew MyPageEventArgs(okClicked));
}
void WPFPage::SetFontFamily(FontFamily^ newFontFamily)
{
  _defaultFontFamily = newFontFamily;
  titleText->FontFamily = newFontFamily;
  nameLabel->FontFamily = newFontFamily;
  addressLabel->FontFamily = newFontFamily;
  cityLabel->FontFamily = newFontFamily;
  stateLabel->FontFamily = newFontFamily;
  zipLabel->FontFamily = newFontFamily;
}
void WPFPage::SetFontStyle(FontStyle newFontStyle)
{
  _defaultFontStyle = newFontStyle;
  titleText->FontStyle = newFontStyle;
  nameLabel->FontStyle = newFontStyle;
  addressLabel->FontStyle = newFontStyle;
  cityLabel->FontStyle = newFontStyle;
  stateLabel->FontStyle = newFontStyle;
  zipLabel->FontStyle = newFontStyle;
}

void WPFPage::SetFontSize(double newFontSize)
{
  _defaultFontSize = newFontSize;
  titleText->FontSize = newFontSize;
  nameLabel->FontSize = newFontSize;
  addressLabel->FontSize = newFontSize;
  cityLabel->FontSize = newFontSize;
  stateLabel->FontSize = newFontSize;
  zipLabel->FontSize = newFontSize;
}

void WPFPage::SetFontWeight(FontWeight newFontWeight)
{
  _defaultFontWeight = newFontWeight;
  titleText->FontWeight = newFontWeight;
  nameLabel->FontWeight = newFontWeight;
  addressLabel->FontWeight = newFontWeight;
  cityLabel->FontWeight = newFontWeight;
  stateLabel->FontWeight = newFontWeight;
  zipLabel->FontWeight = newFontWeight;
}

void WPFPage::SetForeBrush(Brush^ newForeBrush)
{
  _defaultForeBrush = newForeBrush;
  titleText->Foreground = newForeBrush;
  nameLabel->Foreground = newForeBrush;
  addressLabel->Foreground = newForeBrush;
  cityLabel->Foreground = newForeBrush;
  stateLabel->Foreground = newForeBrush;
  zipLabel->Foreground = newForeBrush;
}

MyPageEventArgs::MyPageEventArgs() {}

MyPageEventArgs::MyPageEventArgs(bool okClicked)
{
  IsOK = okClicked;
}
