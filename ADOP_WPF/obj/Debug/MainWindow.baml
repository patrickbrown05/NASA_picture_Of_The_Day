   M S B A M L   `   `   `  ����C  ?ADOP_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ADOP_WPFADOP_WPF  5   	       ADOP_WPF.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:ADOP_WPF     5   	   -   5          � Title$  
MainWindow��5   	   $	��552��6   $	��999��6)   .��5	      � $��0,0,2,2q�6   .+�5
   
   (  "System.Windows.Controls.DatePicker  -   5          Name$ MonthCalender��5
      $
��Left=�6,   $��	10,10,0,0q�6G   $	��Top=�6Z   $	��185��6r   6~   �� 5   
   -   5         ��Name$ LaunchButton��5      $��Go to APOD date��6'   $
��Left=�6A   $��	10,50,0,0q�6\   $	��Top=�6o   $	��185��6�   6�   �� 5   
   $��Settings��6   $
��Left=�6%   $	��110��6@   $��	10,85,0,0q�6M   $	��Top=�6`   $	��190��6x   �� 5   
   -   5         ��Name$ ShowTodaysImageCheckbox��5      $$��Show today's image on start up��64   $
��Left=�6]   $��
12,115,0,0q�6x   $	��Top=�6�   $	��188��6�   $��20��6�   �� 5   
   -   5        $ LimitRangeCheckBox��5      $!��limit range to current year��6/   $
��Left=�6U   $��
12,140,0,0q�6p   $	��Top=�6�   6�   �� 5   
   $��images downloaded today:��6   $
��Left=�63   $��
12,160,0,0q�6N   $	��Top=�6b   �� 5   
   -   5         ��Name$ ImagesTodayTextBox��5      $
��Left=�6.   $��23��6I   $��165,163,0,0q�6U    �� TextWrapping$
 Wrap=�6j   $�� ��6~   $	��Top=�6�   $��30��6�    J� RenderTransformOrigin$ -1.333,1.304&�6�   ���	#FFFFFFC0 �� 
IsReadOnly . 6�   �� 5   
   $!��Image Credit and Copyright:��6   $
��Left=�66   $��
12,239,0,0q�6Q   $	��Top=�6e   �� 5   
   -   5        $ ImageCopyrightTextbox��5      $
��Left=�61   $��50��6L   $��
12,270,0,0q�6X   $
 Wrap=�6l   $�� ��6�   $	��Top=�6�   $	��188��6�    . ���	#FFFFFFC06�   �� 5   
   $��Description:��6   $
��Left=�6'   $��
12,364,0,0q�6B   $	��Top=�6V   �� 5   
   -   5        $ DescriptionTextBox��5      $
��Left=�6.   $	��104��6I   $��
12,405,0,0q�6V   $
 Wrap=�6j   $�� ��6~   $	��Top=�6�   $	��967��6�    . ���	#FFFFFFC06�   �� 5   
   -	   5         ��Name$ PictureBorder��5      
���   �$��1q�6<   $
��Left=�6P   $	��380��6k   $��
205,10,0,0q�6x   $	��Top=�6�   $	��774��6�   ._�5      �� -
   5        	 ��Name$	 ImagePictureBox��5      $��0,0,0,-1q�6-   5      5      5      