 public class KeyOperandModel : KeyModel
 {
     public uint Value { get; }
     
     public KeyOperandModel(KeyActionType actionType, uint value, string descriptor,  int size) : base(actionType, descriptor, size)
     {
         Value = value;
     }
 }