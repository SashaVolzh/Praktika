   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5   	   $	��275��6   $	��700��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��0* �6   �� 5      .+�5   
   �� -   5         ��Name$ label1��5      $��Договоры��6    $
��Left=�63   $��27��6N   $��3,10,0,0q�6Z   $	��Top=�6l   $��83��6�   $��1��6�   �� 5   
   -   5         ��Name$ Drafts��5      $
��Left=�6#   $��21��6>   $��3,42,0,0q�6J   $	��Top=�6\   $	��672��6t    �� 
IsEditable$
 True��6�   $��1��6�   �� 5   
   -   5        $ label2��5      $��
Файлы��6    $
��Left=�60   $��27��6K   $��3,68,0,0q�6W   $	��Top=�6i   $��83��6�   $��1��6�   �� 5   
   -   5         ��Name$ AddFiles��5      $!��Добавить файлы��6#   $��2��6<   $��24��6P   $��440,70,142,0q�6\   $	��Top=�6r    J� RenderTransformOrigin$ 
0.317,0.35&�6�   �� 5   
   -   5        $ DownloadFiles��5      $��Сохранить все��6(   $��2��6@   $
��Left=�6T   $��24��6o   $��
573,70,0,0q�6{   $	��Top=�6�   $	��102��6�   $ 
0.446,0.41&�6�   �� 5   
   -   5         ��Name$ Files��5      $��2��6!   $
��Left=�65   $��
6,100,0,58q�6P   $	��669��6d    � ContextMenu 5      �� 5      .@�5      m� -   5        $��SaveItem��5      $��Сохранить��6/   m� 5      -   5        $��
DeleteItem��5      $��Удалить��62   5      5      �� 5   
   -	   5        $��SaveDB��5      $ ��Сохранить в БД��6    $��2��69   $
��Left=�6M   $��544,206,0,0q�6h   $	��Top=�6}   $��28��6�   $	��131��6�   5      5      