 using System;
 using System.Collections.Generic;

 public class ExpressionCalculationProvider : IExpressionCalculationProvider
 {
     private static readonly IDictionary<OperatorType, Func<uint, uint, (uint, bool)>> Calculation = new Dictionary<OperatorType, Func<uint, uint, (uint, bool)>>
     {
         { OperatorType.Sum, Sum },
         { OperatorType.Sub, Sub },
         { OperatorType.Mul, Mul },
         { OperatorType.Div, Div },
     };

     public bool TryCalculate(ExpressionValueNode arg1, ExpressionValueNode arg2, ExpressionActionNode action, out ExpressionValueNode result)
     {
         result = null;
         var calculationResult = Calculation[action.Value](arg1.Value, arg2.Value);

         if (!calculationResult.Item2) return false;
         
         result = new ExpressionValueNode();
         result.Modify(calculationResult.Item1);

         return true;
     }

     private static (uint, bool) Sum(uint arg1, uint arg2) => (arg1 + arg2, true);
     private static (uint, bool) Sub(uint arg1, uint arg2) => (arg1 - arg2, true);
     private static (uint, bool) Mul(uint arg1, uint arg2) => (arg1 * arg2, true);

     private static (uint, bool) Div(uint arg1, uint arg2)
     {
         return arg2 == 0 ? 
             ((uint, bool))(0, false) : 
             ((uint)((float)arg1 / arg2), true);
     }
 }