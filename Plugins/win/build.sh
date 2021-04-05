# sudo apt install gcc-mingw-w64-i686
i686-w64-mingw32-gcc -shared -o MicrowitchPlugin.dll MicrowitchPlugin.c winMicrowitchOps.c -lsetupapi -Wl,-k,--output-def,Dlltest.def,--out-implib,libtest.a
#i686-w64-mingw32-gcc -o winMicrowitchOps.exe -DMAIN winMicrowitchOps.c
