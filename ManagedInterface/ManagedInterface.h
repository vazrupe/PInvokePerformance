// ManagedInterface.h

#pragma once

#include "..\TraditionalAPI\TraditionalAPI.h"

using namespace System;

namespace ManagedInterface {

	public ref class ManagedInterface
	{
	public:

		void IncrementCounter()
		{
			//	Call the unmanaged function.
			::TA_IncrementCounter();
		}

		double CalculateSquareRoot(double value)
		{
			//	Call the unmanaged function.
			return ::TA_CalculateSquareRoot(value);
		}

		double DotProduct(array<double>^ threeTuple1, array<double>^ threeTuple2)
		{
			//	Pin the arrays.
			pin_ptr<double> p1(&threeTuple1[0]);
			pin_ptr<double> p2(&threeTuple2[0]);
			
			//	Call the unmanaged function.
			return TA_DotProduct(p1, p2);
		}
	};
}
