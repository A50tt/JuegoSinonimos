   M S B A M L   `   `   `  ���� I  EJuegoSinonimos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:JuegoSinonimosJuegoSinonimos      JuegoSinonimos.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:JuegoSinonimos       � Title$   El Juego de los Sinónimos��$	��612��$	��640�� � Icon$ SinonimosGameIcon.ico��.���  � RowDefinitions �� $
��303* ��� $
��278* �����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black�� � Offset$ 0Q�� $ 	#FF00A2FF��$ 0.314Q� � ColumnDefinitions �� $	��97* ��� $
��257* �.+��� -    ��Name$ UrlConsoleText��	 �� 
IsReadOnly	 . $
��Left=�$��18��$��
46,102,0,0q�
 �� TextWrapping$

 Wrap=�$	��Top=�$	��486��$��2��
��������� -   $ SynonymsConsoleText��	 . $
��Left=�$��88��$��
46,227,0,0q�$

 Wrap=�$	��Top=�$	��486��$��2��
�������$��2���� -    ��Name$ UrlLabel��$��URL sinónimos:��$
��Left=�$��	46,77,0,0q�$	��Top=�$��11(�
��������� -   $ SynonymsLabel��$��Sinónimos:��$
��Left=�$��
46,202,0,0q�$	��Top=�$��11(�
��������� -    ��Name$ CheckAnswerButton��$��Enter��$
��Left=�$��
65,168,0,0q�$	��Top=�$��71��$��1��$��22��$��11(����	#FF00DCFF�� -   $ RandomWordLabel��$��Center=�$��RANDOM WORD��$
��Left=�$��
133,24,0,0q�$	��Top=�$��36(�$	��304��$��65��
�������$��2����Z� �� -   $ RefreshRandomButton��$��Pasar��$
��Left=�$��	46,44,0,0q�$	��Top=�$��35��
���   �
�������
���   ��� -   $ WordSelectorTextBox��$��3q�
���   �$
��Left=�$��
46,168,0,0q�$

 Wrap=�$	��Top=�$	��178��$��2�����	#FF00DCFF J� RenderTransformOrigin$ 0.5,0.5&��� -	   $ RespuestaLabel��$��
Respuesta:��$
��Left=�$��
46,138,0,0q�$	��Top=�$��66��$��11(�
��������� -
   $ MusicButton��$��
Reproducir��$��1��$
��Left=�$��267,191,0,0q�$	��Top=�$��71��$��18��$��11(����	#FFB446B9
��������� -   $ StopMusicButton��$��Detener��$��1��$
��Left=�$��356,191,0,0q�$	��Top=�$��71��$��18��$��11(����	#FFB446B9
��������� -   $ ShowSynonymsButton��$��
Respuestas��$��1��$
��Left=�$��153,168,0,0q�$	��Top=�$��71��$��22��$��11(����	#FFFF5252�� $��1��	 . $
��Left=�$��34��$��
72,125,0,0q�$

 Wrap=�$8��2Pulsa la tecla "ENTER" o el botón para confirmar.��$	��Top=�$	��140����Z� ��Z� �� -   $ NowPlayingLabel��$��Reproduciendo...��$��1��$
��Left=�$��295,133,0,0q�$	��Top=��� -   $ SongNameLabel��$��.��$��1��$
��Left=�$��282,159,0,0q�$	��Top=��� -   $ DefinitionLabel��$��Definición:��$
��Left=�$��	46,30,0,0q�$	��Top=�$��11(�
�������$��1���� -   $ DefinitionConsoleText��	 . $
��Left=�$	��177��$��	46,55,0,0q�$

 Wrap=�$	��Top=�$	��486��$��2��
�������$��1��