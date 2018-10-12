// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the TRADITIONALAPI_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// TRADITIONALAPI_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef TRADITIONALAPI_EXPORTS
#define TRADITIONALAPI_API extern "C"   __declspec(dllexport) 
#else
#define TRADITIONALAPI_API extern "C" __declspec(dllimport)
#endif

#include <Windows.h>

//	The simplest function, increment a counter.

TRADITIONALAPI_API void __stdcall TA_IncrementCounter();

//	A slightly more complex function, find the square root of a double.
TRADITIONALAPI_API double __stdcall TA_CalculateSquareRoot(double dValue);

//	A function that would require array marshalling, find the dot product
//	of two three tuples.
TRADITIONALAPI_API double __stdcall TA_DotProduct(double arThreeTuple1[], double arThreeTuple2[]);

//	Run each test x times.
TRADITIONALAPI_API void __stdcall TA_Test1(unsigned __int64 nTestCount);
TRADITIONALAPI_API void __stdcall TA_Test2(unsigned __int64 nTestCount);
TRADITIONALAPI_API void __stdcall TA_Test3(unsigned __int64 nTestCount);
