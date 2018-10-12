// TraditionalAPI.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "TraditionalAPI.h"
#include <math.h>
#include <tchar.h>

//	A global counter.
unsigned int g_uCounter = 0;

TRADITIONALAPI_API void __stdcall TA_IncrementCounter()
{
	g_uCounter++;
}

//	A slightly more complex function, find the square root of a double.
TRADITIONALAPI_API double __stdcall TA_CalculateSquareRoot(double dValue)
{
	return ::sqrt(dValue);
}

//	A function that would require array marshalling, find the dot product
//	of two three tuples.
TRADITIONALAPI_API double __stdcall TA_DotProduct(double arThreeTuple1[], double arThreeTuple2[])
{
	return arThreeTuple1[0] * arThreeTuple2[0] + arThreeTuple1[1] * arThreeTuple2[1] + arThreeTuple1[2] * arThreeTuple2[2];
}

//	Run a test x times.
TRADITIONALAPI_API void __stdcall TA_Test1(unsigned __int64 nTestCount)
{
	for(unsigned __int64 i = 1; i <= nTestCount; i++)
		TA_IncrementCounter();
}

//	Run a test x times.
TRADITIONALAPI_API void __stdcall TA_Test2(unsigned __int64 nTestCount)
{
	for(unsigned __int64 i = 1; i <= nTestCount; i++)
		TA_CalculateSquareRoot((double)i);
}

//	Run a test x times.
TRADITIONALAPI_API void __stdcall TA_Test3(unsigned __int64 nTestCount)
{
	for(unsigned __int64 i = 1; i <= nTestCount; i++)
	{
		double arThreeTuple1[3] = {(double)i, (double)i, (double)i};
		double arThreeTuple2[3] = {(double)i, (double)i, (double)i};
		double result = TA_DotProduct(arThreeTuple1, arThreeTuple2);
	}
}
