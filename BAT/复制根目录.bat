
::注释
@echo off

set logfilename=log_复制根目录_%date:~0,4%_%date:~5,2%_%date:~8,2%

echo,
echo 复制BridgeServer根目录......
echo 复制BridgeServer根目录......>>C:\Users\s2479\Desktop\BS自动更新\%logfilename%.txt
echo,

echo %date:~0,4%年%date:~5,2%月%date:~8,2%日%time:~0,8%:
echo %date:~0,4%年%date:~5,2%月%date:~8,2%日%time:~0,8%:>>C:\Users\s2479\Desktop\BS自动更新\%logfilename%.txt
echo 开始复制BridgeServer根目录......
echo 开始复制BridgeServer根目录......>>C:\Users\s2479\Desktop\BS自动更新\%logfilename%.txt
echo,

echo %date:~0,4%年%date:~5,2%月%date:~8,2%日%time:~0,8%:
echo %date:~0,4%年%date:~5,2%月%date:~8,2%日%time:~0,8%:>>C:\Users\s2479\Desktop\BS自动更新\%logfilename%.txt
echo 开始复制BridgeServer根目录，请稍后......
echo 开始复制BridgeServer根目录，请稍后......>>C:\Users\s2479\Desktop\BS自动更新\%logfilename%.txt
robocopy \\10.130.16.160\BridgeServer C:\Users\s2479\Desktop\BS自动更新\BridgeServer /R:5 /W:5 /E /MAX:1 /TEE /LOG+:C:\Users\s2479\Desktop\BS自动更新\%logfilename%.txt
echo,
echo,
echo,

pause
