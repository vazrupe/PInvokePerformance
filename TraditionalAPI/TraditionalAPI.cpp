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
double test2Temp = 0.0;
TRADITIONALAPI_API void __stdcall TA_Test2(unsigned __int64 nTestCount)
{
	for(unsigned __int64 i = 1; i <= nTestCount; i++)
		test2Temp = TA_CalculateSquareRoot((double)i);
}

//	Run a test x times.
double test3Temp = 0.0;
TRADITIONALAPI_API void __stdcall TA_Test3(unsigned __int64 nTestCount)
{
	for(unsigned __int64 i = 1; i <= nTestCount; i++)
	{
		double arThreeTuple1[3] = {(double)i + 0.1, (double)i + 0.2, (double)i + 0.3 };
		double arThreeTuple2[3] = {(double)i - 0.7, (double)i - 0.6, (double)i - 0.5 };
		test3Temp = TA_DotProduct(arThreeTuple1, arThreeTuple2);
	}
}


TRADITIONALAPI_API double __stdcall Dummy()
{
	return g_uCounter + test2Temp + test3Temp;;
}

TRADITIONALAPI_API double __stdcall ComplexCpp(double arr1[], double arr2[], __int32 length)
{
	double r = 0.0;
	for (int i = 0; i < length; i++)
	{
		r = r + arr1[i] * arr2[i];
		r = r - arr1[i] * arr2[length - i - 1];
		r = ::sqrt(r);
	}
	return r;
}

TRADITIONALAPI_API double __stdcall ComplexCpp2(__int32 num)
{
	double n = num;

	while (n > 1.0)
	{
		n = n / 1.00000001;
	}
	return n;
}

TRADITIONALAPI_API double __stdcall ComplexCppUnman(__int32 nTestCount)
{
	double * arr = new double[nTestCount];
	for (__int32 i = 0; i < nTestCount; i++)
	{
		arr[i] = i;
	}
	return ComplexCpp(arr, arr, nTestCount);
}

TRADITIONALAPI_API double __stdcall ComplexCpp2Unman(__int32 num)
{
	return ComplexCpp2(num);
}
