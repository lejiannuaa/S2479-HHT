
::ע��
@echo off

set logfilename=log_���Ƹ�Ŀ¼_%date:~0,4%_%date:~5,2%_%date:~8,2%

echo,
echo ����BridgeServer��Ŀ¼......
echo ����BridgeServer��Ŀ¼......>>C:\Users\s2479\Desktop\BS�Զ�����\%logfilename%.txt
echo,

echo %date:~0,4%��%date:~5,2%��%date:~8,2%��%time:~0,8%:
echo %date:~0,4%��%date:~5,2%��%date:~8,2%��%time:~0,8%:>>C:\Users\s2479\Desktop\BS�Զ�����\%logfilename%.txt
echo ��ʼ����BridgeServer��Ŀ¼......
echo ��ʼ����BridgeServer��Ŀ¼......>>C:\Users\s2479\Desktop\BS�Զ�����\%logfilename%.txt
echo,

echo %date:~0,4%��%date:~5,2%��%date:~8,2%��%time:~0,8%:
echo %date:~0,4%��%date:~5,2%��%date:~8,2%��%time:~0,8%:>>C:\Users\s2479\Desktop\BS�Զ�����\%logfilename%.txt
echo ��ʼ����BridgeServer��Ŀ¼�����Ժ�......
echo ��ʼ����BridgeServer��Ŀ¼�����Ժ�......>>C:\Users\s2479\Desktop\BS�Զ�����\%logfilename%.txt
robocopy \\10.130.16.160\BridgeServer C:\Users\s2479\Desktop\BS�Զ�����\BridgeServer /R:5 /W:5 /E /MAX:1 /TEE /LOG+:C:\Users\s2479\Desktop\BS�Զ�����\%logfilename%.txt
echo,
echo,
echo,

pause
