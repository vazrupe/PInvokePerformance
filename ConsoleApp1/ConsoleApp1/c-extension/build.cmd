g++ -O3 -c -DTRADITIONALAPI_EXPORTS api.cpp
g++ -shared -o ../example.dll api.o -Wl,-add-stdcall-alias
del api.o

pause>nul