   M S B A M L   `   `   `  ����  ConverterApi    ConverterApi.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��Из валюты:��6!   $��Center=�63   $��0,0,5,0q�6N   �� 5   
   -   5         ��Name$ FromCurrencyComboBox��5   0   $��1��6   $��1��6     �� SelectedIndex$ 0��5      $	��0,5q�6%   �� 5   
   $��2��6   $��В валюту:��6!   $��Center=�62   $��0,0,5,0q�6M   �� 5   
   -   5        $ ToCurrencyComboBox��5   0   $��2��6   $��1��6    $ 1��5      $	��0,5q�6%   �� 5   
   $��3��6   $��Сумма:��6!   $��Center=�6/   $��0,0,5,0q�6J   �� 5   
   -   5         ��Name$ AmountTextBox��5   /   $��3��6   $��1��6   $	��0,5q�5      $��1��6   �� 5!   
   -   5        $��4��5!      $��2��6   $"��Конвертировать��5"      $
��0,10q�6*   $��30��68   5#      �� 5%   
   -   5         ��Name$ ResultTextBlock��5%   5   $��5��6   $��2��6!   $��Center=�5&      $��
0,55, 0, 0q�61   5'      5(      