   M S B A M L   `   `   `  ����I  ECibush_Shtahim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Cibush_Shtahim.ViewsCibush_Shtahim.Views  5      E'clr-namespace:Cibush_Shtahim.ViewModelsCibush_Shtahim.ViewModels  5          Cibush_Shtahim.Views.game   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Cibush_Shtahim.Views     5      6vm'clr-namespace:Cibush_Shtahim.ViewModels     5      -   5          >�Name$  GameUserControl��5      .��5      � 
������ � ColumnDefinitions 5   
   �� 5      $
��150* �6   �� 5      $
��650* �6   .+�5   
   �� -   5         ��Name$ GamePointsStackPanel��5      $��0��64   .+�5      �� -   5         ��Name$ 
exitButton��5      $��30��6B   $��70��6N   $��10,10q�5      $
��Left=�6   $��	Exit Game��68   � 5      $	��140��6   $��	0,100,0,0q�6    .+�5      �� -   5         ��Name$ PrectengeCaculateTab��5   7   $��Center=�6   $��20(�6U   $��Capture��6d   �� 5       -   5        $��PrecentegeTabToPass��5       $
��Left=�67   $��30��6R   $��50,25,50,15q�5!      $��25(�6'   �� 5"      -   5        $��
Precentage��5"      $��80,25,50,15q�6.   $��25(�6C   $��%��6Q   �� 5%      -   5        $��TextToPercentageToPass��5%      $��23(�66   $��	0,60,10,0q�6D   $��Center=�6W   	to pass�� 5&      �� 5'      $��
next level��6   $��10,0,0,0q�6-   5(      5)      5.      � 5/   
   $��1��6    � RowDefinitions 50      �� 51      $	��70* �6    �� 52      $
��480* �6    .+�54      � $��0��6   .+�5<      �� -   5         ��Name$ Heart1��5<      $
��Left=�6.   $	��Top=�6I   $y�Visible=�6a   $5��//Cibush_shtahim;component/images/laser-gun1.png��5=      $��
40,10,0,10q�6M   �� 5>      -	   5        $ Heart2��5>      $
��Left=�6*   $��140,10,0,10q�6E   $	��Top=�6[   $y�Visible=�6s   $5��//Cibush_shtahim;component/images/laser-gun1.png��5?      �� 5@      -
   5        $ Heart3��5@      $
��Left=�6*   $��240,10,0,10q�6E   $	��Top=�6\   $y�Visible=�6t   $5��//Cibush_shtahim;component/images/laser-gun1.png��5A      �� 5D      $��20(�6   $��DemiBold"�6*   $��
400,20,0,0q�6@   score �� 5F      -   5        $ PointTab��5G   !   $��25(�5F      $��	3,2.5,0,0q�6+   $��Medium"�5G      ��63   �� *PointsHolder+5H      5L      �� 5Q      -   5         ��Name$ 
gameCanvas��5R      $��1��5Q      $	��480��6#   $	��650��60   $	��Top=�6<   |�. $
��Left=�5S      ��5T      �� 5U      -   5        BacgroungGame1�� j� Stretch$
 Fill=�5U   ;   	 �� ImageSource$/	 )/Cibush_shtahim;component/images/Reka.jpg��6K   5a      5b      5h      5i      